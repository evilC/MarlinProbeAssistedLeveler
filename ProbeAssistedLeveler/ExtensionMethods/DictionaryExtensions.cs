using System;
using System.Collections.Generic;
using System.Numerics;

namespace ProbeAssistedLeveler.ExtensionMethods
{
    public static class DictionaryExtensions
    {
        public static Vector3 ToVector3(this Dictionary<string, string> chunks)
        {
            if (!chunks.TryGetValue("X", out var x))
            {
                throw new Exception("Could not find value for X in dictionary");
            }
            if (!chunks.TryGetValue("Y", out var y))
            {
                throw new Exception("Could not find value for Y in dictionary");
            }
            if (!chunks.TryGetValue("Z", out var z))
            {
                throw new Exception("Could not find value for Z in dictionary");
            }
            return new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        }

    }
}
