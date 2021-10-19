using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Tribunet.Atv.Services
{
    public class ModelDataProvider
    {


    }

    public static class ModelDataProviderExtensions
    {
        public static string ToRfc3339String(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

        public static string GetXmlEnumName<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum) 
                return null;//or string.Empty, or throw exception

            var member = enumType.GetMember(value.ToString()).FirstOrDefault();
            if (member == null) 
                return null;//or string.Empty, or throw exception

            var attribute = member.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault();
            return attribute?.Name;
        }
    }
}
