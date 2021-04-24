using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace profile_service.Models
{
	public class User
	{
		[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
		public string UId { get; set; }
		
		[BsonElement("Uname")]
		public string Uname { get; set; }

		[BsonElement("Email")]
		public string Email { get; set; }

		[BsonElement("Password")]
		public string Password { get; set; }

		[BsonElement("Friends")]
		public List<string> Friends { get; set; }
	}
}