using System;

namespace AdmissionSystem.Core.Patterns
{
    public interface IRoomSubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }
}
