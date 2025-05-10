namespace AdmissionSystem.Core.Models
{
    public class Student
    {
        public string Name { get; set; } = string.Empty;
        public int Id { get; set; }


        public int Rank { get; }
        public List<Department> Choices { get; }
        public Department AcceptedDepartment { get; set; }
        public bool IsMigrationEnabled { get; private set; } = true;
        public AdmissionStatus Status { get; private set; } = AdmissionStatus.Pending;


        public Student(int id, string name, int rank, List<Department> choices)
        {
            Id = id;
            Name = name;
            Rank = rank;
            Choices = choices ?? throw new ArgumentNullException(nameof(choices));
            AcceptedDepartment = null;
        }

        public void AcceptOffer()
        {
            if (AcceptedDepartment == null)
                throw new InvalidOperationException("No offer to accept");

            Status = AdmissionStatus.Accepted;
        }

        public void DeclineOffer()
        {
            if (AcceptedDepartment == null)
                throw new InvalidOperationException("No offer to decline");

            AcceptedDepartment.RemoveStudent(this);
            Status = AdmissionStatus.Declined;
            AcceptedDepartment = null;
        }

        public void DisableMigration()
        {
            if (Status != AdmissionStatus.Accepted)
                throw new InvalidOperationException("Only accepted students can disable migration");

            IsMigrationEnabled = false;
        }

        internal void SetTentativeOffer(Department department)
        {
            if (department == null) throw new ArgumentNullException();

            // Allow both pending and migration-eligible students
            if (Status == AdmissionStatus.Declined)
                Status = AdmissionStatus.Pending; // reset status so they can be retried

            if (Status != AdmissionStatus.Pending)
                throw new InvalidOperationException("Only pending students can receive offers");

            AcceptedDepartment = department;
        }



    }

    public enum AdmissionStatus
    {
        Pending,
        TentativelyAccepted,
        Accepted,
        Declined
    }
}
