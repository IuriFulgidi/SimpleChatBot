using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Configuraxoine del serversocket
            //serve un programma che sta in ascolto su un endpoint
            IPAddress ipaddr = IPAddress.Any;
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            //collegamento tra listersocket e endpoint
            //Bind: si imposta il server ad ascoltare qualunque ip su porta 23000
            listenerSocket.Bind(ipep);

            //mettere il server in ascolto
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");

            //istruzione bloccante
            //un client si connette
            Socket client = listenerSocket.Accept();
            Console.WriteLine("CLient Connesso\n Client info: " + client.RemoteEndPoint.ToString());

            //ricevere un messaggio da parte del client
            byte[] buff = new byte[128];
            //bytes da ricevere
            int recivedBytes = 0;
            //bytes da inviare
            int sendedBytes = 0;

            while (true)
            {
                //ricezione del messaggio
                recivedBytes = client.Receive(buff);

                //traduzione dei byte in ASCII
                string mesRicevuto = Encoding.ASCII.GetString(buff, 0, recivedBytes);

                //risposte
                string risposta = "";
                if (mesRicevuto.ToUpper() == "QUIT")
                    break;
                else if (mesRicevuto.ToUpper() == "CIAO")
                    risposta = "ciao";
                else if (mesRicevuto.ToUpper() == "COME STAI")
                    risposta = "Bene e tu?";
                else if (mesRicevuto.ToUpper() == "BENE" || mesRicevuto.ToUpper() == "MALE")
                    risposta = "Ok!";
                else
                    risposta = "Non ho capito";

                //pulisco il buffer
                Array.Clear(buff, 0, buff.Length);

                //Traduzione
                buff = Encoding.ASCII.GetBytes(risposta);

                //invio del messaggio al client
                sendedBytes = client.Send(buff);

                //tutto come nuovo
                Array.Clear(buff, 0, buff.Length);
                recivedBytes = 0;
                sendedBytes = 0;
                risposta = "";
            }
            //termina il programma
            Console.WriteLine("Termina");
            Console.ReadLine();

        }
    }
}
