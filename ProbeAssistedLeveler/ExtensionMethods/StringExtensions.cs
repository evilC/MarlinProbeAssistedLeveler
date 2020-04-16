using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProbeAssistedLeveler.ExtensionMethods
{
    public static class StringExtensions
    {
        private static readonly Regex ParameterSplitter = new Regex(@"(\w+:\s*[\w.-]+)");
        private static readonly Regex XyzSplitter = new Regex(@"([XYZ]+\s*[\w.-]+)");
        private static readonly Regex ColonSplitter = new Regex(@"(\w+):\s*([\w.-]+)");

        public static List<string> SplitBySpace(this string text)
        {
            return text.Split(' ').ToList();
        }

        public static float ExtractFloatValue(this string text)
        {
            var value = text.SplitByColon().Value;
            return float.Parse(value);
        }

        public static KeyValuePair<string, string> SplitByColon(this string text)
        {
            var matches = ColonSplitter.Matches(text);
            var match = matches[0].Groups;
            return new KeyValuePair<string, string>(match[1].Value, match[2].Value);
        }

        /// <summary>
        /// Takes a string in a format like
        /// SomeParam:SomeValue SomeParam:SomeValue
        /// And splits it into a Dictionary
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this string text)
        {
            var dict = new Dictionary<string, string>();
            var matches = ParameterSplitter.Matches(text);
            foreach (Match match in matches)
            {
                var keyValuePair = match.Value.SplitByColon();
                dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return dict;
        }

        /// <summary>
        /// Takes a string in the format
        /// X123 Y234 Z456
        /// (eg response from M851) and splits it into a Dictionary
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SplitXyz(this string text)
        {
            var dict = new Dictionary<string, string>();
            var matches = XyzSplitter.Matches(text);
            foreach (Match match in matches)
            {
                dict.Add(match.Value.Substring(0, 1), match.Value.Substring(1));
            }

            return dict;
        }
    }
}
