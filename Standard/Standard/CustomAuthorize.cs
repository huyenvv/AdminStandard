using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebLib;


namespace Standard
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private string[] TheRoles;
        public CustomAuthorize(params string[] roles)
        {
            TheRoles = roles;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var k = new WebLib.DAL.fwUserDAL().Authorize(TheRoles);
            if (k == 0) return;
            if (k == 1)
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl }));
            else
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
        }
    }
}