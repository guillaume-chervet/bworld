using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Demo.Mvc.Core
{
    /// <summary>
    ///     This ICorsService should be used in place of the official default CorsService to support origins
    ///     like http://*.example.comwhich will allow any subdomain for example.com
    /// </summary>
    public class WildCardCorsService : ICorsService
    {
        private readonly CorsOptions _options;

        /// <summary>
        ///     Creates a new instance of the <see cref="CorsService" />.
        /// </summary>
        /// <param name="options">The option model representing <see cref="CorsOptions" />.</param>
        public WildCardCorsService(IOptions<CorsOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _options = options.Value;
        }

        /// <inheritdoc />
        public CorsResult EvaluatePolicy(HttpContext context, CorsPolicy policy)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (policy == null) throw new ArgumentNullException(nameof(policy));

            var corsResult = new CorsResult();
            var accessControlRequestMethod = context.Request.Headers[CorsConstants.AccessControlRequestMethod];
            if (string.Equals(context.Request.Method, CorsConstants.PreflightHttpMethod, StringComparison.Ordinal) &&
                !StringValues.IsNullOrEmpty(accessControlRequestMethod))
                EvaluatePreflightRequest(context, policy, corsResult);
            else
                EvaluateRequest(context, policy, corsResult);

            return corsResult;
        }

        /// <inheritdoc />
        public virtual void ApplyResult(CorsResult result, HttpResponse response)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (response == null) throw new ArgumentNullException(nameof(response));

            var headers = response.Headers;

            if (result.AllowedOrigin != null) headers[CorsConstants.AccessControlAllowOrigin] = result.AllowedOrigin;

            if (result.VaryByOrigin) headers["Vary"] = "Origin";

            if (result.SupportsCredentials) headers[CorsConstants.AccessControlAllowCredentials] = "true";

            if (result.AllowedMethods.Count > 0)
            {
                // Filter out simple methods
                var nonSimpleAllowMethods = result.AllowedMethods
                    .Where(m =>
                        !CorsConstants.SimpleMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                if (nonSimpleAllowMethods.Length > 0)
                    headers.SetCommaSeparatedValues(
                        CorsConstants.AccessControlAllowMethods,
                        nonSimpleAllowMethods);
            }

            if (result.AllowedHeaders.Count > 0)
            {
                // Filter out simple request headers
                var nonSimpleAllowRequestHeaders = result.AllowedHeaders
                    .Where(header =>
                        !CorsConstants.SimpleRequestHeaders.Contains(header, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                if (nonSimpleAllowRequestHeaders.Length > 0)
                    headers.SetCommaSeparatedValues(
                        CorsConstants.AccessControlAllowHeaders,
                        nonSimpleAllowRequestHeaders);
            }

            if (result.AllowedExposedHeaders.Count > 0)
            {
                // Filter out simple response headers
                var nonSimpleAllowResponseHeaders = result.AllowedExposedHeaders
                    .Where(header =>
                        !CorsConstants.SimpleResponseHeaders.Contains(header, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                if (nonSimpleAllowResponseHeaders.Length > 0)
                    headers.SetCommaSeparatedValues(
                        CorsConstants.AccessControlExposeHeaders,
                        nonSimpleAllowResponseHeaders);
            }

            if (result.PreflightMaxAge.HasValue)
                headers[CorsConstants.AccessControlMaxAge]
                    = result.PreflightMaxAge.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Looks up a policy using the <paramref name="policyName" /> and then evaluates the policy using the passed in
        ///     <paramref name="context" />.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="policyName"></param>
        /// <returns>
        ///     A <see cref="CorsResult" /> which contains the result of policy evaluation and can be
        ///     used by the caller to set appropriate response headers.
        /// </returns>
        public CorsResult EvaluatePolicy(HttpContext context, string policyName)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var policy = _options.GetPolicy(policyName);
            return EvaluatePolicy(context, policy);
        }

        public virtual void EvaluateRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            if (!OriginIsAllowed(origin, policy)) return;

            AddOriginToResult(origin, policy, result);
            result.SupportsCredentials = policy.SupportsCredentials;
            AddHeaderValues(result.AllowedExposedHeaders, policy.ExposedHeaders);
        }

        public virtual void EvaluatePreflightRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            if (!OriginIsAllowed(origin, policy)) return;

            var accessControlRequestMethod = context.Request.Headers[CorsConstants.AccessControlRequestMethod];
            if (StringValues.IsNullOrEmpty(accessControlRequestMethod)) return;

            var requestHeaders =
                context.Request.Headers.GetCommaSeparatedValues(CorsConstants.AccessControlRequestHeaders);

            if (!policy.AllowAnyMethod && !policy.Methods.Contains(accessControlRequestMethod)) return;

            if (!policy.AllowAnyHeader &&
                requestHeaders != null &&
                !requestHeaders.All(header =>
                    CorsConstants.SimpleRequestHeaders.Contains(header, StringComparer.OrdinalIgnoreCase) ||
                    policy.Headers.Contains(header, StringComparer.OrdinalIgnoreCase)))
                return;

            AddOriginToResult(origin, policy, result);
            result.SupportsCredentials = policy.SupportsCredentials;
            result.PreflightMaxAge = policy.PreflightMaxAge;
            result.AllowedMethods.Add(accessControlRequestMethod);
            AddHeaderValues(result.AllowedHeaders, requestHeaders);
        }

        protected virtual bool OriginIsAllowed(string origin, CorsPolicy policy)
        {
            if (!string.IsNullOrWhiteSpace(origin) &&
                (policy.AllowAnyOrigin ||
                 policy.Origins.Contains(origin) ||
                 IsWildCardSubdomainMatch(origin, policy)))
                return true;

            return false;
        }

        private void AddOriginToResult(string origin, CorsPolicy policy, CorsResult result)
        {
            if (policy.AllowAnyOrigin)
                if (policy.SupportsCredentials)
                {
                    result.AllowedOrigin = origin;
                    result.VaryByOrigin = true;
                }
                else
                {
                    result.AllowedOrigin = CorsConstants.AnyOrigin;
                }
            else
                result.AllowedOrigin = origin;
        }

        private static void AddHeaderValues(IList<string> target, IEnumerable<string> headerValues)
        {
            if (headerValues == null) return;

            foreach (var current in headerValues) target.Add(current);
        }

        private bool IsWildCardSubdomainMatch(string origin, CorsPolicy policy)
        {
            var actualOriginUri = new Uri(origin);
            var actualOriginRootDomain = GetRootDomain(actualOriginUri);

            foreach (var o in policy.Origins)
            {
                if (!o.Contains("*"))
                    continue;

                // 1) CANNOT USE System.Text.RegularExpression since it does not exist in .net platform 5.4 (which the Microsoft.AspNet.Cors project.json targets)
                // 2) '*' char is not valid for creation of a URI object so we replace it just for this comparison
                var allowedOriginUri = new Uri(o.Replace("*", "SOMELETTERS"));
                if (allowedOriginUri.Scheme == actualOriginUri.Scheme &&
                    actualOriginRootDomain == GetRootDomain(allowedOriginUri))
                    return true;
            }

            return false;
        }

        private string GetRootDomain(Uri uri)
        {
            //Got this snippet here http://stackoverflow.com/questions/16473838/get-domain-name-of-a-url-in-c-sharp-net
            var host = uri.Host;
            int index = host.LastIndexOf('.'), last = 3;

            while (index > 0 && index >= last - 3)
            {
                last = index;
                index = host.LastIndexOf('.', last - 1);
            }

            return host.Substring(index + 1);
        }
    }

    /// <summary>
    ///     Needed to copy these in since some of them are internal to the Microsoft.AspNet.Cors project
    /// </summary>
    public static class CorsConstants
    {
        /// <summary>The HTTP method for the CORS preflight request.</summary>
        public static readonly string PreflightHttpMethod = "OPTIONS";

        /// <summary>The Origin request header.</summary>
        public static readonly string Origin = "Origin";

        /// <summary>
        ///     The value for the Access-Control-Allow-Origin response header to allow all origins.
        /// </summary>
        public static readonly string AnyOrigin = "*";

        /// <summary>The Access-Control-Request-Method request header.</summary>
        public static readonly string AccessControlRequestMethod = "Access-Control-Request-Method";

        /// <summary>The Access-Control-Request-Headers request header.</summary>
        public static readonly string AccessControlRequestHeaders = "Access-Control-Request-Headers";

        /// <summary>The Access-Control-Allow-Origin response header.</summary>
        public static readonly string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        /// <summary>The Access-Control-Allow-Headers response header.</summary>
        public static readonly string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        /// <summary>The Access-Control-Expose-Headers response header.</summary>
        public static readonly string AccessControlExposeHeaders = "Access-Control-Expose-Headers";

        /// <summary>The Access-Control-Allow-Methods response header.</summary>
        public static readonly string AccessControlAllowMethods = "Access-Control-Allow-Methods";

        /// <summary>The Access-Control-Allow-Credentials response header.</summary>
        public static readonly string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        /// <summary>The Access-Control-Max-Age response header.</summary>
        public static readonly string AccessControlMaxAge = "Access-Control-Max-Age";

        internal static readonly string[] SimpleRequestHeaders = new string[4]
        {
            "Origin",
            "Accept",
            "Accept-Language",
            "Content-Language"
        };

        internal static readonly string[] SimpleResponseHeaders = new string[6]
        {
            "Cache-Control",
            "Content-Language",
            "Content-Type",
            "Expires",
            "Last-Modified",
            "Pragma"
        };

        internal static readonly string[] SimpleMethods = new string[3]
        {
            "GET",
            "HEAD",
            "POST"
        };
    }
}