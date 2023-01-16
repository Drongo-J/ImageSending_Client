using ImageSending_Client.Commands;
using ImageSending_Client.Helpers;
using ImageSending_Client.Models;
using ImageSending_Client.Services.NetworkServices;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                 string ip;
                 try
                 {
                     ip = NetworkHelpers.GetLocalIpAddress();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                     return;
                 }
                 var ipAdress = IPAddress.Parse(ip);
                 var port = Constants.Port;
                 var ep = new IPEndPoint(ipAdress, port);

                 if (ButtonText == Constants.ConnectServer) // not connected
                 {
                     try
                     {
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

                             var imageMessage = new ImageMessage()
                             {
                                 ImageBytes = ImageHelper.ConvertImageSourceToBytes(ImageSource as BitmapSource),
                                 Title = this.Title
                             };

                             var jsonStr = JsonConvert.SerializeObject(imageMessage, Formatting.Indented);
                             var bytes = Encoding.ASCII.GetBytes(jsonStr);
                             socket.Send(bytes);

                             MessageBox.Show("Message was sent");
                         }
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message);
                     }
                 }
             });
        }
    }
}
