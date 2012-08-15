using FLS.Sharepoint.MVC.Infrastructure;

namespace FLS.Sharepoint.MVC.Web
{
    public class AppMvcRegistration : ISPMvcAreaRegistration
    {
        public string AreaName
        {
            get { return "ListViewArea"; }
        }

        public void RegisterRoutes(SPMvcAreaRegistrationContext context)
        {
            context.MapRoute("Home", "mvcengine/home", new { controller = "Home", action = "Index" });
        }
    }
}
