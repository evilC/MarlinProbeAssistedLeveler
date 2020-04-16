using System;
using System.Collections.Generic;
using System.Numerics;

namespace ProbeAssistedLeveler.ExtensionMethods
{
    public static class ListExtensions
    {
        public static Vector3 ExtractVector3(this List<string> chunks)
        {
            float x = 0;
            var foundX = false;
            float y = 0;
            var foundY = false;
            float z = 0;
            var foundZ = false;
            foreach (var chunk in chunks)
            {
                if (chunk.StartsWith("X:"))
                {
                    x = chunk.ExtractFloatValue();
                    foundX = true;
                }
                else if (chunk.StartsWith("Y:"))
                {
                    y = chunk.ExtractFloatValue();
                    foundY = true;
                }
                else if (chunk.StartsWith("Z:"))
                {
                    z = chunk.ExtractFloatValue();
                    foundZ = true;
                }
            }

            if (!foundX || !foundY || !foundZ)
            {
                throw new Exception("Could not find X, Y and Z");
            }

            return new Vector3(x, y, z);
        }

        public static Dictionary<string, string> ToDictionary(this List<string> multiLineResponse)
        {
            var dict = new Dictionary<string, string>();
            foreach (var response in multiLineResponse)
            {
                var keyValuePair = response.SplitByColon();
                dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return dict;
        }


    }
}
