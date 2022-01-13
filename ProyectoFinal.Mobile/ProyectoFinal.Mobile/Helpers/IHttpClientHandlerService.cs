using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ProyectoFinal.Mobile.Helpers
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
