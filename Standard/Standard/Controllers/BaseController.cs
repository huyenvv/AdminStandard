using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebLib;

namespace Standard.Controllers
{
    public class BaseController : Controller
    {
        protected DB_9CF750_dbEntities db = DB.Entites;
    }
    public class fuck : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!SessionUtilities.Exist("CurrentUser"))
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Base", action = "AccessDenied" }));
        }
    }
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly string[] TheRoles;
        public CustomAuthorize(params string[] roles)
        {
            TheRoles = roles;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
                if (Roles.Length == 0)
                    return;
                var user = DB.CurrentUser;

                bool kq = new WebLib.DAL.fwUserDAL().UserInRole(user.Identity.ToString(), TheRoles);
                if (!kq)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Base", action = "AccessDenied" }));
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Account", action = "Login", ReturnUrl = filterContext.HttpContext.Request.Url }));
            }
        }
    }
}