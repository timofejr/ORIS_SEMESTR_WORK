namespace MiniHttpServer.DTOs;

public class GetHotelsResponseDto
{
    public string HotelId  { get; set; }
    public string Resort { get; set; }
    public string Location { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string Conception { get; set; }
    public string Price { get; set; }
}