﻿using System.Dynamic;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace MS.ConfigurationService.Api
{
    public static class MongoDynamic
    {
        /// <summary>
        /// deserializes this bson doc to a .net dynamic object
        /// </summary>
        /// <param name="bson">bson doc to convert to dynamic</param>
        public static dynamic ToDynamic(this BsonDocument bson)
        {
            var json = bson.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            dynamic e = JsonSerializer.Deserialize<ExpandoObject>(json);
            BsonValue id;
            if (bson.TryGetValue("_id", out id))
            {
                // Lets set _id again so that its possible to save document.
                e._id = new ObjectId(id.ToString());
            }
            return e;
        }
    }
}
