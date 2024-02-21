using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

                while(true)
                {
                    Console.WriteLine("Digite Mensagem: ");
                    string data = Console.ReadLine();
                    // vamos enviar em bytes os dados da nossa string data
                    cliente.Send(Encoding.UTF8.GetBytes(data));
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            } finally
            {
                cliente.Close();
            }
        }
    }
}
