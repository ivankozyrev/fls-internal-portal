using System.Web.Mvc;
using System.Web.Routing;

namespace FLS.Sharepoint.MVC.Infrastructure
{
    /// <summary>
    /// Bootstrapper for SharePoint MVC application.
    /// </summary>
    public sealed class SPMvcBootstrapper
    {
        internal readonly string WebUrl;

        private ISPMvcAreaRegistration _areaRegistration;

        private SPMvcAreaRegistrationContext _areaRegistrationContext;

        static SPMvcBootstrapper()
        {
            // register view engine only once to find views in Layouts folder
            RegisterViewEngine();

            // register controller factory only once per all sites
            RegisterControllerFactory();
        }

        public SPMvcBootstrapper(string webUrl)
        {
            WebUrl = webUrl;
        }

        /// <summary>
        /// Register view engine, register controller factory and routes.
        /// </summary>
        /// <typeparam name="T">Registered area. </typeparam>
        public void Init<T>() where T : ISPMvcAreaRegistration, new()
        {
            _areaRegistration = new T();
            _areaRegistrationContext = new SPMvcAreaRegistrationContext(WebUrl, _areaRegistration.AreaName, RouteTable.Routes);

            SPMvcControllerFactory.Init(_areaRegistration);

            RegisterRoutes();
        }

        /// <summary>
        /// Set controller factory that distinguishes controllers for areas.
        /// </summary>
        private static void RegisterControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(SPMvcControllerFactory));
        }

        /// <summary>
        /// Use just areas for mvc in SharePoint projects. This allows to use MVC in multiple site collections/sites.
        /// </summary>
        private static void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SPMvcViewEngine());
        }

        /// <summary>
        /// Registers routes for area application.
        /// </summary>
        private void RegisterRoutes()
        {
            _areaRegistrationContext.Routes.RouteExistingFiles = true;
            _areaRegistration.RegisterRoutes(_areaRegistrationContext);
        }
    }
}
