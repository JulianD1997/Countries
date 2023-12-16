using System.ComponentModel.DataAnnotations;

namespace Countries.Models;

public class Country
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; }
    public List<CountryRestaurant> CountryRestaurants { get; set;}
    public List<CountryHotel> CountryHotels { get; set;}
}
