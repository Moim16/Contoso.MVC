using Contoso.Models.Entities;
using Contoso.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.MVC.WebServiceAccess.Base
{
   public interface IWebApCalls
    {
        Task<IList<Student>> GetStudents();
        Task<List<Student>> GetStudent(int id);
        Task<string> AddStudent(Student student);
        Task<string> UpdateStudent(Student student);
        Task DeleteStudent(int id);
        Task<List<DatosAbogadoDto>> datosAbogadoDto(int idabogado);
       
    }
    
}
