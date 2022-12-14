namespace AuctionService;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
 
  public class Productdto
  { 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
      public string? Id { get; set; } 
 
      [BsonElement("price")]
      public double Price { get; set; } 

    public Productdto(string id, double price){
      Price = price;
      Id = id;
    }

    public Productdto(){}
 
  } 