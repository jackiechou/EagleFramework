using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eagle.Services.Dtos.Common
{
    public class JsonResultConverter : JsonCreationConverter<Result>
    {
        protected override Result Create(Type objectType, JObject jObject)
        {
            if (FieldExists("Errors", jObject))
            {
                return new FailResult();
            }
            if (FieldExists("Data", jObject))
            {
                return new SuccessResult();
            }
            return null;
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            Result target = Create(objectType, jObject);

            // Populate the object properties
            if (target != null)
                serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}
