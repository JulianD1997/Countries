using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Countries.Models;

public class Country
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; }
    [JsonIgnore]
    public List<CountryRestaurant> CountryRestaurants { get; set;}
    public List<Restaurant> Restaurants { get; set;}
    [JsonIgnore]
    public List<CountryHotel> CountryHotels { get; set;}
    public List<Hotel> Hotels { get; set;}
}

