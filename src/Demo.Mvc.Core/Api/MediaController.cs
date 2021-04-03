using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command.File;
using Demo.Mvc.Core.Sites.Core.Command.File.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class MediaController : ApiControllerBase
    {
        // This will be used in copying input stream to output stream.
        public const int ReadStreamBufferSize = 1024 * 1024;

        private readonly GetFileCommand _getFileCommand;

        // We will discuss this later.
        public readonly IReadOnlyCollection<char> InvalidFileNameChars;

        // We have a read-only dictionary for mapping file extensions and MIME names. 
        public readonly IReadOnlyDictionary<string, string> MimeNames;

        public MediaController(BusinessFactory business, GetFileCommand getFileCommand)
            : base(business)
        {
            _getFileCommand = getFileCommand;
            var mimeNames = new Dictionary<string, string>();

            mimeNames.Add(".mp3", "audio/mpeg"); // List all supported media types; 
            mimeNames.Add(".mp4", "video/mp4");
            mimeNames.Add(".ogg", "application/ogg");
            mimeNames.Add(".ogv", "video/ogg");
            mimeNames.Add(".oga", "audio/ogg");
            mimeNames.Add(".wav", "audio/x-wav");
            mimeNames.Add(".webm", "video/webm");

            MimeNames = new ReadOnlyDictionary<string, string>(mimeNames);

            InvalidFileNameChars = Array.AsReadOnly(Path.GetInvalidFileNameChars());
        }

        /// <summary>
        ///     Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0) output.Write(buffer, 0, len);
        }

        [HttpGet]
        [Route("api/media/get/{siteid}/{id}/{filename}")]
        public async Task<IActionResult> Play(string siteId, string id, string filename)
        {
            // This can prevent some unnecessary accesses. 
            // These kind of file names won't be existing at all. 
            if (string.IsNullOrWhiteSpace(filename) || AnyInvalidFileNameChars(filename))
                return NotFound();

            var getFileInput = new GetFileInput
            {
                SiteId = siteId,
                Id = id,
                Key = string.Empty,
                PropertyName = null,
                Module = "VideoData"
            };
            var result = await
                Business.InvokeAsync<GetFileCommand, GetFileInput, CommandResult<GetFileResult>>(_getFileCommand,
                    getFileInput);

            if (result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(result.Data.RedirectUrl))
                {
                    return Redirect(result.Data.RedirectUrl);
                }
            }
            return null;
        }
            /*   var path = (Path.Combine(Path.GetTempPath(), filename));
               if (!File.Exists(path))
               {
                   using (Stream fileStream = File.Create(path))
                   {
                       CopyStream(result.Data.Stream, fileStream);
                   }
               }

               FileInfo fileInfo = new FileInfo(path);

               if (!fileInfo.Exists)
                   throw new HttpResponseException(HttpStatusCode.NotFound);


               long totalLength = result.Data.FileSize;

               var contentType = result.Data.ContentType;
               var stream = fileInfo.OpenRead();

               RangeHeaderValue rangeHeader = base.Request.Headers.Range;
               HttpResponseMessage response = new HttpResponseMessage();

               response.Headers.AcceptRanges.Add("bytes");

               // The request will be treated as normal request if there is no Range header.
               if (rangeHeader == null || !rangeHeader.Ranges.Any())
               {
                   response.StatusCode = HttpStatusCode.OK;
                   response.Content = new PushStreamContent((outputStream, httpContent, transpContext)
                       =>
                   {
                       using (outputStream) // Copy the file to output stream straightforward. 
                       using (Stream inputStream = stream)
                       {
                           try
                           {
                               inputStream.CopyTo(outputStream, ReadStreamBufferSize);
                           }
                           catch (Exception error)
                           {
                               Debug.WriteLine(error);
                           }
                       }
                   }, contentType);

                   response.Content.Headers.ContentLength = totalLength;
                   return response;
               }

               long start = 0, end = 0;

               // 1. If the unit is not 'bytes'.
               // 2. If there are multiple ranges in header value.
               // 3. If start or end position is greater than file length.
               if (rangeHeader.Unit != "bytes" || rangeHeader.Ranges.Count > 1 ||
                   !TryReadRangeItem(rangeHeader.Ranges.First(), totalLength, out start, out end))
               {
                   response.StatusCode = HttpStatusCode.RequestedRangeNotSatisfiable;
                   response.Content = new StreamContent(Stream.Null); // No content for this status.
                   response.Content.Headers.ContentRange = new ContentRangeHeaderValue(totalLength);
                   response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                   return response;
               }

               var contentRange = new ContentRangeHeaderValue(start, end, totalLength);

               // We are now ready to produce partial content.
               response.StatusCode = HttpStatusCode.PartialContent;
               response.Content = new PushStreamContent((outputStream, httpContent, transpContext)
                   =>
               {
                   using (outputStream) // Copy the file to output stream in indicated range.
                   using (Stream inputStream = fileInfo.OpenRead())
                       CreatePartialContent(inputStream, outputStream, start, end);
               }, new MediaTypeHeaderValue(contentType));

               response.Content.Headers.ContentLength = end - start + 1;
               response.Content.Headers.ContentRange = contentRange;

               return response;
           }
           return null;*/
        // }

        private bool AnyInvalidFileNameChars(string fileName)
        {
            return InvalidFileNameChars.Intersect(fileName).Any();
        }

        private MediaTypeHeaderValue GetMimeNameFromExt(string ext)
        {
            string value;

            if (MimeNames.TryGetValue(ext.ToLowerInvariant(), out value))
                return new MediaTypeHeaderValue(value);
            return new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        }

        private static bool TryReadRangeItem(RangeItemHeaderValue range, long contentLength,
            out long start, out long end)
        {
            if (range.From != null)
            {
                start = range.From.Value;
                if (range.To != null)
                    end = range.To.Value;
                else
                    end = contentLength - 1;
            }
            else
            {
                end = contentLength - 1;
                if (range.To != null)
                    start = contentLength - range.To.Value;
                else
                    start = 0;
            }

            return start < contentLength && end < contentLength;
        }

        private static void CreatePartialContent(Stream inputStream, Stream outputStream,
            long start, long end)
        {
            var count = 0;
            var remainingBytes = end - start + 1;
            var position = start;
            var buffer = new byte[ReadStreamBufferSize];

            inputStream.Position = start;
            do
            {
                try
                {
                    if (remainingBytes > ReadStreamBufferSize)
                        count = inputStream.Read(buffer, 0, ReadStreamBufferSize);
                    else
                        count = inputStream.Read(buffer, 0, (int) remainingBytes);
                    outputStream.Write(buffer, 0, count);
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error);
                    break;
                }

                position = inputStream.Position;
                remainingBytes = end - position + 1;
            } while (position <= end);
        }
    }
}