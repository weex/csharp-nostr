﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nostr.Client.Messages;
using Nostr.Client.Responses;
using Nostr.Client.Responses.Contacts;
using Nostr.Client.Responses.Metadata;

namespace Nostr.Client.Json
{
    internal class NostrEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(NostrEvent);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var jObject = JObject.Load(reader);

            var target = RecognizeEvent(jObject);
            return NostrSerializer.Serializer.Deserialize(jObject.CreateReader(), target);
        }

        private static Type RecognizeEvent(JObject jObject)
        {
            try
            {
                var kind = jObject["kind"]?.ToObject<NostrKind>();
                return kind switch
                {
                    NostrKind.Metadata => typeof(NostrMetadataEvent),
                    NostrKind.Contacts => typeof(NostrContactEvent),
                    _ => typeof(NostrEvent)
                };
            }
            catch (Exception)
            {
                // default
                return typeof(NostrEvent);
            }
        }
    }
}