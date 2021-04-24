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
		
		[BsonElement("FirstName")]
		public string FirstName { get; set; }
		
		[BsonElement("LastName")]
		public string LastName { get; set; }

		[BsonElement("Email")]
		public string Email { get; set; }

		[BsonElement("Password")]
		public string Password { get; set; }

		[BsonElement("Friends")]
		public List<string> Friends { get; set; }
	}

	public enum UserEvents
	{
		CREATED,
		EXISTS,
		ERROR,
		ADDED
	}
}