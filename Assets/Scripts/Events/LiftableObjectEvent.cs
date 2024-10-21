namespace Assets.Scripts.Events
{
    public class LiftableObjectEvent : IEvent
    {
        // This event is published when the player does some action on a liftable object.
        // We use this event to determine if the player is lifted or dropped the object. 
        // The default state is lifted.
        public bool ObjectLifted { get; set; } = true;
        public bool ObjectDropped => !ObjectLifted;

    }
}