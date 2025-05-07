using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class AdmissionSystem
    {
        private static readonly Lazy<AdmissionSystem> _instance = new(() => new AdmissionSystem());
        public static AdmissionSystem GetInstance() => _instance.Value;

        private readonly List<AdmissionCenter> _centers = new();
        private int _nextCenterId = 1;

        public IReadOnlyList<AdmissionCenter> Centers => _centers;

        private AdmissionSystem() { }

        public AdmissionCenter RegisterCenter(AdmissionCenter center)
        {
            center.CenterId = _nextCenterId++;
            _centers.Add(center);
            return center;
        }

        public AdmissionCenter? GetCenterById(int id) => _centers.FirstOrDefault(c => c.CenterId == id);
    }
}