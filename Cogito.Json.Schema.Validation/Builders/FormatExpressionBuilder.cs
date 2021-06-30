using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

using Cogito.Json.Schema.Validation.Internal;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Cogito.Json.Schema.Validation.Builders
{

    /// <summary>
    /// Implements validation of the 'format' JSON schema test.
    /// </summary>
    public class FormatExpressionBuilder : ExpressionBuilderBase
    {

        public override Expression Build(JSchemaExpressionBuilder builder, JSchema schema, Expression token)
        {
            if (schema.Format == null)
                return null;

            return IfThenElseTrue(
                IsTokenType(token, JTokenType.String),
                ValidateFormat(schema.Format, Expression.Convert(token, typeof(string))));
        }

        Expression ValidateFormat(string format, Expression o)
        {
            switch (format)
            {
                case Constants.Formats.Color:
                    return CallThis(nameof(ValidateColor), o);
                case Constants.Formats.Hostname:
                case Constants.Formats.Draft3Hostname:
                    return CallThis(nameof(ValidateHostname), o);
                case Constants.Formats.IdnHostname:
                    return CallThis(nameof(ValidateIdnHostname), o);
                case Constants.Formats.IPv4:
                case Constants.Formats.Draft3IPv4:
                    return CallThis(nameof(ValidateIPv4), o);
                case Constants.Formats.IPv6:
                    return CallThis(nameof(ValidateIPv6), o);
                case Constants.Formats.Email:
                    return CallThis(nameof(ValidateEmail), o);
                case Constants.Formats.IdnEmail:
                    return CallThis(nameof(ValidateIdnEmail), o);
                case Constants.Formats.Uri:
                    return CallThis(nameof(ValidateUri), o);
                case Constants.Formats.UriReference:
                    return CallThis(nameof(ValidateUriReference), o);
                case Constants.Formats.UriTemplate:
                    return CallThis(nameof(ValidateUriTemplate), o);
                case Constants.Formats.Iri:
                    return CallThis(nameof(ValidateIri), o);
                case Constants.Formats.IriReference:
                    return CallThis(nameof(ValidateIriReference), o);
                case Constants.Formats.JsonPointer:
                    return CallThis(nameof(ValidateJsonPointer), o);
                case Constants.Formats.RelativeJsonPointer:
                    return CallThis(nameof(ValidateRelativeJsonPointer), o);
                case Constants.Formats.Date:
                    return CallThis(nameof(ValidateDate), o);
                case Constants.Formats.Time:
                    return CallThis(nameof(ValidateTime), o);
                case Constants.Formats.DateTime:
                    return CallThis(nameof(ValidateDateTime), o);
                case Constants.Formats.UtcMilliseconds:
                    return CallThis(nameof(ValidateUtcMilliseconds), o);
                case Constants.Formats.Regex:
                    return CallThis(nameof(ValidateRegex), o);
                default:
                    return True;
            }
        }

        static bool ValidateEmail(string value) =>
            EmailHelpers.Validate(value, false);

        static bool ValidateIdnEmail(string value) =>
            EmailHelpers.Validate(value, true);

        static bool ValidateUri(string value) =>
            Uri.IsWellFormedUriString(value, UriKind.Absolute);

        static bool ValidateUriReference(string value) =>
            FormatHelpers.ValidateUriReference(value);

        static bool ValidateIri(string value) =>
            Uri.IsWellFormedUriString(value, UriKind.Absolute);

        static bool ValidateIriReference(string value) =>
            FormatHelpers.ValidateIriReference(value);

        static bool ValidateUriTemplate(string value) =>
            FormatHelpers.ValidateUriTemplate(value);

        static bool ValidateJsonPointer(string value) =>
            FormatHelpers.ValidateJsonPointer(value);

        static bool ValidateRelativeJsonPointer(string value) =>
            FormatHelpers.ValidateRelativeJsonPointer(value);

        static bool ValidateDate(string value) => DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _);

        static bool ValidateTime(string value) => ValidateTimeRegex(value);

        static readonly Regex TimeRegex = new Regex(@"^(?<h>\d{2})\:(?<m>\d{2})\:(?<s>\d{2})(.(?<ms>\d+))?([Zz]|(?<o>[-+])(?<zh>\d{2}):(?<zm>\d{2}))?$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        static bool ValidateTimeRegex(string value)
        {
            var r = TimeRegex.Match(value);
            if (r.Success)
            {
                if (int.TryParse(r.Groups["h"].Value, out var h) && int.TryParse(r.Groups["m"].Value, out var m) && int.TryParse(r.Groups["s"].Value, out var s))
                {
                    // time values must be in range (leap seconds!)
                    if (h < 0 || h > 23 || m < 0 || m > 59 || s < 0 || s > 60)
                        return false;

                    // millisecond value must be at least 0
                    if (r.Groups["ms"] is Group msg && msg.Success && int.TryParse(msg.Value, out var ms))
                        if (ms < 0)
                            return false;

                    // offset indicator exists but isn't proper value
                    if (r.Groups["o"] is Group og && og.Success && og.Value != "-" && og.Value != "+")
                        return false;

                    // check that offset values are accurate
                    if (r.Groups["zh"] is Group zhg && zhg.Success && r.Groups["zm"] is Group zmg && zmg.Success && int.TryParse(zhg.Value, out var zh) && int.TryParse(zmg.Value, out var zm))
                        if (zh < 0 || zh > 23 || zm < 0 || zm > 59)
                            return false;

                    // all the checks succeeded
                    return true;
                }
            }

            return false;
        }

        static bool ValidateDateTime(string value) =>
            DateTime.TryParseExact(value, @"yyyy-MM-dd\THH:mm:ss.FFFFFFFK", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _) ||
            DateTime.TryParseExact(value.ToUpper(), @"yyyy-MM-dd\THH:mm:ss.FFFFFFFK", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _);

        static bool ValidateUtcMilliseconds(string value) =>
            double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var _);

        static bool ValidateRegex(string value)
        {
            try
            {
                new Regex(value, RegexOptions.ECMAScript);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static readonly Regex HostnameRegex = new Regex(@"^(?=.{1,255}$)[0-9A-Za-z](?:(?:[0-9A-Za-z]|-){0,61}[0-9A-Za-z])?(?:\.[0-9A-Za-z](?:(?:[0-9A-Za-z]|-){0,61}[0-9A-Za-z])?)*\.?$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        static bool ValidateHostname(string value)
        {
            return HostnameRegex.IsMatch(value);
        }

        static readonly Regex IdnHostnameRegex = new Regex(@"^(?:[\p{L}\p{N}][\p{L}\p{N}-_]*.)+[\p{L}\p{N}]{2,}$", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        static readonly char[] DisallowedIdnChars = new[] { '\u0020', '\u002D', '\u00A2', '\u00A3', '\u00A4', '\u00A5', '\u034F', '\u0640', '\u07FA', '\u180B', '\u180C', '\u180D', '\u200B', '\u2060', '\u2104', '\u2108', '\u2114', '\u2117', '\u2118', '\u211E', '\u211F', '\u2123', '\u2125', '\u2282', '\u2283', '\u2284', '\u2285', '\u2286', '\u2287', '\u2288', '\u2616', '\u2617', '\u2619', '\u262F', '\u2638', '\u266C', '\u266D', '\u266F', '\u2752', '\u2756', '\u2758', '\u275E', '\u2761', '\u2775', '\u2794', '\u2798', '\u27AF', '\u27B1', '\u27BE', '\u3004', '\u3012', '\u3013', '\u3020', '\u302E', '\u302F', '\u3031', '\u3032', '\u3035', '\u303B', '\u3164', '\uFFA0' };
        static readonly IdnMapping IdnMapping = new IdnMapping();

        static bool ValidateIdnHostname(string value)
        {
            return IdnHostnameRegex.IsMatch(value) && value.IndexOfAny(DisallowedIdnChars) == -1 && TryGetIdnAsciiString(value, out var idn) && idn.Split('.').All(i => i.Length <= 63);
        }

        static bool TryGetIdnAsciiString(string unicode, out string idn)
        {
            try
            {
                idn = IdnMapping.GetAscii(unicode);
                return true;
            }
            catch (ArgumentException)
            {

            }

            idn = null;
            return false;
        }

        static bool ValidateColor(string value) => ColorHelpers.IsValid(value);

        static bool ValidateIPv6(string value) => Uri.CheckHostName(value) == UriHostNameType.IPv6 && IPAddress.TryParse(value, out var ip) && ip.AddressFamily == AddressFamily.InterNetworkV6;

        static bool ValidateIPv4(string value)
        {
            var parts = value.Split('.');
            if (parts.Length != 4)
                return false;

            for (var i = 0; i < parts.Length; i++)
                if (!int.TryParse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out var num) || num < 0 || num > 255 || (parts[i] != "0" && parts[i].StartsWith("0")))
                    return false;

            return true;
        }

    }

}
