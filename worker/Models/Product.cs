namespace WorkerService; 
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
 
  public class Product 
  { 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
      public string? Id { get; set; } 
      [BsonElement("name")]
      public string? Name { get; set; } 
      [BsonElement("description")]
      public string? Description { get; set; } 
      [BsonElement("category")]
      public string? Category { get; set; } 
      [BsonElement("assesment")]
      public double? Assesment { get; set; } 
      [BsonElement("price")]
      public double? Price { get; set; } 
      [BsonElement("minbid")]
      public double? MinBid { get; set; } 
      [BsonElement("time")]
      public DateTime? Time { get; set; } 
      [BsonElement("images")]
      public string? Images { get; set; } 
      [BsonElement("state")]
      public short? State { get; set; } 

      [BsonElement("customer")]
      public Customer? customer {get; set; }     
 
  } 