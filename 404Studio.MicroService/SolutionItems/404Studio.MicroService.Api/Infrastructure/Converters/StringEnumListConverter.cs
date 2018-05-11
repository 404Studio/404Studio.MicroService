using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YH.Etms.Settlement.Api.Infrastructure.Converters
{
    public class StringEnumListConverter : JsonConverter
    {
        public bool CamelCaseText { get; set; }

        public bool AllowIntegerValues { get; set; }

        public StringEnumListConverter()
        {
            AllowIntegerValues = true;
        }

        public StringEnumListConverter(bool camelCaseText)
            : this()
        {
            CamelCaseText = camelCaseText;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                //判断是否为可空类型
               

                return null;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            List<Enum> es = value as List<Enum>;
            if (!AllowIntegerValues)
            {
                throw new JsonSerializationException("Integer value is not allowed.");
            }
            es.ForEach(e =>
            {
                writer.WriteValue(e);
            });
        }
    }
}
