using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoStop.VIEW.Filters
{
    public class AutorizaAdmin : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Role"] == null)
            {
                filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","Home"},
                    {"Action","Erro"}
                });
            }
            else if (Convert.ToString(HttpContext.Current.Session["Role"]) == "user")
            {
                filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller","Home"},
                    {"Action","Erro"}
                });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}