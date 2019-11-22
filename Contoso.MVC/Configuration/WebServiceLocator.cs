using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*Se agregan los Using*/
using Microsoft.Extensions.Configuration;// Para ir al appsettings que permita.

namespace Contoso.MVC.Configuration
{
    public class WebServiceLocator : IWebServiceLocator
    {


        public WebServiceLocator(IConfiguration  config)
        {
            var customSection = config.GetSection(nameof(WebServiceLocator));
            ServiceAddress=customSection?.GetSection("ServiceAddressContoso")?.Value;
            ServiceAddressAbogado = customSection?.GetSection("ServiceAddressAbogado")?.Value;
        }
        public string ServiceAddress { get; }
        public string ServiceAddressAbogado { get; }

    }
}
