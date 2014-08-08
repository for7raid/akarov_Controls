
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace akarov.Controls.Utils
{
    public class JsonNetDateConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime date;
            if (DateTime.TryParseExact(reader.Value.ToString(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                return date;
            }
            if (DateTime.TryParse(reader.Value.ToString(), out date))
            {
                return date;
            }
            throw new InvalidDataException(string.Format("Не удается преобразовать строку {1} в дату по пути {0}", reader.Path, reader.Value));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer.Path.Contains("ESD"))
            {
                writer.WriteValue(((DateTime)value).ToString("dd.MM.yyyy"));
            }
            else
            {
                writer.WriteValue(((DateTime)value).ToString("dd.MM.yyyy  HH:mm"));
            }

        }
    }

    public class DoubleConverter: CustomCreationConverter<double>
    {

        public override double Create(Type objectType)
        {
            return 0.0;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().Replace(",", ".").Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            var outVal = 0.0;
            if (double.TryParse(value, out outVal))
                return outVal;

            throw new JsonReaderException(string.Format("Не удается преобразовать {0} в число по пути {1}",reader.Value, reader.Path));
        }
    }

    //public class ByteArrayConverter : JsonConverter
    //{

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return true;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.StartObject)
    //        {
    //            OrderItemPreview obj = null;

    //            while (reader.Read())
    //            {
    //                if(reader.TokenType == JsonToken.String)
    //                    obj = new OrderItemPreview() { Image = Convert.FromBase64String(reader.Value.ToString()) };
    //                if (reader.TokenType == JsonToken.EndObject)
    //                    break;
    //            }

    //            return obj;
    //        }
    //        else
    //        {
    //            throw new Exception(
    //                string.Format(
    //                    "Unexpected token parsing binary. "
    //                    + "Expected StartObject, got {0}.",
    //                    reader.TokenType));
    //        }
            
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
           
    //    }
    //}
}