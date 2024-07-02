﻿using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonWriter = Newtonsoft.Json.JsonWriter;
using JsonReader = Newtonsoft.Json.JsonReader;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Silmoon.Data.LiteDB.Converters
{
    /// <summary>
    /// 用于LiteDB的ObjectId转换器，原本不用此转换器，可以直接将LiteDB的ObjectId类型转换成字符串，但是在反序列化时，字符串没有反序列化称LiteDB的ObjectId类型的实现，所以需要此转换器。
    /// </summary>
    /// <remarks>
    /// <code>
    /// 可以直接在属性上添加[JsonConverter(typeof(ObjectIdConverter))]，也可以在Newtonsoft.Json.JsonConvert.DefaultSettings中添加此转换器：
    ///            Newtonsoft.Json.JsonConvert.DefaultSettings = new Func<Newtonsoft.Json.JsonSerializerSettings>(() =>
    ///            {
    ///                var settings = new Newtonsoft.Json.JsonSerializerSettings();
    ///                settings.Converters.Add(new ObjectIdConverter());
    ///                return settings;
    ///            });
    /// </code>
    /// </remarks>
    public class LiteDBObjectIdJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is null) return null;
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing ObjectId. Expected String, got {reader.TokenType}");
            }
            var value = (string)reader.Value;
            return string.IsNullOrEmpty(value) ? ObjectId.Empty : new ObjectId(value);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
        }
    }
}
