using System.Net;
using System.Text.Json;
using MiniHttpServer.DTOs;
using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;
using MiniHttpServer.Services;

namespace MiniHttpServer.Controllers;

[Controller]
public class ToursController: BaseController
{
    [HttpGet("/")]
    public IResponseResult MainPage()
    {
        return Page("/tours/index.html");
    }

    [HttpPost("/getHotels/")]
    public IResponseResult GetHotels()
    {
        if (!HttpContext.Request.HasEntityBody)
            return Json(string.Empty, HttpStatusCode.BadRequest);

        using var reader = new StreamReader(HttpContext.Request.InputStream, HttpContext.Request.ContentEncoding);
        var body = reader.ReadToEnd();

        if (string.IsNullOrEmpty(body))
            return Json(string.Empty, HttpStatusCode.BadRequest);

        try
        {
            var data = JsonSerializer.Deserialize<GetHotelsRequestDto>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var hotelService = new HotelService();

            var hotels = hotelService.GetHotels(data).ToList();

            foreach (var hotel in hotels)
                hotel.Price = data.DaysAmount * hotel.Price;

            var jsonString = JsonSerializer.Serialize(hotels, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return Json(jsonString);
        }
        catch (Exception ex)
        {
            return Json(ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("/{hotelId}", true)]
    public IResponseResult GetHotel(string hotelId)
    {
        if (!int.TryParse(hotelId, out _))
            return Json("Invalid hotel ID", HttpStatusCode.BadRequest);
        
        return Page("/tours/hotel.html");
    }
}