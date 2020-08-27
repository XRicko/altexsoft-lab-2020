using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RecipeBook.BL.Controllers
{
    public class JSONDataManager : IDataManager
    {
        private readonly JsonSerializer serializer;

        public JSONDataManager(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Save<T>(List<T> items)
        {
            var fileName = Path.ChangeExtension(typeof(T).Name, "json");

            using (var file = new FileStream(fileName, FileMode.Create))
            using (var sw = new StreamWriter(file))
            using (var writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, items);
            }
        }
        public List<T> Load<T>()
        {
            var fileName = Path.ChangeExtension(typeof(T).Name, "json");

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            using (var sr = new StreamReader(file))
            using (var reader = new JsonTextReader(sr))
            {
                var items = serializer.Deserialize<List<T>>(reader);

                if (items is List<T>)
                    return items;
                else
                    return new List<T>();
            }
        }
    }
}
