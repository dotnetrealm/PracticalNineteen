using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Data.Contexts;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Interfaces;
using PracticalNineteen.Domain.Models;

namespace PracticalNineteen.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public StudentRepository(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentModel>> GetAllStudentsAsync()
        {
            var data = await _db.Students.ToListAsync();
            var students = _mapper.Map<IEnumerable<StudentModel>>(data);
            return students;
        }

        public async Task<StudentModel> GetStudentByIdAsync(int studentId)
        {
            Student? data = await _db.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            StudentModel student = _mapper.Map<StudentModel>(data);
            return student!;
        }

        public async Task<int> InsertStudentAsync(StudentModel student)
        {
            try
            {
                var data = _mapper.Map<Student>(student);
                data.Id = 0;
                await _db.Students.AddAsync(data);
                await _db.SaveChangesAsync();
                int id = await _db.Students.MaxAsync(s => s.Id);
                return id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> UpdateStudentAsync(int studentId, StudentModel student)
        {
            var data = await _db.Students.FirstOrDefaultAsync(s => s.Id == studentId);

            if (data == null) return false;

            data.FirstName = student.FirstName;
            data.MiddleName = student.MiddleName;
            data.LastName = student.LastName;
            data.MobileNumber = student.MobileNumber;
            data.Address = student.Address;
            data.DOB = student.DOB;
            data.Gender = student.Gender; 
            _db.Students.Update(data);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var data = await _db.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (data == null) return false;
            _db.Remove(data);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
