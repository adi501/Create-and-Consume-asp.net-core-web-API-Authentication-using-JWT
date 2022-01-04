using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine(GetWebAPIToken());
            GetUserInfo();
            Console.ReadLine();
        }
        public static void GetUserInfo()
        {
            string AccessToken = GetWebAPIToken();
            var BaseUrl = "https://localhost:44376/api/Users/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",AccessToken);
                var response = client.GetAsync(BaseUrl).Result;
                string JsonResult = response.Content.ReadAsStringAsync().Result;
                var content1 = JsonResult;
                var jsonResult = JsonConvert.DeserializeObject(content1).ToString();
                var ApiResponse = JsonConvert.DeserializeObject<List<User>>(jsonResult);
                foreach(var obj in ApiResponse)
                {
                    Console.WriteLine("Id=" + obj.Id + ", Name=" + obj.Name);
                }

            }
        }
        public static string GetWebAPIToken()
        {
            try
            {
                var userName = "adi";
                var password = "password";
                string TokenUrl = "https://localhost:44376/api/Users/authentication";
                string data = "{'userName':'" + userName + "','password':'" + password + "'}";
                data = data.Replace("'", "\"");
                var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(TokenUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PostAsync(TokenUrl, content).Result;

                    string JsonResult = response.Content.ReadAsStringAsync().Result;
                    var jsonResult = JsonConvert.DeserializeObject(JsonResult).ToString();
                    var ApiResponse = JsonConvert.DeserializeObject<TokenResponce>(jsonResult);
                    return ApiResponse.Token;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class TokenResponce
    {
        public string Token { get; set; }
        public string Issuer { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
