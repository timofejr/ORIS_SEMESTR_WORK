namespace MiniHttpServer.DTOs;

public class GetHotelsRequestDto
{
    public string ResortName { get; set; }
    public int DaysAmount { get; set; }
    public List<string> Categories { get; set; }
    public List<string> FoodTypes { get; set; }
    public int PriceFrom { get; set; }
    public int PriceTo { get; set; }
    public List<string> BeachDistances { get; set; }
    public List<string> Services { get; set; }
    public List<string> Conceptions { get; set; }
    public List<string> BeachTypes { get; set; }
    public List<string> RoomTypes { get; set; }
    public List<string> HotelServices { get; set; }
}