using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TournamentWebApi.WEB.Security
{
    public class CustomApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (CurrentUser != null && CurrentUser.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(Roles) && !CurrentUser.IsInRole(Roles))
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                }

                if (!String.IsNullOrEmpty(Users) && !Users.Contains(CurrentUser.UserId.ToString()))
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}