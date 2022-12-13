namespace WorkerService; 
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
 
  public class Productdto
  { 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
      public string? Id { get; set; } 

      [BsonElement("price")]
      public double? Price { get; set; } 

    //[BsonElement("customer")]
    //public Customer? customer {get; set; }     
 
  } 