using System;

namespace StopLossKata.Worker.Bus
{
    public interface IBus
    {
        void Send<T>(T message);
        
        void Publish<T>(T message);

        void Defer<T>(TimeSpan delay, T state);
    }
}