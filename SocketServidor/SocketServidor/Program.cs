using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;

namespace SocketServidor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 9000);
            Socket servidor = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                servidor.Bind(endPoint);
                servidor.Listen(10);

                while (true)
                {
                    Console.WriteLine("À espera de ligaçôes...");
                    Socket socket = servidor.Accept();

                    Thread t1 = new Thread(EscutaCliente);
                    t1.Start(socket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                servidor.Close();
            }
        }

        static void EscutaCliente(object socket)
        {
            Socket s = (Socket)socket;
            while (true)
            {
                byte[] bytes = new byte[1024];
                int bytesLenght = s.Receive(bytes);
                Console.WriteLine("Mensagem: " + Encoding.UTF8.GetString(bytes, 0, bytesLenght));

                string dataHoraAtual = DateTime.Now.ToString();
                byte[] dataHoraBytes = Encoding.UTF8.GetBytes(dataHoraAtual);
                s.Send(dataHoraBytes);

                string ipEndereco = ((IPEndPoint)s.LocalEndPoint).Address.ToString();
                byte[] ipBytes = Encoding.UTF8.GetBytes(ipEndereco);
                s.Send(ipBytes);

                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                string macEndereco = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))
                    {
                        macEndereco = adapter.GetPhysicalAddress().ToString();
                        break;
                    }
                }

                byte[] macBytes = Encoding.UTF8.GetBytes(macEndereco);
                s.Send(macBytes);
            }
        }
    }
}
