
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Countries.Models;

public class Restaurant
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    [JsonIgnore]
    public List<CountryRestaurant> CountryRestaurants { get; set; }
}