using System;

namespace FLS.SharePoint.SiteStructure
{
    public interface IConfigPropertiesParser
    {
        uint ToUInt(string propertyValue);

        string[] ToStringArray(string propertyValue);

        Uri ToUri(string propertyValue);
    }
}
