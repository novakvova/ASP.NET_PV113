using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebRozetka.Data;
using WebRozetka.Data.Entities;
using WebRozetka.Data.Entities.Addres;
using WebRozetka.Interfaces;
using WebRozetka.Models.NovaPoshta;

namespace WebRozetka.Services
{
    public class NovaPoshtaService : INovaPoshtaService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly AppEFContext _context;

        public NovaPoshtaService(IConfiguration configuration, IMapper mapper,
            AppEFContext context)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _mapper = mapper; 
            _context = context;
        }

        public void GetAreas()
        {
            string key = _configuration.GetValue<string>("NovaposhtaKey");
            NPAreaRequestViewModel model = new NPAreaRequestViewModel
            {
                ApiKey = key,
                ModelName = "Address",
                CalledMethod = "getSettlementAreas",
                MethodProperties = new NPAreaProperties
                {
                    Page = 1,
                    Ref = ""
                }
            };

            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync("https://api.novaposhta.ua/v2.0/json/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<NPAreaResponseViewModel> (responseData);
                if (result.Data.Any())
                {
                    List<AreaEntity> dataEntities = _mapper.Map<List<AreaEntity>>(result.Data);
                    _context.Areas.AddRange(dataEntities);
                    _context.SaveChanges();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine($"Error novaposhta: {response.StatusCode}");
            }
        }

        public void GetSettlements()
        {
            string key = _configuration.GetValue<string>("NovaposhtaKey");
            int page = 1;
            while(true)
            {
                NPSettlementRequestViewModel model = new NPSettlementRequestViewModel
                {
                    ApiKey = key,
                    ModelName = "AddressGeneral",
                    CalledMethod = "getSettlements",
                    MethodProperties = new NPSettlementProperties
                    {
                        Page = page
                    }
                };

                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync("https://api.novaposhta.ua/v2.0/json/", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<NPSettlementResponseViewModel>(responseData);
                    if (result.Data.Any())
                    {
                        List<SettlementEntity> dataEntities = 
                            _mapper.Map<List<SettlementEntity>>(result.Data);

                        _context.Settlements.AddRange(dataEntities);
                        _context.SaveChanges();
                        page++;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Error novaposhta: {response.StatusCode}");
                }
            }
            
        }

        public void GetWarehouses()
        {
            string key = _configuration.GetValue<string>("NovaposhtaKey");
            int page = 1;
            while (true)
            {
                NPWarehouseRequestViewModel model = new NPWarehouseRequestViewModel
                {
                    ApiKey = key,
                    ModelName = "Address",
                    CalledMethod = "getWarehouses",
                    MethodProperties = new NPWarehouseProperties
                    {
                        Page = page
                    }
                };

                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync("https://api.novaposhta.ua/v2.0/json/", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<NPWarehouseResponseViewModel>(responseData);
                    if (result.Data.Any())
                    {
                        List<WarehouseEntity> dataEntities =
                            _mapper.Map<List<WarehouseEntity>>(result.Data);

                        _context.Warehouses.AddRange(dataEntities);
                        _context.SaveChanges();
                        page++;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Error novaposhta: {response.StatusCode}");
                }
            }
        }
    }
}
