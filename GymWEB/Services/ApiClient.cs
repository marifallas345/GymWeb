using GymWEB.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GymWEB.Services
{
    public class ApiClient
    {
        private static readonly HttpClient _httpClient =
            CrearHttpClient();

        // =========================================================
        // CREAR HTTP CLIENT
        // =========================================================

        private static HttpClient CrearHttpClient()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            var apiBaseUrl =
                ConfigurationManager.AppSettings["ApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(apiBaseUrl))
            {
                throw new Exception(
                    "No se encontró ApiBaseUrl en Web.config.");
            }

            var client = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl),
                Timeout = TimeSpan.FromSeconds(
                    ObtenerTimeoutSegundos())
            };

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(
                    "application/json"));

            return client;
        }

        // =========================================================
        // TIMEOUT
        // =========================================================

        private static int ObtenerTimeoutSegundos()
        {
            var valor =
                ConfigurationManager.AppSettings[
                    "ApiTimeoutSeconds"];

            if (int.TryParse(valor, out int segundos)
                && segundos > 0)
            {
                return segundos;
            }

            return 15;
        }

        // =========================================================
        // OBTENER TOKEN DE SESIÓN
        // =========================================================

        private string ObtenerToken()
        {
            return HttpContext.Current?
                .Session?["Token"] as string;
        }

        // =========================================================
        // CREAR REQUEST
        // =========================================================

        private HttpRequestMessage CrearRequest(
            HttpMethod metodo,
            string endpoint)
        {
            var request =
                new HttpRequestMessage(
                    metodo,
                    endpoint);

            var token = ObtenerToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            return request;
        }

        // =========================================================
        // CREAR CONTENIDO JSON
        // =========================================================

        private static StringContent CrearContenidoJson<T>(
            T data)
        {
            string json =
                JsonConvert.SerializeObject(data);

            return new StringContent(
                json,
                Encoding.UTF8,
                "application/json");
        }

        // =========================================================
        // ENVIAR REQUEST
        // =========================================================

        private async Task<HttpResponseMessage> EnviarAsync(
            HttpRequestMessage request)
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient
                    .SendAsync(request)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                throw new ApiException(
                    HttpStatusCode.RequestTimeout,
                    "GymAPI no respondió a tiempo. " +
                    "Intenta nuevamente en unos segundos.");
            }
            catch (HttpRequestException)
            {
                throw new ApiException(
                    HttpStatusCode.ServiceUnavailable,
                    "No fue posible conectarse con GymAPI. " +
                    "Verifica que el servicio esté disponible.");
            }

            if (!response.IsSuccessStatusCode)
            {
                string mensaje =
                    await ExtraerMensajeErrorAsync(response)
                        .ConfigureAwait(false);

                throw new ApiException(
                    response.StatusCode,
                    mensaje);
            }

            return response;
        }

        // =========================================================
        // EXTRAER MENSAJE DE ERROR
        // =========================================================

        private static async Task<string>
            ExtraerMensajeErrorAsync(
                HttpResponseMessage response)
        {
            try
            {
                string contenido =
                    await response.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(contenido))
                {
                    try
                    {
                        var json =
                            JObject.Parse(contenido);

                        var mensaje =
                            json["mensaje"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(mensaje))
                        {
                            return mensaje;
                        }

                        var title =
                            json["title"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(title))
                        {
                            return title;
                        }

                        var detail =
                            json["detail"]?.ToString();

                        if (!string.IsNullOrWhiteSpace(detail))
                        {
                            return detail;
                        }
                    }
                    catch (JsonException)
                    {
                        // El contenido no era JSON.
                    }

                    return contenido;
                }
            }
            catch
            {
                // Se utilizará el mensaje según el código HTTP.
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:

                    return "La solicitud enviada no es válida.";

                case HttpStatusCode.Unauthorized:

                    return "Tu sesión expiró o no tienes " +
                           "autorización. Inicia sesión nuevamente.";

                case HttpStatusCode.Forbidden:

                    return "No tienes permisos para realizar " +
                           "esta acción.";

                case HttpStatusCode.NotFound:

                    return "El recurso solicitado no existe.";

                case HttpStatusCode.Conflict:

                    return "La operación no se puede realizar " +
                           "porque existe un conflicto con los datos.";

                case HttpStatusCode.InternalServerError:

                    return "GymAPI presentó un error interno.";

                case HttpStatusCode.ServiceUnavailable:

                    return "GymAPI no está disponible.";

                default:

                    return "Ocurrió un error al comunicarse " +
                           "con GymAPI.";
            }
        }

        // =========================================================
        // GET
        // =========================================================

        public async Task<T> GetAsync<T>(
            string endpoint)
        {
            using (var request =
                CrearRequest(
                    HttpMethod.Get,
                    endpoint))
            {
                using (var response =
                    await EnviarAsync(request)
                        .ConfigureAwait(false))
                {
                    string json =
                        await response.Content
                            .ReadAsStringAsync()
                            .ConfigureAwait(false);

                    return JsonConvert
                        .DeserializeObject<T>(json);
                }
            }
        }

        // =========================================================
        // POST
        // =========================================================

        public async Task<TResponse>
            PostAsync<TRequest, TResponse>(
                string endpoint,
                TRequest data)
        {
            using (var request =
                CrearRequest(
                    HttpMethod.Post,
                    endpoint))
            {
                request.Content =
                    CrearContenidoJson(data);

                using (var response =
                    await EnviarAsync(request)
                        .ConfigureAwait(false))
                {
                    string json =
                        await response.Content
                            .ReadAsStringAsync()
                            .ConfigureAwait(false);

                    return JsonConvert
                        .DeserializeObject<TResponse>(
                            json);
                }
            }
        }

        // =========================================================
        // POST SIMPLE
        // =========================================================

        public async Task<bool> PostSimpleAsync<T>(
            string endpoint,
            T data)
        {
            using (var request =
                CrearRequest(
                    HttpMethod.Post,
                    endpoint))
            {
                request.Content =
                    CrearContenidoJson(data);

                using (await EnviarAsync(request)
                    .ConfigureAwait(false))
                {
                    return true;
                }
            }
        }

        // =========================================================
        // PUT
        // =========================================================

        public async Task<bool> PutAsync<T>(
            string endpoint,
            T data)
        {
            using (var request =
                CrearRequest(
                    HttpMethod.Put,
                    endpoint))
            {
                request.Content =
                    CrearContenidoJson(data);

                using (await EnviarAsync(request)
                    .ConfigureAwait(false))
                {
                    return true;
                }
            }
        }

        // =========================================================
        // DELETE
        // =========================================================

        public async Task<bool> DeleteAsync(
            string endpoint)
        {
            using (var request =
                CrearRequest(
                    HttpMethod.Delete,
                    endpoint))
            {
                using (await EnviarAsync(request)
                    .ConfigureAwait(false))
                {
                    return true;
                }
            }
        }
    }
}