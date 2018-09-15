﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClientChart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Socket clientSocket = null;
        public delegate void UpdateTextCallback(string message);
        private static byte[] _buffer = new byte[1024];
        private static MainWindow mainwin;
        PointCollection pcoll = new PointCollection();
        int[] temp = new int[500];
        Polyline pl = new Polyline();
        public MainWindow()
        {
            InitializeComponent();
            mainwin = this;
           
            


            for (int i = 0; i < pcoll.Count; i++)
            {
                Point p = new Point(0, 250);
                pcoll.Add(p);
            }

            draw();
        }


        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (clientSocket != null)  //evtl vorhandenen Socket schließen
            {
                clientSocket.Close();
                clientSocket.Dispose();
                clientSocket = null;
            }
            //und neuen aufmachen
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(ServerIP.Text, Convert.ToInt16(portnr.Text));
                Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { /*UpdateText("mit dem Server verbunden");*/ });
                clientSocket.Send(Encoding.ASCII.GetBytes(" " + nickname.Text));
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
            }
            catch
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { UpdateText("Server nicht verfügbar"); });
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            try
            {
                int received = socket.EndReceive(AR);
                byte[] dataBuffer = new byte[received];
                Array.Copy(_buffer, dataBuffer, received);
                string text = Encoding.ASCII.GetString(dataBuffer);
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { UpdateText(text); });
                //auf die nächsten Zeichen warten
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch
            {
                socket.Close();
                socket.Dispose();
            }
        }

        private void UpdateText(string s)
        {
            Chat.Text += s + "\n";

            int currTemp = int.Parse(s);
            mycanvas.Children.Clear();
            for (int i = 0; i < 499; i++)
            {
                Point po = pcoll[i + 1];
                po.X = i;
                pcoll[i] = po;
            }
            Point p = new Point();
            p.X = 499;
            p.Y =  currTemp;
            pcoll[499] = p;

            pl.Stroke = Brushes.Black;
            pl.StrokeThickness = 0.5;

            pl.Points = pcoll;
            mycanvas.Children.Add(pl);
        }

        private void btnSenden_Click(object sender, RoutedEventArgs e)
        {
            if (SendeDaten.Text != "")
            {
                string s = nickname.Text + ": " + SendeDaten.Text;
                try
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(s);
                    Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { clientSocket.Send(buffer); });
                    Chat.Text += s + "\n";
                    SendeDaten.Clear();
                }
                catch
                {
                    SendeDaten.Clear();
                    Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { UpdateText("Sendefehler - ist eine Verbindung zum Server vorhanden?\n"); });
                }
            }
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("~");
            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Send(buffer);
                    clientSocket.Close();
                    clientSocket.Dispose();
                    clientSocket = null;
                }
            }
            catch
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { UpdateText("Abmeldung fehlgeschlagen - ist eine Verbindung zum Server vorhanden?\n"); });
            }
        }

        private void draw()
        {
            pl.Stroke = Brushes.White;
            pl.StrokeThickness = 0.5;
            for (int i = 0; i < 500; i++)
            {
                Point p = new Point();
                p.X = i;
                p.Y = temp[i];
                pcoll.Add(p);
            }
            pl.Points = pcoll;
            mycanvas.Children.Add(pl);
        }

        
       
    }
}