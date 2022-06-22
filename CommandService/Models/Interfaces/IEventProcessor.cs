namespace CommandService.Models.Interfaces{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}