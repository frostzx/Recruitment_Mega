using Frontend_Mega_Recruitment.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frontend_Mega_Recruitment.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateUser(LoginModel loginModel);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> AuthenticateUser(LoginModel loginModel)
        {
            var loginUrl = "https://localhost:7156/api/User/login";
            var jsonContent = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(loginUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var jsonObject = JObject.Parse(responseBody);
                    var user = new User
                    {
                        UserId = jsonObject["user"]["userId"].Value<int>(),
                        Username = jsonObject["user"]["username"].Value<string>(),
                        IsActive = jsonObject["user"]["isActive"].Value<bool>(),
                        Status = jsonObject["user"]["status"].Value<string>()
                    };

                    return user;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                    return null;
                }
            }

            return null;
        }
    }
}
