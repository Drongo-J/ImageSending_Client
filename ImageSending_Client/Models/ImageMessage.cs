using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageSending_Client.Models
{
    [Serializable]
    public class ImageMessage
    {
        public byte[] ImageBytes { get; set; }
        public string Title { get; set; }
    }
}
