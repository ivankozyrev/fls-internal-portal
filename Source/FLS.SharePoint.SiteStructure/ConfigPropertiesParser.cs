using System;

namespace FLS.SharePoint.SiteStructure
{
    public class ConfigPropertiesParser : IConfigPropertiesParser
    {
        private const char Separator = ';';
        
        public uint ToUInt(string propertyValue)
        {
            return uint.Parse(propertyValue);
        }

        public string[] ToStringArray(string propertyValue)
        {
            return propertyValue.Split(Separator);
        }

        public Uri ToUri(string propertyValue)
        {
            return new Uri(propertyValue);
        }
    }
}
