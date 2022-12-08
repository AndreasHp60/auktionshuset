using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace productReceive;
 
  public class Customer 
  { 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
      public string? Id { get; set; } 
      [BsonElement("firstname")]
      public string? FirstName { get; set; } 
      [BsonElement("lastname")]
      public string? LastName { get; set; } 
      [BsonElement("password")]
      public string? Password { get; set; } 
      [BsonElement("email")]
      public string? Email { get; set; } 
      [BsonElement("phonenumber")]
      public string? Phonenr { get; set; } 
      [BsonElement("address")]
      public string? Address { get; set; } 
      [BsonElement("postal")]
      public short? Postal { get; set; } 
      [BsonElement("city")]
      public string? City { get; set; } 
      [BsonElement("country")]
      public string? Country { get; set; }       
 
  } 