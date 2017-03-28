using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Trakk.Models;
using Newtonsoft.Json;
using Trakk.Helpers;

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

        public async Task<EntityResponse> CreateTeam(Team team)
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

        public async Task<EntityResponse> UpdateTeam(Team team)
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
    }
}