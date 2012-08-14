using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace FLS.Sharepoint.MVC.Infrastructure
{
    public class SPMvcControllerFactory : DefaultControllerFactory
    {
        private static readonly Dictionary<string, Type> TypesCache = new Dictionary<string, Type>();

        private static readonly object Locker = new object();

        public static void Init(ISPMvcAreaRegistration areaRegistration)
        {
            if (areaRegistration == null)
            {
                throw new ArgumentNullException("areaRegistration");
            }

            var assembly = areaRegistration.GetType().Assembly;
            var types = assembly.GetTypes();

            AddControllersToCache(areaRegistration.AreaName, types);
        }

        /// <summary>
        /// Retrieves the controller type for the specified name and request context.
        /// </summary>
        /// <returns>
        /// The controller type.
        /// </returns>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param><param name="controllerName">The name of the controller.</param>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            Type type;

            var area = requestContext.RouteData.DataTokens["area"] as string;

            if (string.IsNullOrEmpty(area))
            {
                return null;
            }

            var typeCacheKey = string.Format(CultureInfo.InvariantCulture, "{0}_{1}Controller", area, controllerName);

            TypesCache.TryGetValue(typeCacheKey, out type);

            return type;
        }

        private static void AddControllersToCache(string areaName, IEnumerable<Type> types)
        {
            var controllerTypes = types.Where(IsControllerType);

            foreach (var controllerType in controllerTypes)
            {
                var typeCacheKey = string.Format("{0}_{1}", areaName, controllerType.Name);

                lock (Locker)
                {
                    if (!TypesCache.ContainsKey(typeCacheKey))
                    {
                        TypesCache.Add(typeCacheKey, controllerType);
                    }
                }
            }
        }

        private static bool IsControllerType(Type type)
        {
            return (((type != null) && type.IsPublic) &&
                     (type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && !type.IsAbstract)) &&
                    typeof(IController).IsAssignableFrom(type);
        }
    }
}
