using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Listener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunListener();
        }
        static void RunListener()
        {
            // Establish the remote endpoint for the socket.
            // host: localhost
            // port: 12345

            // declaring a constant of the port that will be used in the communication.
            const int PORT = 12345;

            // Gets the hostname of our local address then resolves it to IPHostEntry type.
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            // Gets the ip adress from the host entry and storing it as IPAdress type.
            IPAddress ip = host.AddressList[0];
            // Creating a new EndPoint for our communication using the IP and PORT that we will communicate with.
            IPEndPoint localEndPoint = new IPEndPoint(ip, PORT);

            // Creating a new socket that will be used to send messages from the client to the listener.
            Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // this basically means that we created a socket of IP family IPV4 (127.0.0.1 in our case) and that will Stream the data via the TCP protocol.

            try
            {
                // Bind the socket with the local endpoint (localhost, 12345)
                listener.Connect(localEndPoint);

                // Make the socket listening with max capability of 10 requests at time.
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("[*] listening for incoming connections...");

                    // Create a new socket for a connection.
                    Socket clientSocket = listener.Accept();

                    // Data buffer.
                    string data = null;
                    byte[] bytes = new byte[1024];

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    Console.WriteLine("[*] Message received: {0} [{1}] characters.", data, data.Length);

                    // Creating a message to send from the server to the client.
                    byte[] message = Encoding.ASCII.GetBytes("[server] message received succssefuly!");
                    clientSocket.Send(message);

                    // cleaning up.
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}