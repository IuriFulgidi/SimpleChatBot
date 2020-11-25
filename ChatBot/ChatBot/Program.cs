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

            //Configurazoine del serversocket
            IPAddress ipaddr = IPAddress.Any;
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            //si imposta il server ad ascoltare qualunque ip su porta 23000
            listenerSocket.Bind(ipep);

            //server in ascolto
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");

            //un client si connette
            Socket client = listenerSocket.Accept();
            Console.WriteLine("CLient Connesso\n Client info: " + client.RemoteEndPoint.ToString());

            //ricevere un messaggio da parte del client
            byte[] buff = new byte[128];
            //bytes da ricevere
            int recivedBytes = 0;
            //bytes da inviare
            int sendedBytes = 0;

            //genereatore random
            Random rnd = new Random();

            //messaggio di benvenuto
            sendedBytes = client.Send(Encoding.ASCII.GetBytes("Benvenuto!\nscirvi 'cmd' per la lista dei comandi"));
            while (true)
            {
                //ricezione del messaggio
                recivedBytes = client.Receive(buff);

                //traduzione dei byte in ASCII
                string mesRicevuto = Encoding.ASCII.GetString(buff, 0, recivedBytes);
                mesRicevuto = mesRicevuto.ToUpper().Trim();

                //risposta in base all'input

                string risposta = "";
                if (mesRicevuto == "CIAO")
                {
                    switch (rnd.Next(3))
                    {
                        case 0:
                            risposta = "Ciao";
                            break;
                        case 1:
                            risposta = "Buongiorno";
                            break;
                        case 2:
                            risposta = "Buonasera";
                            break;
                    }
                }
                else if (mesRicevuto == "COME STAI?")
                {
                    switch (rnd.Next(2))
                    {
                        case 0:
                            risposta = "Bene e tu?";
                            break;
                        case 1:
                            risposta = "Male e tu?";
                            break;
                    }
                }
                else if (mesRicevuto == "CHE FAI?")
                    risposta = "Niente";
                else if (mesRicevuto == "BENE" || mesRicevuto.ToUpper() == "MALE")
                    risposta = "Ok!";
                else if (mesRicevuto == "QUIT")
                    break;
                else if (mesRicevuto == "CMD")
                    risposta = "Posso ripondere a:\n 'Ciao', 'Come stai?' e 'Che fai?'";
                else
                    risposta = "Non ho capito";

                risposta += "\n";

                //pulisco il buffer
                Array.Clear(buff, 0, buff.Length);

                //Traduzione in ascii
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
