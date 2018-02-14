using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommonShares.Utilities;
using Newtonsoft.Json;

namespace CommonShares.Serialization
{
    public static class MyJsonConvert
    {
        static JsonSerializer _serializer;

        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings;

        static MyJsonConvert()
        {
            DefaultJsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new SpecialContractResolver()
            };
            _serializer = JsonSerializer.Create(DefaultJsonSerializerSettings);
        }



        public static string SerializeObject<T>(T result)
        {
            try
            {
                return InnerSerializeObjectToJson(result);
            }
            catch (OutOfMemoryException)
            {
                MemoryHelper.CleanMemory();

                Task.Delay(TimeSpan.FromMilliseconds(100)).GetAwaiter().GetResult();
                return InnerSerializeObjectToJson(result);
            }
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    return InnerSerializeObjectToJson(result);
            //}

        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, DefaultJsonSerializerSettings);
        }

        private static string InnerSerializeObjectToJson<T>(T result)
        {
            StringBuilder sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            using (JsonTextWriter textWriter = new JsonTextWriter(writer))
            {
                _serializer.Serialize(textWriter, result);
            }

            return sb.ToString();
        }
    }
}
