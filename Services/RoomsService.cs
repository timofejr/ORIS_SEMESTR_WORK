using MiniHttpServer.Framework.Context;
using MiniHttpServer.Models;
using MyORMLibrary;

namespace MiniHttpServer.Services;

public class RoomsService
{
    private readonly ORMContext _orm = GlobalContext.OrmContext;

    public IEnumerable<Room> GetRoomsByHotelId(int hotelId)
    {
        var rooms = _orm.Where<Room>(room => room.HotelId == hotelId);
    
        foreach (var room in rooms)
        {
            room.PhotoPath = _orm.FirstOrDefault<RoomPhoto> (photo => photo.HotelId == hotelId).FilePath;
        }

        return rooms;
    }
}