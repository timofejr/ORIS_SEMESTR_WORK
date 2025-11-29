using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHttpServer.Models;

[Table("room")]
public class Room
{
    [Column("room_id")]
    public int RoomId  { get; set; }
    
    [Column("hotel_id")]
    public int HotelId  { get; set; }

    [Column("photo_path")]
    public string PhotoPath { get; set; } = null;
    
    [Column("price_per_night")]
    public int PricePerNight { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
    
}