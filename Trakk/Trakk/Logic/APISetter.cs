using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using API.Models;
using Trakk.Models;
using Newtonsoft.Json;
using Trakk.Helpers;
using Trakk.Viewmodels;

namespace Trakk.Logic
{

    public class APISetter : IAPISetter
    {
        public Uri Uri = new Uri("http://localhost:63751/"); // change to config key
        public string path;

        public async Task<EntityResponse> CreateUser(TeamMember member)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(member), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("users/POST/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> UpdateUser(TeamMember user)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("users/PUT/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> CreateTeam(TeamReturnCreateViewModel team)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(team), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("teams/POST/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> UpdateTeam(TeamReturnEditViewModel team)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(team), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("teams/PUT/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }
        public async Task<EntityResponse> CreateSport(Sport sport)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(sport), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("sports/POST/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> UpdateSport(Sport sport)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(sport), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("sports/PUT/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }


        public async Task<EntityResponse> CreateFormation(Formation formation)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(formation), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("formations/POST/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }
        public async Task<EntityResponse> UpdateFormation(Formation formation)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(formation), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("formations/PUT/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> CreateFixture(FixtureCreateReturnViewModel fixture)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(fixture), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("fixtures/POST/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> UpdateFixture(FixtureCreateReturnViewModel fixture)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(fixture), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("fixtures/PUT/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> CreateEvent(EventReturnCreateViewModel newEvent)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(newEvent), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("events/POST/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }

        public async Task<EntityResponse> UpdateEvent(EventReturnEditViewModel eventUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(eventUpdate), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("events/PUT/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }


        public async Task<EntityResponse> UpdateAvailability(PlayerEventAvailability eventUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(eventUpdate), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("events/CreateAvailability/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }
        public async Task<EntityResponse> UpdateFixtureAvailability(PlayerFixtureAvailability eventUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(eventUpdate), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("fixtures/CreateAvailability/", content);
                string textResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EntityResponse>(textResult);
            }
        }
    }
}