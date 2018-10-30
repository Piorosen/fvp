using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class NetClient
{
    const int ReadBufferCapacity = 1024 * 8 * 2;
    const int SendBufferCapacity = 1024 * 8;

    TcpClient tcpClient;
    NetworkStream stream;
    Queue<byte[]> sendQueue = new Queue<byte[]>();
    int sendQueueReadIndex = 0;
    byte[] readBuffer = new byte[ReadBufferCapacity];
    int readBufferLength = 0;
    byte[] sendBuffer = new byte[SendBufferCapacity];
    IPEndPoint endpoint;

    public NetClient(string addr, int port)
    {
        endpoint = new IPEndPoint(IPAddress.Parse(addr), port);
    }

    public void Connect()
    {
        tcpClient = new TcpClient();
        tcpClient.Connect(endpoint);
        stream = tcpClient.GetStream();
        readBufferLength = 0;
        sendQueue.Clear();
        sendQueueReadIndex = 0;
    }

    public void Close()
    {
        if (tcpClient != null)
        {
            stream.Close();
            tcpClient.Close();
            stream = null;
            tcpClient = null;
        }
        sendQueue.Clear();
    }

    public void Send(byte[] buffer)
    {
        sendQueue.Enqueue(buffer);
    }

    public void Send(byte[] buffer, int offset, int size)
    {
        byte[] buf = new byte[size];
        Array.Copy(buffer, buf, size);
        sendQueue.Enqueue(buf);
    }

    bool TryGetPacket()
    {
        return false;
    }

    public void Update()
    {
        if (stream == null)
        {
            return;
        }

        if (stream.CanRead)
        {
            if (stream.DataAvailable)
            {
                int readBufferRemainingLength = readBuffer.Length - readBufferLength;
                if (0 < readBufferRemainingLength)
                {
                    int bytesTransferred = stream.Read(readBuffer, readBufferLength, readBufferRemainingLength);
                    readBufferLength += bytesTransferred;
                }
            }
        }

        if (stream.CanWrite)
        {
            int sendBufferValidLength = 0;
            while (sendQueue.Count != 0)
            {
                byte[] first = sendQueue.First();
                int firstRemainingLength = first.Length - sendQueueReadIndex;
                int sendBufferRemainingLength = sendBuffer.Length - sendBufferValidLength;

                if (firstRemainingLength < sendBufferRemainingLength)
                {
                    Array.Copy(first, sendQueueReadIndex, sendBuffer, sendBufferValidLength, firstRemainingLength);
                    sendBufferValidLength += firstRemainingLength;
                    sendQueue.Dequeue();
                    sendQueueReadIndex = 0;
                }
                else
                {
                    Array.Copy(first, sendQueueReadIndex, sendBuffer, sendBufferValidLength, firstRemainingLength);
                    sendBufferValidLength += firstRemainingLength;
                    sendQueueReadIndex = firstRemainingLength - sendBufferValidLength;
                    break;
                }
            }

            if (0 < sendBufferValidLength)
            {
                stream.Write(sendBuffer, 0, sendBufferValidLength);
            }
        }
    }
}