using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Google.Protobuf;

public struct PacketInfo
{
    public Packet.Type Type;
    public byte[] Payload;
}

public class NetClient
{
    const int ReadBufferCapacity = 1024 * 8 * 2;
    const int SendBufferCapacity = 1024 * 8;
    const int HeaderSize = 4;

    TcpClient tcpClient;
    NetworkStream stream;
    Queue<byte[]> sendQueue = new Queue<byte[]>();
    int sendQueueReadIndex = 0;
    byte[] readBuffer = new byte[ReadBufferCapacity];
    int readBufferLength = 0;
    byte[] sendBuffer = new byte[SendBufferCapacity];
    IPEndPoint endpoint;
    Queue<PacketInfo> readQueue = new Queue<PacketInfo>();

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
        readQueue.Clear();
        sendQueueReadIndex = 0;
    }

    public void Close()
    {
        if(tcpClient != null)
        {
            if(stream != null)
            {
                stream.Close();
            }
            tcpClient.Close();
            stream = null;
            tcpClient = null;
        }
        sendQueue.Clear();
        readQueue.Clear();
    }

    public void Send(Packet.Type type, IMessage message)
    {
        short messageSize = (short)message.CalculateSize();

        byte[] buffer = new byte[HeaderSize + messageSize];

        MemoryStream ms = new MemoryStream(buffer);
        BinaryWriter binaryStream = new BinaryWriter(ms);
        binaryStream.Write(messageSize);
        binaryStream.Write((short)type);
        message.WriteTo(ms);

        Send(buffer, 0, (int)ms.Position);
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

    public bool TryGetPacket(out PacketInfo packet)
    {
        if(readQueue.Count != 0)
        {
            var info = readQueue.Dequeue();
            packet = info;
            return true;
        }
        packet = new PacketInfo() { Payload = null, Type = 0 };
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

                    int headerSize = 4;
                    int readIndex = 0;
                    while (headerSize <= readBufferLength)
                    {
                        short payloadSize = BitConverter.ToInt16(readBuffer, readIndex);
                        short type = BitConverter.ToInt16(readBuffer, readIndex + sizeof(short));
                        int packetSize = headerSize + payloadSize;
                        if (packetSize <= readBufferLength)
                        {
                            byte[] payload = new byte[payloadSize];
                            Array.Copy(readBuffer, readIndex + headerSize, payload, 0, payloadSize);
                            readQueue.Enqueue(new PacketInfo() { Payload = payload, Type = (Packet.Type)type });
                            readBufferLength -= packetSize;
                            readIndex += packetSize;
                        }
                        else
                        {
                            Array.ConstrainedCopy(readBuffer, readIndex, readBuffer, 0, readBufferLength);
                            break;
                        }
                    }
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

            if(0 < sendBufferValidLength)
            {
                stream.Write(sendBuffer, 0, sendBufferValidLength);
            }
        }
    }
}