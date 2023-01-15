using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using ImageSending_Client.Views;
using System.Runtime.Remoting.Messaging;
using ImageSending_Client.Commands;
using Microsoft.Win32;
using ImageSending_Client.Helpers;

namespace ImageSending_Client.ViewModels
{
    public class DragDropImageUCViewModel : BaseViewModel
    {
        public RelayCommand ChooseFileCommand { get; set; }

        public DragDropImageUCViewModel()
        {
            ChooseFileCommand = new RelayCommand((c) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    var filename = ofd.FileName;
                    var imageSource = ImageHelper.StringToImageSource(filename);
                    if (imageSource != null)
                    {
                        CrateSendImageUC(imageSource);
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
            });
        }

        public void DropEvent(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) != null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var imageSource = ImageHelper.StringToImageSource(files[0]);
                if (imageSource != null)
                {
                    CrateSendImageUC(imageSource);
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void CrateSendImageUC(ImageSource imageSource)
        {
            var sendImageUC = new SendImageUC();
            var sendImageUCVM = new SendImageUCViewModel();
            sendImageUC.DataContext = sendImageUCVM;
            sendImageUCVM.ImageSource = imageSource;
            App.ChangePage(sendImageUC);
        }
    }
}
