using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Countries.Models;

public class Hotel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Starts { get; set; }
    [JsonIgnore]
    public List<CountryHotel> CountryHotels { get; set; }
}