using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FunctionApp4
{
        public class Machine
        {
            [BsonId]
            public string Id {get; set;}

            [BsonElement("Name")]
            //[Required]
            public string Name { get; set; }

            public string TimeCreated { get; set; }
        }
}
