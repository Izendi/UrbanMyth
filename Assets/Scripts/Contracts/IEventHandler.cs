using Assets.Scripts.Events;

namespace Assets.Scripts.Contracts
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}