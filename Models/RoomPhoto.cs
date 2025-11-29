using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHttpServer.Models;

[Table("room_photo")]
public class RoomPhoto
{
    [Column("photo_id")]
    public int PhotoId { get; set; }
    
    [Column("file_path")]
    public string FilePath { get; set; }
    
    [Column("room_id")]
    public int HotelId { get; set; }
}