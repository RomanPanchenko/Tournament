using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Ninject;
using TournamentWebApi.WEB.Interfaces;

namespace TournamentWebApi.WEB.Filters
{
    public class ApiExceptionLoggingFilter : ExceptionFilterAttribute
    {
        [Inject]
        public ITournamentWebApiLogger Logger
        {
            get { return NinjectWebCommon.Kernel.Get<ITournamentWebApiLogger>(); }
        }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            Exception exception = filterContext.Exception;

            int httpCode = new HttpException(null, exception).GetHttpCode();
            filterContext.Response = new HttpResponseMessage((HttpStatusCode)httpCode);

            Logger.Error(filterContext);
        }
    }
}