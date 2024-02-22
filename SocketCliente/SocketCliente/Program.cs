using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace SocketCliente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // vamos obter o host e o ip da máquina e especificamos um ponto de ligação
            IPAddress ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 9000);

            // vamos criar o socket para o cliente
            Socket cliente = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                cliente.Connect(endPoint);

                // Imprimir a data e hora atual
                //Console.WriteLine("Data e Hora Atual: " + DateTime.Now);

                // Imprimir o endereço IP do cliente
                //Console.WriteLine("Endereço IP do Cliente: " + ipAddress.ToString());

                // Obter e imprimir o endereço MAC do cliente
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String macAddress = String.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))
                    {
                        macAddress = adapter.GetPhysicalAddress().ToString();
                        break;
                    }
                }
                //Console.WriteLine("Endereço MAC do Cliente: " + macAddress); 

                while (true)
                {
                    Console.WriteLine("Digite Mensagem: ");
                    string data = Console.ReadLine();
                    // vamos enviar em bytes os dados da nossa string data
                    cliente.Send(Encoding.UTF8.GetBytes(data));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cliente.Close();
            }
        }
    }
}
