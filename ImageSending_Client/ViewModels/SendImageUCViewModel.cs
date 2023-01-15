using ImageSending_Client.Commands;
using ImageSending_Client.Helpers;
using ImageSending_Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace ImageSending_Client.ViewModels
{
    public class SendImageUCViewModel : BaseViewModel
    {
        public RelayCommand ButtonCommand { get; set; }

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged(); }
        }

        private string title = String.Empty;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        private string buttonText;

        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; OnPropertyChanged(); }
        }

        private delegate void Func();

        public SendImageUCViewModel()
        {
            ButtonText = Constants.ConnectServer;

            ButtonCommand = new RelayCommand(async (b) =>
             {
                 var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                 var ipAdress = IPAddress.Parse(App.IPAdress);
                 var port = Constants.Port;
                 var ep = new IPEndPoint(ipAdress, port);

                 if (ButtonText == Constants.ConnectServer) // not connected
                 {
                     try
                     {
                         //string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                         //string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString(); // Get the IP
                         using (socket)
                         {
                             await socket.ConnectAsync(ep);

                             if (socket.Connected)
                             {
                                 MessageBox.Show("Connected To Server");
                                 Func del = delegate
                                 {
                                     ButtonText = Constants.SendMessage;
                                 };
                                 del.Invoke();
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message);
                     }
                 }
                 else
                 {
                     if (Title.Trim() == String.Empty)
                     {
                         MessageBox.Show("Enter title for the image");
                         return;
                     }

                     try
                     {
                         using (socket)
                         {

                             if (!socket.Connected)
                             {
                                 await socket.ConnectAsync(ep);
                             }

                             //var imageMessage = new ImageMessage()
                             //{
                             //    ImageBytes = ImageHelper.ImageSourceToBytes(ImageSource),
                             //    Title = this.Title
                             //};

                             //var str = JsonHelpers.Serialize(imageMessage);
                             //Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str.ToCharArray());
                             //await socket.ConnectAsync(ep);

                             //var bytes = ByteHelper.ToByteArray(imageMessage);
                             //socket.Send(bytes);
                             //var bytes = Encoding.UTF8.GetBytes();




                             //Stream stream = File.Open("data.xml", FileMode.Create);
                             //SoapFormatter formatter= new SoapFormatter();
                             //formatter.Serialize(stream, imageMessage);

                             //Byte[] bytesSent = ByteHelper.ReadToEnd(stream);
                             //stream.Close();
                             //SocketAsyncEventArgs writeEventArgs = new SocketAsyncEventArgs();
                             //writeEventArgs.SetBuffer(bytesSent, 0, bytesSent.Length);
                             //socket.SendAsync(writeEventArgs);

                             var message = "Hello there";
                             var bytes = Encoding.UTF8.GetBytes(message);
                             socket.Send(bytes);

                             MessageBox.Show("Message was sent");

                             //else
                             //{
                             //    MessageBox.Show("You are not connected to the server");
                             //}
                         }
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message);
                     }
                 }
             });
        }

        public void SendMessage(string text)
        {
           
        }
    }
}
