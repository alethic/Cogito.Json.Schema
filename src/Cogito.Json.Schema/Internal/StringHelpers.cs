namespace Cogito.Json.Schema.Internal
{

    static class StringHelpers
    {

        public static bool IsBase64String(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length % 4 != 0)
                return false;

            var index = value.Length - 1;
            if (value[index] == '=')
                index--;

            if (value[index] == '=')
                index--;

            for (var i = 0; i <= index; i++)
                if (IsInvalid(value[i]))
                    return false;

            return true;
        }

        static bool IsInvalid(char value)
        {
            if (value >= 48 && value <= 57)
                return false;

            if (value >= 65 && value <= 90)
                return false;

            if (value >= 97 && value <= 122)
                return false;

            return value != 43 && value != 47;
        }

    }

}
