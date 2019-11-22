using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*Agregando Using*/
using System.Net.Http;
using Newtonsoft.Json;
using Contoso.MVC.Configuration;
using System.Net.Http.Headers;//Para manipular encabezados de las solicitudes
using System.Text;

namespace Contoso.MVC.WebServiceAccess
{
    //Esta clase no debe instanciarse directamente por eso sera abstracta
    public abstract class WebApiCallsBase
    {
        protected readonly string ServiceAddress;
        protected readonly string ServiceAddressAbogado;
        protected readonly string StudentBaseUri;
        protected readonly string AbogadoBaseUri;

        protected HttpResponseHeaders _headers;
        public WebApiCallsBase(IWebServiceLocator ContosoService)
        {
            /*Esta es la direccion del servicio web csharp7 en adelante*/
            ServiceAddress = ContosoService.ServiceAddress;
            ServiceAddressAbogado = ContosoService.ServiceAddressAbogado;
            StudentBaseUri = $"{ServiceAddress}api/Estudent";
            AbogadoBaseUri= $"{ServiceAddressAbogado}DGI/ConsultaAbogado/SolicitudId/DGI/NCarnet";
        }

        /*Cualquier tipo de coleccion puede ser Dictionary, List o Collection*/
        internal async Task<IList<T>> GetItemListAsync<T>(string uri) where T : class, new()
        {
            try
            {
                /*Deserializamos porque lo que obtenemos es un json, lo convierte al tipo de entidad en este caso estudiante*/
                return JsonConvert.DeserializeObject<IList<T>>(await GetJsonFromGetResponseAsync(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        internal async Task<string> GetJsonFromGetResponseAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())/*Transaction Scope Todo codigo que se ejecute dentro de using automaticamente
                    hace un rollback, cierra todas las conexiones automaticamente*/
                {
                    /*dos tipos de mensajes RequestMessage y ResponseMessage*/

                    var response = await client.GetAsync(uri); /*Uri es el servicio Web, se manda ejecutar el get a uri*/
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"The call to {uri} failed. Status code:{response.StatusCode}");
                    }
                    _headers = response.Headers;
                    return await response.Content.ReadAsStringAsync();
                    /*Convierte el Json en cadena, funcionalidad asincronica la ejecuta en paralelo*/
                    /*En csharp existen tres palabras reservadas son*/
                    /*Async, Task y Await(Espera al resultado), tiene que haber un callback para indicar que lo que esta
                     * en background ya termino de ejecutar*/

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        internal async Task<List<T>> GetItemAsync<T>(string uri) where T : class, new()
        {
            try
            {
                var json = await GetJsonFromGetResponseAsync(uri);
                return  JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        protected async Task<string> SubmitPostRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                var task = client.PostAsync(uri, CreateStringContent(json));
                /*Averigua si la solicitud fue exitosa si fue exitosa lee el contenido sino crea la respuesta*/
                return await ExecuteRequestAndProcessResponse(uri, task);
            }
        }



        /*Este brother genera un stringcontent del json utilizando  Encoding*/
        protected StringContent CreateStringContent(string json)
        {
            /*Caracteres a nivel alfabetico de cada cultura el habla hispana, latinoamerica y norteamerica*/
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        /*El task es un httpResponseMessage*/
        protected static async Task<string> ExecuteRequestAndProcessResponse(string uri, Task<HttpResponseMessage> task)
        {
            try
            {
                var response = await task;/*La variable task contiene un mensaje respuesta de http pos*/
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"The call to {uri} failed. Status code:{response.StatusCode}");
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            throw new NotImplementedException();
        }
        protected async Task<string> SubmitPutRequestAsync(string uri, string json)
        {
            using (var client = new HttpClient())
            {
                Task<HttpResponseMessage> task = client.PutAsync(uri, CreateStringContent(json));
                return await ExecuteRequestAndProcessResponse(uri, task);
            }
        }
        protected async Task SubmitDeleteRequestAsync(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(uri);
                    var response = await deleteAsync;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        internal HttpResponseHeaders HeadersBase
        {
            get
            {
                return _headers;
            }
        }
    }
}
