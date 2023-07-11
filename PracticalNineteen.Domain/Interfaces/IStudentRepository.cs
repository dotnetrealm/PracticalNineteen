using PracticalNineteen.Domain.DTO;

namespace PracticalNineteen.Domain.Interfaces
{
    public interface IStudentRepository
    {
        /// <summary>
        /// Return all students
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<StudentModel>> GetAllStudentsAsync();

        /// <summary>
        /// Return student by Id
        /// </summary>
        /// <param name="studentId">Id of student</param>
        /// <returns></returns>
        Task<StudentModel> GetStudentByIdAsync(int studentId);

        /// <summary>
        /// Insert new student data
        /// </summary>
        /// <param name="student">Student type object</param>
        /// <returns>newly inserted StudentId</returns>
        Task<int> InsertStudentAsync(StudentModel student);

        /// <summary>
        /// Update existing details of student
        /// </summary>
        /// <param name="student">Student type object</param>
        /// <returns>Updated student object</returns>
        Task<bool> UpdateStudentAsync(int studentId, StudentModel student);

        /// <summary>
        /// Delete student record
        /// </summary>
        /// <param name="studentId">StudentId</param>
        /// <returns>assertion of student delete</returns>
        Task<bool> DeleteStudentAsync(int studentId);

    }
}
