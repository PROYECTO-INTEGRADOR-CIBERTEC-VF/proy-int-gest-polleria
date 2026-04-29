using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace gest_polleria_front.Filters
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        public string RequiredRole { get; set; }
        public string RequiredRoles { get; set; }
        public string DeniedRole { get; set; }
        public string DeniedRoles { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null || httpContext.Session == null)
            {
                return false;
            }

            var userName = httpContext.Session["AuthUser"] as string;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            var role = httpContext.Session["AuthRole"] as string;

            if (RoleMatches(role, DeniedRole) || RoleMatchesAny(role, DeniedRoles))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(RequiredRole) && string.IsNullOrWhiteSpace(RequiredRoles))
            {
                return true;
            }

            if (RoleMatches(role, RequiredRole))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(RequiredRoles))
            {
                return false;
            }

            return RoleMatchesAny(role, RequiredRoles);
        }

        private static bool RoleMatches(string currentRole, string expectedRole)
        {
            return !string.IsNullOrWhiteSpace(expectedRole) &&
                string.Equals(currentRole, expectedRole, StringComparison.OrdinalIgnoreCase);
        }

        private static bool RoleMatchesAny(string currentRole, string expectedRoles)
        {
            if (string.IsNullOrWhiteSpace(expectedRoles))
            {
                return false;
            }

            return expectedRoles
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Trim())
                .Any(item => string.Equals(currentRole, item, StringComparison.OrdinalIgnoreCase));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var isAuthenticated = session != null && !string.IsNullOrWhiteSpace(session["AuthUser"] as string);

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Home",
                        action = isAuthenticated ? "Index" : "Login"
                    }));
        }
    }
}
