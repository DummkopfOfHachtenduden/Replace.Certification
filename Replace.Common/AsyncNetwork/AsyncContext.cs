using System;

namespace Replace.Common.AsyncNetwork
{
    public class AsyncContext
    {
        public AsyncState State { get; set; }
        public Guid Guid { get { return guid; } }
        public IAsyncInterface Interface { get; set; }
        public object User { get; set; }

        private Guid guid;

        public AsyncContext()
        {
            guid = Guid.NewGuid();
        }

        public void Disconnect()
        {
            State.Disconnect();
        }

        public void Send(byte[] buffer)
        {
            State.Write(new AsyncBuffer(buffer));
        }

        public void Send(byte[] buffer, int offset, int count)
        {
            State.Write(new AsyncBuffer(buffer, offset, count));
        }
    }
}