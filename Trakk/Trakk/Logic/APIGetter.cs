using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Trakk.Models;
using Newtonsoft.Json;
using Trakk.Viewmodels;

namespace Trakk.Logic
{
    public class APIGetter : IAPIGetter
    {
        public Uri Uri = new Uri(System.Configuration.ConfigurationManager.AppSettings["APIAddress"]); // change to config key

        IEventLogic _eventLogic = new EventLogic();
        public async Task<Fixture> GetFixture(int id)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("fixtures/GET/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                Fixture fixture = JsonConvert.DeserializeObject<Fixture>(textResult);
                return fixture;
            }
        }

        public async Task<Fixture> GetFixture(FixtureAvailabilityViewModel fixtureRequest)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var content = new StringContent(JsonConvert.SerializeObject(fixtureRequest), Encoding.UTF8, "application/json");
                var response = client.PostAsync("fixtures/GetWithAvailability/",  content).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                Fixture fixture = JsonConvert.DeserializeObject<Fixture>(textResult);
                return fixture;
            }
        }

        public async Task<List<Fixture>> GetAllFixtures()
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("fixtures/GET/").Result;
                string textResult = await response.Content.ReadAsStringAsync();
                List<Fixture> fixture = JsonConvert.DeserializeObject<List<Fixture>>(textResult);
                return fixture;
            }
        }

        public async Task<Event> GetEvent(int? id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("events/GET/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                Event @event = JsonConvert.DeserializeObject<Event>(textResult);
                return @event;
            }
        }

        public async Task<Team> GetTeam(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/teams/GET/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                Team team = JsonConvert.DeserializeObject<Team>(textResult);
                return team;
            }
        }

        public async Task<List<Team>> GetAllTeams(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/teams/GET/").Result;
                string textResult = await response.Content.ReadAsStringAsync();
                List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(textResult);
                return teams;
            }
        }

        public async Task<TeamMember> GetUser(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/users/GET/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                TeamMember member = JsonConvert.DeserializeObject<TeamMember>(textResult);
                return member;
            }
        }

        public async Task<List<Event>> GetUserEvents(int id, bool primary)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/fixtures/member/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                List<Event> events = new List<Event>();
                events.AddRange(JsonConvert.DeserializeObject<List<Fixture>>(textResult));
                response = client.GetAsync("/events/member/" + id).Result;
                textResult = await response.Content.ReadAsStringAsync();
                events.AddRange(JsonConvert.DeserializeObject<List<Event>>(textResult));
                if (primary)
                    events = _eventLogic.GetPrimaryEvents(events);
                return events;
            }
        }

        public async Task<List<Event>> GetPrimaryEvents(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/fixtures/member/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                List<Event> events = new List<Event>();
                events.AddRange(JsonConvert.DeserializeObject<List<Fixture>>(textResult));
                response = client.GetAsync("/events/member/" + id).Result;
                textResult = await response.Content.ReadAsStringAsync();
                events.AddRange(JsonConvert.DeserializeObject<List<Event>>(textResult));
                return events;
            }
        }

        public async Task<List<TeamMember>> GetAllUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/users/GET/").Result;
                string textResult = await response.Content.ReadAsStringAsync();
                List<TeamMember> team = JsonConvert.DeserializeObject<List<TeamMember>>(textResult);
                return team;
            }
        }

        public async Task<Sport> GetSport(int? id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/sports/GET/" + id).Result;
                string textResult = await response.Content.ReadAsStringAsync();
                Sport sport = JsonConvert.DeserializeObject<Sport>(textResult);
                return sport;
            }
        }


        public async Task<List<Sport>> GetAllSports()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                var response = client.GetAsync("/sports/GET/").Result;
                string textResult = await response.Content.ReadAsStringAsync();
                List<Sport> sport = JsonConvert.DeserializeObject<List<Sport>>(textResult);
                return sport;
            }
        }

        public async Task<List<Sport>> GetSportList(List<Sport> sportsList)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = Uri;
                List<int> ids = sportsList.Select(i => i.Id).ToList();
                List<Sport> sports = new List<Sport>();
                foreach (int id in ids)
                {
                    var response = client.GetAsync("/sports/GET/" + id).Result;
                    string textResult = await response.Content.ReadAsStringAsync();
                    sports.Add(JsonConvert.DeserializeObject<Sport>(textResult));
                }

                return sports;
            }
        }
    }
}