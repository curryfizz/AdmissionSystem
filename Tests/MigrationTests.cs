using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using Xunit;

namespace AdmissionSystem.Tests
{
    public class MigrationTests
    {
        [Fact]
        public void Should_Assign_Top_Ranked_Student_First()
        {
            var d1 = new Department("CSE", 1); // only one seat
            var student1 = new Student(1, "Alice", 2, new List<Department> { d1 });
            var student2 = new Student(2, "Bob", 1, new List<Department> { d1 });

            var system = new MigrationSystem(2, new RankFirstStrategy(), new InitialAssignmentStrategy());
            system.Students.AddRange(new[] { student1, student2 });
            system.Departments.AddRange(new[] { d1 });
            system.Initialize();

            Assert.Equal("CSE", student2.AcceptedDepartment?.Name);
            Assert.Null(student1.AcceptedDepartment);
        }

        [Fact]
        public void Should_Not_Allow_Decline_Without_Offer()
        {
            var student = new Student(1, "Zoe", 1, new List<Department>());
            Assert.Throws<InvalidOperationException>(() => student.DeclineOffer());
        }

        [Fact]
        public void Should_Not_Allow_Migration_Disable_If_Not_Accepted()
        {
            var d1 = new Department("CSE", 1);
            var student = new Student(1, "Dan", 1, new List<Department> { d1 });

            Assert.Throws<InvalidOperationException>(() => student.DisableMigration());
        }

        [Fact]
        public void Should_Not_Allow_Accept_Without_Tentative_Offer()
        {
            var student = new Student(1, "Emma", 1, new List<Department>());
            Assert.Throws<InvalidOperationException>(() => student.AcceptOffer());
        }

        [Fact]
        public void Should_Migrate_To_Better_Choice_When_Available()
        {
            var d1 = new Department("CSE", 1);
            var d2 = new Department("EEE", 1);

            var student = new Student(1, "Rick", 1, new List<Department> { d1, d2 });

            d1.AddStudent(student); // Fill up CSE
            var system = new MigrationSystem(2, new RankFirstStrategy(), new InitialAssignmentStrategy());
            system.Students.Add(student);
            system.Departments.AddRange(new[] { d1, d2 });

            system.Initialize();
            Assert.Equal("EEE", student.AcceptedDepartment?.Name);

            d1.Students.Clear(); // Free up a seat in CSE
            system.RunNextCall();

            Assert.Equal("CSE", student.AcceptedDepartment?.Name);
        }

        [Fact]
        public void Should_Not_Migrate_If_Student_Finalized()
        {
            var d1 = new Department("CSE", 1);
            var d2 = new Department("EEE", 1);

            var student = new Student(1, "Leo", 1, new List<Department> { d1, d2 });
            var system = new MigrationSystem(2, new RankFirstStrategy(), new InitialAssignmentStrategy());
            system.Students.Add(student);
            system.Departments.AddRange(new[] { d1, d2 });

            system.Initialize();
            system.FinalizeStudent(student.Id);  
            system.RunNextCall();  

            Assert.Equal("CSE", student.AcceptedDepartment?.Name);  
        }
    }
}
