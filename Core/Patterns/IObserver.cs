namespace AdmissionSystem.Core.Patterns
{
    public interface IObserver
    {
        void Update(IRoomSubject subject);
    }
}
