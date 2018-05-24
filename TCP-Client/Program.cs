namespace TCP_Client
{
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

    /// <summary>
    /// This class handles communication - unfinished
    /// </summary>
    public class Program
    {
        private static TcpClient client = new TcpClient();
        private static void Initialize()
        {
            Console.Write("Enter the string to be transmitted: ");
            string msg = Console.ReadLine();
            SendMessage(client, msg);
        }
        private static void ContinueSession()
        {
            switch(Console.ReadLine().ToUpper())
            {
                case "Y":
                    Initialize();
                    break;                    
                case "N":
                    client.Close();
                    Console.Write("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default:
                    ContinueSession();
                    break;
            }
        }
        private static void PrintMessage(TcpClient client)
        {
            while(true)
            {
                string msg = RecieveMessage(client);
                Console.WriteLine("Recieved message: " + msg);
            }
        }
        private static void SendMessage(TcpClient client, string msg)
        {
            NetworkStream netStream = client.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] msgBytes = asen.GetBytes(msg);
            netStream.Write(msgBytes, 0, msgBytes.Length);
        }

        private static string RecieveMessage(TcpClient client)
        {
            NetworkStream netStream = client.GetStream();
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] msgBytes = new byte[2048];
            int size = netStream.Read(msgBytes, 0, msgBytes.Length);
            byte[] fixedBytes = new byte[size];
            for(int i = 0; i < size; i++)
            {
                fixedBytes[i] = msgBytes[i];
            }
            return ascii.GetString(fixedBytes);
        }

        private static void Main()
        {
            try
            {
                Console.Write("Adress: ");
                client.Connect(Console.ReadLine(), 8001);
                Console.WriteLine("Conncetd");
                Initialize();
                Console.WriteLine("Message sent");
                Thread recieve = new Thread(() => PrintMessage(client));
                recieve.Start();
                Console.WriteLine("Would you like to continue the session?");
                ContinueSession();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace);
                Console.Write("Press any key to close.");
                Console.ReadKey();
            }
        }
    }
}
