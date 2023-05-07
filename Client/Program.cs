using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        // This is the client part of the program
        // The client role is sending a message to the listener.
        static void Main(string[] args)
        {
            RunClient();
        }
        static void RunClient()
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
            Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // this basically means that we created a socket of IP family IPV4 (127.0.0.1 in our case) and that will Stream the data via the TCP protocol.

            try
            {
                // Attempting to connect to the server endpoint (IP, PORT)
                sender.Connect(localEndPoint);
                Console.WriteLine("[*] connected successfully to:");
                Console.WriteLine("\t- local endpoint: {0}", sender.LocalEndPoint.ToString());
                Console.WriteLine("\t- remote endpoint: {0}", sender.RemoteEndPoint.ToString());

                // Getting the message from the user.
                Console.Write("Enter the message you want to send: ");
                string message = Console.ReadLine();
                // Convert the message string to bytes array which is the appropriate format to send in sockets.
                byte[] messageBytes = Encoding.ASCII.GetBytes(message + "<EOF>");

                // sending the message to the remote endpoint.
                int bytesSent = sender.Send(messageBytes);
                Console.WriteLine("[*] Message sent successfully to: {0} [{1}] bytes sent", sender.RemoteEndPoint.ToString(), bytesSent);

                // Get the response from the server.
                int bytesReceived = sender.Receive(messageBytes);
                Console.WriteLine("[*] Message reached the listener: {0}", Encoding.ASCII.GetString(messageBytes, 0, bytesReceived));

                // Shutdown and closing the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}