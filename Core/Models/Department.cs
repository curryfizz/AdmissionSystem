namespace AdmissionSystem.Core.Models
{
    public class Department
    {
        public string Name { get; }
        public int Capacity { get; }
        public List<Student> Students { get; }

        public Department(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            Students = new List<Student>();
        }

        public bool HasVacancy => Students.Count < Capacity;

        public void AddStudent(Student student)
        {
            if (!Students.Contains(student) && Students.Count < Capacity)
            {
                Students.Add(student);
            }
        }

        public void RemoveStudent(Student student)
        {
            if (!Students.Contains(student))
                throw new InvalidOperationException("Student not found.");

            Students.Remove(student);
        }
    }
}
