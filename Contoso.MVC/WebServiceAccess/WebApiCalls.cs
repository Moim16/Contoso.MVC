using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;/*Para el uso del servicio*/
using System.Net.Http.Headers;/*Para manuipular encabezados de la solicitud, acceder a encabezado y asignar a una variable*/
using Newtonsoft.Json;/*Permite hacer serializaciones y deseralizaciones*/
using Contoso.MVC.Configuration;
using Contoso.Models.Entities;
using Contoso.MVC.WebServiceAccess.Base;
using Contoso.MVC.Models;

namespace Contoso.MVC.WebServiceAccess
{
    /*Hereda de WebApiCallsBase e implementa interfaz IWebApiCalls*/
    /* Mi papa es WebApiCallsBase*/
    public class WebApiCalls : WebApiCallsBase, IWebApCalls
    {

        /* Definir constructor para poder inyectar iwebservicelocator*/
        /*No podes heredar mas de una clase, esto solo en java*/
        /*Si se puede implementar mas de una interfaz*/
        public WebApiCalls(IWebServiceLocator ContosoService) : base(ContosoService) /*base manda ejecutar el constructor de mi papa*/
        {
            /* a mi papa le voy a pasar lo que estoy implementando*/
        }

        public HttpResponseHeaders Headers
        {
            get
            {
                return HeadersBase; /*Es una propiedad de clase de mi papa que yo heredo*/
            }
        }

        /*Debe Colocarse el Async para que funcione, donde uso await hay que usar async*/
        public async Task<string> AddStudent(Student student)
        {
            /* Lo serializamos a json*/
            var json = JsonConvert.SerializeObject(student);
            /*con esto return await mandamos a ejecutar el metodo de webapicallsbase*/
            return await SubmitPostRequestAsync(StudentBaseUri, json);
        }

        public async Task<List<DatosAbogadoDto>> datosAbogadoDto(int idabogado)
        {
            return await GetItemAsync<DatosAbogadoDto>(AbogadoBaseUri + "/" + idabogado);
        }

        public async Task DeleteStudent(int id)
        {
            await SubmitDeleteRequestAsync(StudentBaseUri + "/" + id);
        }

        public async Task<List<Student>> GetStudent(int id)
        {
            return await GetItemAsync<Student>(StudentBaseUri + "/" + id);
        }

        public async Task<IList<Student>> GetStudents()
        {
            return await GetItemListAsync<Student>(StudentBaseUri);
        }

        public async Task<string> UpdateStudent(Student student)
        {
            var json = JsonConvert.SerializeObject(student);
            return await SubmitPutRequestAsync(StudentBaseUri + "/" + student.Id, json);
        }
    }
}
    


