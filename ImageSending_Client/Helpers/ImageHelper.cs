using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace ImageSending_Client.Helpers
{
    public static class ImageHelper
    {
        public static System.Windows.Media.ImageSource StringToImageSource(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(source, UriKind.RelativeOrAbsolute);
            bi3.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            bi3.EndInit();
            return bi3;
        }

        /// <summary>
        /// ImageSource to bytes
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                using (var stream = new MemoryStream())
                {
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        public static BitmapSource GetImageStream(Image myImage)
        {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        /// <summary>
        /// Convert String to ImageFormat
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat ImageFormatFromString(string format)
        {
            if (format.Equals("Jpg"))
                format = "Jpeg";
            Type type = typeof(System.Drawing.Imaging.ImageFormat);
            BindingFlags flags = BindingFlags.GetProperty;
            object o = type.InvokeMember(format, flags, null, type, null);
            return (System.Drawing.Imaging.ImageFormat)o;
        }

        /// <summary>
        /// Read image from path
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public static byte[] BytesFromImage(String imageFile, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, imageFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// Convert image to byte array
        /// </summary>
        /// <param name="imageIn"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// Byte array to photo
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
