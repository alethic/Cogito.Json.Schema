using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web;

namespace Cogito.Json.Schema.Validation.Builders
{

    /// <summary>
    /// Provides helper methods that conduct format validation.
    /// </summary>
    static class FormatValidation
    {

        static readonly Regex UriTemplateRegex = new Regex(@"^(?:(?:[^\x00-\x20""'<>%\\^`{|}]|%[0-9a-f]{2})|\{[+#.\/;?&=,!@|]?(?:[a-z0-9_]|%[0-9a-f]{2})+(?:\:[1-9][0-9]{0,3}|\*)?(?:,(?:[a-z0-9_]|%[0-9a-f]{2})+(?:\:[1-9][0-9]{0,3}|\*)?)*\})*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        static readonly Regex JsonPointerRegex = new Regex(@"^(?:\/(?:[^~/]|~0|~1)*)*$|^#(?:\/(?:[a-z0-9_\-.!$&'()*+,;:=@]|%[0-9a-f]{2}|~0|~1)+)+$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        static readonly Regex RelativeJsonPointerRegex = new Regex(@"^\d(#|((?:\/(?:[^~/]|~0|~1)*)*$|^#(?:\/(?:[a-z0-9_\-.!$&'()*+,;:=@]|%[0-9a-f]{2}|~0|~1)+)+))$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static bool ValidateUriTemplate(string text)
        {
            return UriTemplateRegex.IsMatch(text);
        }

        public static bool ValidateUri(string text)
        {
            return ValidateIri(text);
        }

        public static bool ValidateIri(string text)
        {
            return Uri.IsWellFormedUriString(text, UriKind.Absolute) || Uri.TryCreate(text, UriKind.Absolute, out var _);
        }

        public static bool ValidateJsonPointer(string text)
        {
            return JsonPointerRegex.IsMatch(text);
        }

        public static bool ValidateRelativeJsonPointer(string text)
        {
            return RelativeJsonPointerRegex.IsMatch(text);
        }

        public static bool ValidateUriReference(string text)
        {
            if (ValidateUri(text))
                return true;

            if (text.StartsWith("//"))
                return Uri.IsWellFormedUriString("scheme:" + text, UriKind.Absolute);

            if (text.StartsWith("#"))
                return Uri.IsWellFormedUriString("scheme://foo.bar" + text, UriKind.Absolute);
            else
                return Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute);
        }

        public static bool ValidateIriReference(string text)
        {
            if (ValidateIri(text))
                return true;

            if (text.StartsWith("//"))
                return Uri.IsWellFormedUriString("scheme:" + text, UriKind.Absolute);

            if (text.StartsWith("#"))
                return Uri.IsWellFormedUriString("scheme://ƒøø.ßår" + text, UriKind.Absolute);
            else
                return Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute) || Uri.TryCreate(text, UriKind.Relative, out var _);
        }

        public static bool ValidateIPv4(string value)
        {
            var parts = value.Split('.');
            if (parts.Length != 4)
                return false;

            for (var i = 0; i < parts.Length; i++)
                if (!int.TryParse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out var num) || num < 0 || num > 255 || (parts[i] != "0" && parts[i].StartsWith("0")))
                    return false;

            return true;
        }

        public static bool ValidateIPv6(string value)
        {
            return Uri.CheckHostName(value) == UriHostNameType.IPv6 && IPAddress.TryParse(value, out var ip) && ip.AddressFamily == AddressFamily.InterNetworkV6 && value.Contains("%") == false;
        }
    }

}
