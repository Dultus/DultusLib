namespace Discord.Attribute
{
    using Controller;
    /// <summary>
    /// To be placed over dynamic methods that will be executed by the <see cref="EventController"></see>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EventControllerAttribute<T> : System.Attribute
    {
        public T Event { get; set; }
        public EventControllerAttribute(T Event)
        {
            this.Event = Event;
        }
    }
}