
using System;
using System.Net;

namespace GymWEB.Exceptions
{

    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode statusCode, string mensaje)
            : base(mensaje)
        {
            StatusCode = statusCode;
        }

        public bool EsNoAutorizado =>
            StatusCode == HttpStatusCode.Unauthorized;

        public bool EsProhibido =>
            StatusCode == HttpStatusCode.Forbidden;
    }
}
