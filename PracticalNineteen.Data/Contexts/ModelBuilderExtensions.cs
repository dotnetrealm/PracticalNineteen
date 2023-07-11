using Microsoft.EntityFrameworkCore;
using PracticalNineteen.Domain.Models;

namespace PracticalNineteen.Data.Context
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Seed Students table data
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void SeedStudents(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student() { Id = 1, FirstName = "Bhavin", LastName = "Kareliya", MobileNumber = "1231231231", Gender = 'M', Address = "Rajkot", DOB = Convert.ToDateTime("2002-02-09").Date },
                new Student() { Id = 2, FirstName = "Jil", LastName = "Patel", MobileNumber = "1231231231", Gender = 'M', Address = "Rajkot", DOB = Convert.ToDateTime("2001-01-01").Date },
                new Student() { Id = 3, FirstName = "Vipul", LastName = "Kumar", MobileNumber = "1231231231", Gender = 'M', Address = "Rajkot", DOB = Convert.ToDateTime("1999-07-07").Date },
                new Student() { Id = 4, FirstName = "Jay", LastName = "Gohel", MobileNumber = "1231231231", Gender = 'M', Address = "Rajkot", DOB = Convert.ToDateTime("2000-04-12").Date }
            );
        }
    }
}
