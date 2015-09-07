using System.Net.Http.Formatting;
using System.Web.Http;

namespace TournamentWebApi.WEB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.Add(new XmlMediaTypeFormatter());

            // this works for /api/players/?formatter=json or /api/players/1?formatter=json
            config.Formatters.XmlFormatter.AddQueryStringMapping("formatter", "xml", "text/xml");
            config.Formatters.JsonFormatter.AddQueryStringMapping("formatter", "json", "application/json");

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "ScoresApiRoute",
            //    routeTemplate: "api/players/{id:int}/score",
            //    defaults: new { controller = "ScoresController", action = "Get" }
            //    );

            //config.Routes.MapHttpRoute(
            //    name: "MatchesApiRoute",
            //    routeTemplate: "api/{controller}/{id}/matches",
            //    defaults: new { controller = "MatchesController", action = "Get" }
            //    );

            config.Routes.MapHttpRoute("DefaultApiRoute", "api/{controller}/{id}", new { id = RouteParameter.Optional }
                );
        }
    }
}