using System;

namespace Cogito.Json.Schema.Internal
{

    static class Constants
    {

        public static class SchemaVersions
        {

            public static readonly Uri Draft3 = new Uri("http://json-schema.org/draft-03/schema#");
            public static readonly Uri Draft4 = new Uri("http://json-schema.org/draft-04/schema#");
            public static readonly Uri Draft6 = new Uri("http://json-schema.org/draft-06/schema#");
            public static readonly Uri Draft7 = new Uri("http://json-schema.org/draft-07/schema#");

        }

        public static class Formats
        {

            public const string Draft3Hostname = "host-name";
            public const string Draft3IPv4 = "ip-address";
            public const string Hostname = "hostname";
            public const string IdnHostname = "idn-hostname";
            public const string DateTime = "date-time";
            public const string Date = "date";
            public const string Time = "time";
            public const string UtcMilliseconds = "utc-millisec";
            public const string Regex = "regex";
            public const string Color = "color";
            public const string Style = "style";
            public const string Phone = "phone";
            public const string Uri = "uri";
            public const string UriReference = "uri-reference";
            public const string UriTemplate = "uri-template";
            public const string Iri = "iri";
            public const string IriReference = "iri-reference";
            public const string JsonPointer = "json-pointer";
            public const string RelativeJsonPointer = "relative-json-pointer";
            public const string IPv6 = "ipv6";
            public const string IPv4 = "ipv4";
            public const string Email = "email";
            public const string IdnEmail = "idn-email";

        }

        public static class ContentEncodings
        {

            public const string Base64 = "base64";

        }

    }

}
