using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week21.Application;
using Week21.Domain;
using Week21.Infrastructure.Data;

namespace Week21.Infrastructure
{
    public class SudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        public SudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> AddStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteStudent(Guid id)
        {
            var result = await _context.Students.FindAsync(id);
            if (result != null)
            {
                _context.Students.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudent()
        {
            var result = await _context.Students.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Student>> GetStudentById(Guid id)
        {
            return await _context.Students.Where(c=>c.Id == id).ToListAsync();
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }
    }
}
