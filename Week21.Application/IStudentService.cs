using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week21.Domain;

namespace Week21.Application
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudentById(Guid id);
        Task<IEnumerable<Student>> GetAllStudent();
        Task<Student> AddStudent(Student student);
        Task<Student> UpdateStudent(Student student);
        Task DeleteStudent(Guid id);
    }
}
