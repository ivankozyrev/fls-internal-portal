using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Utilities;

namespace FLS.SharePoint.ContentDeployment
{
    public class ConfigurationHelper
    {
        public ConfigurationHelper()
        {
            Settings = XDocument.Load(SPUtility.GetGenericSetupPath(Constants.SettingsFilePath));
            ImportSection = Settings.Descendants(Constants.ImportSettingElementName).FirstOrDefault();
            ExportSection = Settings.Descendants(Constants.ExportSettingElementName).FirstOrDefault();
        }

        public XDocument Settings { get; private set; }

        public XElement ImportSection { get; private set; }

        public XElement ExportSection { get; private set; }

        public static void SaveOrCreateElement(XContainer root, string element, string value)
        {
            var el = root.Descendants(element).FirstOrDefault();
            if (el == null)
            {
                el = new XElement(element);
                root.Add(el);
            }

            el.Value = value;
        }

        public static T GetMigrationConfigItem<T>(XContainer root, string element)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            var configItem = root.Element(element);
            if (configItem == null)
            {
                throw new OperationCanceledException(string.Format("Cancelled due element absence. Element: {0}.", element));
            }

            var typeConverter = TypeDescriptor.GetConverter(typeof(T));
            return (T)typeConverter.ConvertFromInvariantString(configItem.Value);
        }

        public void Save()
        {
            Settings.Save(SPUtility.GetGenericSetupPath(Constants.SettingsFilePath));
        }
    }
}
