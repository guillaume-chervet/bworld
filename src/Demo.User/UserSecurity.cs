using System;
using System.Threading.Tasks;
using Demo.User.Identity;
using Demo.User.Site;
using Demo.Common;

namespace Demo.User
{
    public static class UserSecurity
    {
        private const string SuperAdministrator = "super_administrator";

        public static bool IsSiteId(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return false;
            }
            return role != SuperAdministrator && !role.Contains("_");
        }

        public static string MapRole(string siteId, SiteUserRole userRole)
        {
            if (SiteUserRole.Administrator == userRole)
            {
                return siteId;
            }
            else 
            {
                return siteId + "_private_user";
            }
        }

        public static async Task CheckHasOneRolesAsync(UserService userService, string userId, string siteId, params SiteUserRole[] siteUserRoles)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotAuthentifiedException();
            }

            var hasRole = await HasRolesAsync(userService, userId, siteId, false, siteUserRoles);
            if (!hasRole)
            {
                throw new NotAuthorizedException();
            }
        }

        public static async Task<bool> HasRolesAsync(UserService userService, string userId, string siteId, bool checkAllRoles, params SiteUserRole[] siteUserRoles)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            if (siteUserRoles.Length <= 0)
            {
                return true;
            }
            var user = await userService.FindApplicationUserByIdAsync(userId);
            var nbRole = 0;
            foreach (var userRole in siteUserRoles)
            {
                if (SiteUserRole.Administrator == userRole)
                {
                    var isAdmin = HasRole(user, MapRole(siteId, SiteUserRole.Administrator), siteId);
                    if (isAdmin)
                    {
                        nbRole++;
                    }
                } else if (SiteUserRole.PrivateUser == userRole)
                {
                    var hasRole = HasRole(user, MapRole(siteId, SiteUserRole.PrivateUser), siteId);
                    if (hasRole)
                    {
                        nbRole++;
                    }
                }
            }

            if (checkAllRoles)
            {
                return siteUserRoles.Length == nbRole;
            }
            else
            {
                return nbRole > 0;
            }
        }

        public static bool IsAdministrator(User user, string siteId)
        {
            if (string.IsNullOrEmpty(siteId))
            {
                return false;
            }

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    if (role == SuperAdministrator)
                    {
                        return true;
                    }

                    if (role == siteId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private static bool HasRole(ApplicationUser user, string role, string siteId = null)
        {
            if (user?.Roles != null)
            {
                foreach (var userRole in user.Roles)
                {
                    if (userRole == SuperAdministrator)
                    {
                        return true;
                    }

                    // Administrator
                    if (!string.IsNullOrEmpty(siteId))
                    {
                        if (userRole.ToString() == siteId)
                        {
                            return true;
                        }
                    }

                    if (role == userRole.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static async Task CheckAdministratorAsync(UserService userService, string userId, string siteId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotAuthentifiedException();
            }

            var user = await userService.FindApplicationUserByIdAsync(userId);
            var hasRole = HasRole(user, siteId);
            if (hasRole==false)
            {
                throw new NotAuthorizedException();
            }
        }

        private static bool IsSuperAdministratorFrom(ApplicationUser user)
        {
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    if (role == SuperAdministrator)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static async Task CheckIsSuperAdministratorAsync(UserService userService, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotAuthentifiedException();
            }

            var user = await userService.FindApplicationUserByIdAsync(userId);
            if (!IsSuperAdministratorFrom(user))
            {
                throw new NotAuthorizedException();
            }
        }
    }
}