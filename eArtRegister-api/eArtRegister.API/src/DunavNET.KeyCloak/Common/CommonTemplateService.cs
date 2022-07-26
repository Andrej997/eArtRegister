using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace KeyCloak.Common
{
    public class CommonTemplateService<T>
    {
        public static IEnumerable<T> ConvertToEntities(string entitiesStr)
        {
            JArray jsonArray = JArray.Parse(entitiesStr);

            HashSet<T> entities = new HashSet<T>();
            foreach (var item in jsonArray)
            {
                entities.Add(JsonConvert.DeserializeObject<T>(item.ToString()));
            }

            return entities;
        }

        public static T ConvertToEntity(string entityStr) => JsonConvert.DeserializeObject<T>(entityStr);
    }
}
