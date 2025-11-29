using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHttpServer.Models;

[Table("hotel")]
public class Hotel
{
    [Column("hotel_id")]
    public int HotelId  { get; set; }
    
    [Column("resort")]
    public string Resort { get; set; }
    
    [Column("location")]
    public string Location { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("category")]
    public string Category { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
    [Column("conception")]
    public string Conception { get; set; }
    
    [Column("minimal_price_per_night")]
    public int Price { get; set; }
    
    [Column("photo_path")]
    public string PhotoPath { get; set; } = null;
    
    [Column("food_type")]
    public string FoodType { get; set; }
    
    [Column("beach_distance")]
    public int BeachDistance { get; set; }
    
    [Column("beach_type")]
    public string BeachType { get; set; }
    
    [Column("room_type")]
    public string RoomType { get; set; }
}