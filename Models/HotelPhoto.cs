using System.ComponentModel.DataAnnotations.Schema;

namespace MiniHttpServer.Models;

[Table("hotel_photo")]
public class HotelPhoto
{
    [Column("photo_id")]
    public int PhotoId { get; set; }
    
    [Column("file_path")]
    public string FilePath { get; set; }
    
    [Column("hotel_id")]
    public int HotelId { get; set; }
}