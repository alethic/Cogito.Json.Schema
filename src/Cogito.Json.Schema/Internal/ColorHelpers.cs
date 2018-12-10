#region License
// Copyright (c) Newtonsoft. All Rights Reserved.
// License: https://raw.github.com/JamesNK/Newtonsoft.Json.Schema/master/LICENSE.md
#endregion

using System.Collections.Generic;

namespace Cogito.Json.Schema.Internal
{

    static class ColorHelpers
    {

        static readonly List<string> NamedColors = new List<string>
        {
            "aqua",
            "black",
            "blue",
            "fuchsia",
            "gray",
            "green",
            "lime",
            "maroon",
            "navy",
            "olive",
            "orange",
            "purple",
            "red",
            "silver",
            "teal",
            "white",
            "yellow"
        };

        public static bool IsValid(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            if (NamedColors.Contains(s))
                return true;

            if (s[0] == '#')
            {
                // #CC8899
                // #C89
                if (s.Length == 7 || s.Length == 4)
                {
                    for (var i = 1; i < s.Length; i++)
                    {
                        var c = s[i];
                        var isHex = c >= '0' && c <= '9' ||
                                    c >= 'a' && c <= 'f' ||
                                    c >= 'A' && c <= 'F';

                        if (!isHex)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

    }

}
