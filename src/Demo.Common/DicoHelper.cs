using System.Collections.Generic;

namespace Demo.Common
{
    public static class DicoHelper
    {
        public static bool IsDicoEqual(IDictionary<string, string> data1, IDictionary<string, string> data2)
        {
            if (data1 == null && data2 == null) return true;

            if (data1 != null && data2 == null) return false;

            if (data1 == null && data2 != null) return false;

            foreach (var value in data1)
            {
                if (data2.ContainsKey(value.Key))
                {
                    if (data2[value.Key] != value.Value) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static void AddObject(IDictionary<string, object> data, string key, object value)
        {
            if (data == null) return;

            AddObjectIndex(data, key, value, 0);
        }

        private static void AddObjectIndex(IDictionary<string, object> data, string key, object value, int index)
        {
            if (data == null) return;

            string keyTemp;
            if (index > 0)
            {
                keyTemp = key + index;
            }
            else
            {
                keyTemp = key;
            }

            if (!data.ContainsKey(keyTemp))
            {
                data[keyTemp] = value;
            }
            else
            {
                index++;
                AddObjectIndex(data, key, value, index);
            }
        }
    }
}