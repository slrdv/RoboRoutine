using System;
using System.Collections.Generic;
using System.Linq;

namespace RoboRoutine
{
    public static class EnumUtils
    {
        public static List<string> GetNames<T>() where T : Enum
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        public static T GetMember<T>(string name) where T : struct, Enum
        {
            if (!Enum.TryParse<T>(name, true, out var result))
            {
                throw new KeyNotFoundException($"Invalid name {name} for {typeof(T).Name}");
            }

            return result;
        }
    }
}