using Newtonsoft.Json;

namespace Demo.Common
{
    public static class CloneHelper
    {
        /// <summary>
        ///     Clone entièrement un objet
        ///     ainsi que ces sous objet référéncé en ajacent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeepCopy(object obj)
        {
            if (obj != null)
            {
                var json = JsonConvert.SerializeObject(obj);
                return JsonConvert.DeserializeObject(json, obj.GetType());
            }

            return null;
        }
    }
}