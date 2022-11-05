namespace GeneralResources.XUnitExample.RaiseEventOrThrowException
{
    public class Message
    {
        public event EventHandler<MessageEventArgs> SendMessageEvent;

        public void OnSendMessageHandler(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void SendMessageToUser(string message)
        {
            SendMessageEvent += OnSendMessageHandler;
            SendMessageEvent.Invoke(this, new MessageEventArgs { Message = message });
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
