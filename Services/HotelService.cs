using MiniHttpServer.DTOs;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Models;
using MyORMLibrary;

namespace MiniHttpServer.Services;

public class HotelService
{
    private readonly ORMContext _orm = GlobalContext.OrmContext;

    public IEnumerable<Hotel> GetHotels(GetHotelsRequestDto data)
    {
        var hotels = _orm.Where<Hotel>(hotel => hotel.Resort == data.ResortName);

        if (data.Categories.Any())
            hotels = hotels.Where(hotel => data.Categories.Contains(hotel.Category));
        if (data.FoodTypes.Any())
            hotels = hotels.Where(hotel => data.FoodTypes.Contains(hotel.FoodType));
        if (data.BeachTypes.Any())
            hotels = hotels.Where(hotel => data.BeachTypes.Contains(hotel.BeachType));
        if (data.RoomTypes.Any())
            hotels = hotels.Where(hotel => data.RoomTypes.Contains(hotel.RoomType));
        if (data.BeachDistances.Any())
            hotels = hotels.Where(hotel => data.BeachDistances.Contains("Любое") || data.BeachDistances.Contains(hotel.BeachDistance.ToString()));
        hotels = data.Sort == "price_desc" ? hotels.OrderByDescending(hotel => hotel.Price) : hotels.OrderBy(hotel => hotel.Price);
        hotels = hotels.Where(hotel => data.PriceFrom <= hotel.Price * data.DaysAmount && data.PriceTo >= hotel.Price * data.DaysAmount);
        
        foreach (var hotel in hotels)
        {
            hotel.PhotoPath = _orm.FirstOrDefault<HotelPhoto> (photo => photo.HotelId == hotel.HotelId).FilePath;
        }
        
        return  hotels;
    }

    public Hotel GetHotelById(int hotelId)
    {
        var hotel = _orm.FirstOrDefault<Hotel>(hotel => hotel.HotelId == hotelId);
        
        hotel.PhotoPath = _orm.FirstOrDefault<HotelPhoto> (photo => photo.HotelId == hotel.HotelId).FilePath;

        return hotel;
    }
}