using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServidor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // vamos obter o host e o ip da máquina
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            // especificamos um ponto de ligacao com ip e porta
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 9000);

            // vamos criar um socket
            Socket servidor = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                servidor.Bind(endPoint);
                servidor.Listen(10); // vai aceitar até 10 ligações simultaneas

                while(true)
                {
                    // quando recebe uma ligação cria um socket de ligação ao cliente
                    // para continuar à escuta de novas ligações vamos usar um thread
                    Console.WriteLine("À espera de ligaçôes...");
                    Socket socket = servidor.Accept();

                    Thread t1 = new Thread(new Program().EscutaCliente);
                    t1.Start(socket);
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            } finally
            {
                servidor.Close();
            }
        }

        void EscutaCliente(object socket)
        {
            Socket s = (Socket)socket;
            while (true)
            {
                byte[] bytes = new byte[1024];
                int bytesLenght = s.Receive(bytes);
                Console.WriteLine("Mensagem: " + Encoding.UTF8.GetString(bytes, 0, bytesLenght));
            }
        }
    }
}
