namespace Infrastructure.WebSockets.EventsArgs
{
    public class MessageEventArgs : EventArgs
    {
        public byte[] RawData;
        public MessageEventArgs(byte[] rawData)
        {
            RawData = rawData;
        }
    }
}
