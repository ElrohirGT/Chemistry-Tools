using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using Avalonia.Media.Imaging;

using Chemistry_Tools.UserSettings;

namespace Chemistry_Tools.JSONConverters
{
    public class BitmapConverter : JsonConverter<ImageContainer>
    {
        readonly Type ACCEPTED_TYPE = typeof(string);
        readonly Type SECOND_TYPE = typeof(ImageContainer);
        readonly string BASE_PATH;

        public BitmapConverter() => BASE_PATH = Path.Combine(Environment.CurrentDirectory, "themes");

        public override bool CanConvert(Type typeToConvert) => typeToConvert == ACCEPTED_TYPE || typeToConvert == SECOND_TYPE;

        public override ImageContainer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var path = reader.GetString();
            if (path is null)
                return null;

            path = Path.Combine(BASE_PATH, path);
            if (!File.Exists(path))
                return null;
            
            var bitmap = new Bitmap(path);
            return new ImageContainer(path, bitmap);
        }

        public override void Write(Utf8JsonWriter writer, ImageContainer value, JsonSerializerOptions options) => writer.WriteStringValue(value.Path);
    }
}
