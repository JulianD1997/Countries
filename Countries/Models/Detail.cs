using System.Text.Json.Serialization;

namespace Countries.Models;

public class Detail
{
    [JsonIgnore]
    public ResponseStatus Status { get; set; }

    public bool IsSuccessful { get; set; }

    public string Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Country>? Countries { get; set; } 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Hotel>? Hotels { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Restaurant>? Restaurants { get; set; }
}
public enum ResponseStatus : int
{
    Success = 200,
    Created = 201,
    NoContent = 204,
    BadRequest = 400,
    NotFound = 404,
}