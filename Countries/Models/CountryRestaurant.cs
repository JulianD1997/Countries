using Countries.Models;
using System.Text.Json.Serialization;

public class CountryRestaurant
{
    public int CountryId { get; set; }
    public int RestaurantId { get; set; }
    [JsonIgnore]
    public Country Country { get; set; }
    [JsonIgnore]
    public Restaurant Restaurant { get; set; }
}