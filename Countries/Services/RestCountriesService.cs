using System.Text.Json;
using Countries.Models.Dto;
using Countries.Services.Interfaces;
using System.Text.Json.Nodes;
using AutoMapper;
using Countries.Models;

namespace Countries.Services
{
    public class RestCountriesService : IRestCountriesService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private const string Url = "https://countriesnow.space/api/v0.1/countries";

        public RestCountriesService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            var response = await _httpClient.GetAsync(Url);
            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            JsonNode nodes = JsonNode.Parse(jsonData);
            JsonNode results = nodes["data"];
            var countryData = JsonSerializer.Deserialize<List<CountryData>>(results.ToString());
            var countries = _mapper.Map<List<Country>>(countryData);
            return countries;
        }
    }
}
