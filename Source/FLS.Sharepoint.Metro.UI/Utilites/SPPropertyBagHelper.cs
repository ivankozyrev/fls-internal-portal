using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.Sharepoint.Metro.UI.Utilites
{
    public static class SPPropertyBagHelper
    {
        public static void AddProperty(this SPWeb web, string key, string value)
        {
            web.Properties[key] = value;
            web.AllProperties[key] = value;
        }

        public static void RemoveProperty(this SPWeb web, string key)
        {
            web.AllProperties.Remove(key);
            web.Properties[key] = null;
        }

        public static void AddPropertyAndUpdate(this SPWeb web, string key, string value)
        {
            AddPropertiesAndUpdate(web, new Dictionary<string, string>(1) { { key, value } });
        }

        public static void AddPropertiesAndUpdate(this SPWeb web, IDictionary<string, string> properties)
        {
            web.DoUnsafeUpdateAsSystemUser(systemWeb =>
                         {
                             foreach (var item in properties)
                             {
                                 AddProperty(systemWeb, item.Key, item.Value);
                             }

                             systemWeb.Update();
                             systemWeb.Properties.Update();
                         });
        }

        public static void RemovePropertyAndUpdate(this SPWeb web, string key)
        {
            RemovePropertiesAndUpdate(web, new[] { key });
        }

        public static void RemovePropertiesAndUpdate(this SPWeb web, params string[] keys)
        {
            web.DoUnsafeUpdateAsSystemUser(systemWeb =>
                         {
                             foreach (var item in keys)
                             {
                                 RemoveProperty(systemWeb, item);
                             }

                             systemWeb.Update();
                             systemWeb.Properties.Update();
                         });
        }

        public static string GetPropertyValue(this SPWeb web, string key)
        {
            return web.AllProperties.ContainsKey(key) ? web.AllProperties[key].ToString() : null;
        }

        public static string GetFarmProperty(string key)
        {
            return GetFarmProperty<string>(key);
        }

        public static T GetFarmProperty<T>(string key)
        {
            var value = default(T);
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                var local = SPFarm.Local.Properties;
                if (!local.ContainsKey(key))
                {
                    return;
                }

                value = (T)local[key];
            });

            return value;
        }
    }
}
