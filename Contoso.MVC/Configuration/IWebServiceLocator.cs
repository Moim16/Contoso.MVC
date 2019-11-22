using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.MVC.Configuration
{
   public interface IWebServiceLocator
    {
        /*Solo propiedad de lectura*/
        string ServiceAddress { get; }

        string ServiceAddressAbogado { get; }
        
    }
}
