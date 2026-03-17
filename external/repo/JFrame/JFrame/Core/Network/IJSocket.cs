using System;
using System.Threading;

namespace JFramework
{
    public interface IJSocket :  ICloneable
    {
        event Action<IJSocket> onOpen;
        event Action<IJSocket, SocketStatusCodes, string> onClosed;
        event Action<IJSocket, string> onError;
        event Action<IJSocket, byte[]> onBinary;
        //event Action<IJFrameworkSocket, string> onMessage;
        bool IsOpen { get; }
        void Init(string url, string token = null);
        void Open();
        void Close();

        void Send(byte[] data);
    }


    public interface IJSocketListener  :  ICloneable
    {
        event Action<IJSocketListener> onListening;
        event Action<IJSocketListener, SocketStatusCodes, string> onClosed;
        event Action<IJSocketListener, string> onError;
        event Action<IJSocketListener, string, byte[]> onBinary;

        void StartListening(ushort port, CancellationToken stoppingToken);
        void StopListening();

        void Send(byte[] data);
        bool Send(string clientId, byte[] data);
    }
}
