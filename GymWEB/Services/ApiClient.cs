using GymWEB.Exceptions;
using Newtonsoft.Json;
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
        // =========================================================
        // HTTP CLIENT
        // =========================================================

        private static readonly HttpClient _httpClient;

        // =========================================================
        // CONFIGURACIÓN INICIAL
        // =========================================================

        static ApiClient()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            _httpClient = new HttpClient();

            string apiBaseUrl =
                ConfigurationManager
                    .AppSettings["ApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(apiBaseUrl))
            {
                throw new Exception(
                    "No se encontró ApiBaseUrl en Web.config.");
            }

            if (!apiBaseUrl.EndsWith("/"))
            {
                apiBaseUrl += "/";
            }

            _httpClient.BaseAddress =
                new Uri(apiBaseUrl);

            string timeoutConfig =
                ConfigurationManager
                    .AppSettings["ApiTimeoutSeconds"];

            if (int.TryParse(
                timeoutConfig,
                out int timeoutSeconds))
            {
                _httpClient.Timeout =
                    TimeSpan.FromSeconds(
                        timeoutSeconds);
            }
            else
            {
                _httpClient.Timeout =
                    TimeSpan.FromSeconds(30);
            }
        }

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ApiClient()
        {
            // El HttpClient ya fue configurado
            // en el constructor estático.
        }

        // =========================================================
        // TOKEN
        // =========================================================

        private void AgregarToken()
        {
            string token =
                HttpContext.Current?
                    .Session["Token"] as string;

            _httpClient.DefaultRequestHeaders
                .Authorization = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders
                    .Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }
        }

        // =========================================================
        // GET
        // =========================================================

        public async Task<T> GetAsync<T>(
            string endpoint)
        {
            AgregarToken();

            HttpResponseMessage response;

            try
            {
                response =
                    await _httpClient
                        .GetAsync(endpoint)
                        .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                throw new ApiException(
                    HttpStatusCode.RequestTimeout,
                    "La solicitud tardó demasiado tiempo.");
            }
            catch (HttpRequestException)
            {
                throw new ApiException(
                    HttpStatusCode.ServiceUnavailable,
                    "No se pudo conectar con la API.");
            }

            await ValidarRespuestaAsync(response);

            string contenido =
                await response.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

            return JsonConvert
                .DeserializeObject<T>(contenido);
        }

        // =========================================================
        // POST
        // =========================================================

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest objeto)
        {
            AgregarToken();

            string json =
                JsonConvert.SerializeObject(objeto);

            using (var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"))
            {
                HttpResponseMessage response;

                try
                {
                    response =
                        await _httpClient
                            .PostAsync(
                                endpoint,
                                content)
                            .ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    throw new ApiException(
                        HttpStatusCode.RequestTimeout,
                        "La solicitud tardó demasiado tiempo.");
                }
                catch (HttpRequestException)
                {
                    throw new ApiException(
                        HttpStatusCode.ServiceUnavailable,
                        "No se pudo conectar con la API.");
                }

                await ValidarRespuestaAsync(response);

                string contenido =
                    await response.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                return JsonConvert
                    .DeserializeObject<TResponse>(
                        contenido);
            }
        }

        // =========================================================
        // POST SIMPLE
        // =========================================================

        public async Task<bool> PostSimpleAsync<T>(
            string endpoint,
            T objeto)
        {
            AgregarToken();

            string json =
                JsonConvert.SerializeObject(objeto);

            using (var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"))
            {
                HttpResponseMessage response;

                try
                {
                    response =
                        await _httpClient
                            .PostAsync(
                                endpoint,
                                content)
                            .ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    throw new ApiException(
                        HttpStatusCode.RequestTimeout,
                        "La solicitud tardó demasiado tiempo.");
                }
                catch (HttpRequestException)
                {
                    throw new ApiException(
                        HttpStatusCode.ServiceUnavailable,
                        "No se pudo conectar con la API.");
                }

                await ValidarRespuestaAsync(response);

                return true;
            }
        }

        // =========================================================
        // PUT
        // =========================================================

        public async Task<bool> PutAsync<T>(
            string endpoint,
            T objeto)
        {
            AgregarToken();

            string json =
                JsonConvert.SerializeObject(objeto);

            using (var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"))
            {
                HttpResponseMessage response;

                try
                {
                    response =
                        await _httpClient
                            .PutAsync(
                                endpoint,
                                content)
                            .ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    throw new ApiException(
                        HttpStatusCode.RequestTimeout,
                        "La solicitud tardó demasiado tiempo.");
                }
                catch (HttpRequestException)
                {
                    throw new ApiException(
                        HttpStatusCode.ServiceUnavailable,
                        "No se pudo conectar con la API.");
                }

                await ValidarRespuestaAsync(response);

                return true;
            }
        }

        // =========================================================
        // DELETE
        // =========================================================

        public async Task<bool> DeleteAsync(
            string endpoint)
        {
            AgregarToken();

            HttpResponseMessage response;

            try
            {
                response =
                    await _httpClient
                        .DeleteAsync(endpoint)
                        .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                throw new ApiException(
                    HttpStatusCode.RequestTimeout,
                    "La solicitud tardó demasiado tiempo.");
            }
            catch (HttpRequestException)
            {
                throw new ApiException(
                    HttpStatusCode.ServiceUnavailable,
                    "No se pudo conectar con la API.");
            }

            await ValidarRespuestaAsync(response);

            return true;
        }

        // =========================================================
        // VALIDAR RESPUESTA
        // =========================================================

        private async Task ValidarRespuestaAsync(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string mensaje =
                await ExtraerMensajeErrorAsync(response)
                    .ConfigureAwait(false);

            // =====================================================
            // 401 - NO AUTORIZADO
            // =====================================================

            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                var session =
                    HttpContext.Current?.Session;

                if (session != null)
                {
                    session.Clear();
                }
            }

            // =====================================================
            // 403 - PROHIBIDO
            // =====================================================

            if (response.StatusCode ==
                HttpStatusCode.Forbidden)
            {
                if (string.IsNullOrWhiteSpace(mensaje))
                {
                    mensaje =
                        "No tienes permisos para realizar esta acción.";
                }
            }

            // =====================================================
            // LANZAR EXCEPCIÓN
            // =====================================================

            throw new ApiException(
                response.StatusCode,
                mensaje);
        }

        // =========================================================
        // EXTRAER MENSAJE DE ERROR
        // =========================================================

        private async Task<string>
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
                        dynamic error =
                            JsonConvert
                                .DeserializeObject(
                                    contenido);

                        if (error?.mensaje != null)
                        {
                            return error.mensaje
                                .ToString();
                        }

                        if (error?.message != null)
                        {
                            return error.message
                                .ToString();
                        }

                        if (error?.title != null)
                        {
                            return error.title
                                .ToString();
                        }
                    }
                    catch
                    {
                        // Si no es JSON,
                        // utilizamos el contenido directamente.
                    }
                }
            }
            catch
            {
                // Ignorar errores al intentar
                // leer el mensaje.
            }

            // =====================================================
            // MENSAJES SEGÚN CÓDIGO HTTP
            // =====================================================

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:

                    return
                        "Tu sesión expiró o no tienes autorización. Inicia sesión nuevamente.";

                case HttpStatusCode.Forbidden:

                    return
                        "No tienes permisos para realizar esta acción.";

                case HttpStatusCode.NotFound:

                    return
                        "El recurso solicitado no existe.";

                case HttpStatusCode.BadRequest:

                    return
                        "Los datos enviados no son válidos.";

                case HttpStatusCode.InternalServerError:

                    return
                        "Ocurrió un error interno en el servidor.";

                case HttpStatusCode.ServiceUnavailable:

                    return
                        "El servicio no está disponible.";

                case HttpStatusCode.RequestTimeout:

                    return
                        "La solicitud tardó demasiado tiempo.";

                default:

                    return
                        $"La API respondió con el código {(int)response.StatusCode}.";
            }
        }
    }
}