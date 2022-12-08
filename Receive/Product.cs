using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace productReceive;
 
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
      public Customer? customer {get; }   
      public Product(string? name, string? description, string? category, double? assesment, double? price, double? minbid, DateTime? time, string? images, short? state, Customer? customer)
        {
            name = Name;
            description = Description;
            category = Category;
            assesment = Assesment;
            price = Price;
            minbid = MinBid;
            time = Time;
            images = Images;
            state = State;
        }  
 
  } 