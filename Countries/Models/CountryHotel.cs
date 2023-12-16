using System.Text.Json.Serialization;

namespace Countries.Models;

public class CountryHotel
{
    public int CountryId { get; set; }
    public int HotelId { get; set; }
    [JsonIgnore]
    public Country Country { get; set;}
    [JsonIgnore]
    public Hotel Hotel { get; set;}

}
