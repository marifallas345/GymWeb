using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GymWEB.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();

            _httpClient.BaseAddress =
                new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]);
        }

        private void AgregarToken()
        {
            if (HttpContext.Current?.Session == null)
                return;

            var token = HttpContext.Current.Session["Token"] as string;

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        //==========================
        // GET
        //==========================
        public async Task<T> GetAsync<T>(string endpoint)
        {
            AgregarToken();

            HttpResponseMessage response =
                await _httpClient.GetAsync(endpoint).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(json);
        }

        //==========================
        // POST
        //==========================
        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest data)
        {
            AgregarToken();

            string json = JsonConvert.SerializeObject(data);

            StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await _httpClient.PostAsync(endpoint, content).ConfigureAwait(false);

            string resultado =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(resultado);
            }

            return JsonConvert.DeserializeObject<TResponse>(resultado);
        }

        //==========================
        // POST SIMPLE
        //==========================
        public async Task<bool> PostSimpleAsync<T>(
            string endpoint,
            T data)
        {
            AgregarToken();

            string json =
                JsonConvert.SerializeObject(data);

            StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await _httpClient.PostAsync(endpoint, content).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        //==========================
        // PUT
        //==========================
        public async Task<bool> PutAsync<T>(
            string endpoint,
            T data)
        {
            AgregarToken();

            string json =
                JsonConvert.SerializeObject(data);

            StringContent content =
                new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await _httpClient.PutAsync(endpoint, content).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        //==========================
        // DELETE
        //==========================
        public async Task<bool> DeleteAsync(string endpoint)
        {
            AgregarToken();

            HttpResponseMessage response =
                await _httpClient.DeleteAsync(endpoint).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}