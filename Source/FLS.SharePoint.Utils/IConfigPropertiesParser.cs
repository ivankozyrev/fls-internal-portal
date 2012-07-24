using System;

namespace FLS.SharePoint.Utils
{
    public interface IConfigPropertiesParser
    {
        uint ToUInt(string propertyValue);

        string[] ToStringArray(string propertyValue);

        Uri ToUri(string propertyValue);
    }
}
