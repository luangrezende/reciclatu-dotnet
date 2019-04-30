using APLICACAO.Controllers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace APLICACAO
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //TRATATIAS DE ERROS
        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (MvcApplication)sender;
            var context = app.Context;
            var ex = app.Server.GetLastError();
            context.Response.Clear();
            context.ClearError();

            HttpException httpException = ExceptionMethod(ex);

            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["exception"] = ex;
            routeData.Values["action"] = "DefaultError";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values["action"] = "NotFound404";
                        break;
                    case 500:
                        routeData.Values["action"] = "ServerError500";
                        break;
                }
            }

            IController controller = new ErrorsController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }

        private static HttpException ExceptionMethod(Exception ex)
        {
            return ex as HttpException;
        }
    }
}
