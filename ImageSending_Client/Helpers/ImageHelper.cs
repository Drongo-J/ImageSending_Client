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
using System.Windows.Interop;
using System.Drawing.Imaging;

namespace ImageSending_Client.Helpers
{
    public static class ImageHelper
    {
        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        /// <summary>
        /// Convert String to ImageSource
        /// </summary>
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
        /// Convert ImageSource to bytes
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static byte[] ConvertImageSourceToBytes(BitmapSource imageSource)
        {
            byte[] bytes = null;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                bytes = stream.ToArray();
            }

            return bytes;
        }

        /// <summary>
        /// Convert bytes to ImageSource
        /// </summary>
        public static ImageSource ConvertBytesToImageSource(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Image returnImage = Image.FromStream(ms);
            using (var mst = new MemoryStream())
            {
                returnImage.Save(mst, ImageFormat.Bmp);
                mst.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = mst;
                bitmapImage.EndInit();
                return ImageSourceFromBitmap(BitmapImage2Bitmap(bitmapImage));
            }
        }

        /// <summary>
        /// Get ImageSource from Bitmap
        /// </summary>
        public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        /// <summary>
        /// Convert BitmapImage to Bitmap
        /// </summary>
        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        /// <summary>
        /// Get Bitmap Source from Image
        /// </summary>
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
        public static byte[] ImageToBytes(System.Drawing.Image imageIn, System.Drawing.Imaging.ImageFormat imageFormat)
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
        public static Image BytesToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
