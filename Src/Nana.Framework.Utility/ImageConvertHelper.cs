using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// TIFF图处理类
    /// </summary>
    public class ImageConvertHelper
    {
        /// <summary>
        /// 图片格式转换
        /// </summary>
        /// <param name="data">原始图像数据</param>
        /// <param name="intToFormat">目标图像格式</param>
        /// <returns></returns>
        public static byte[] Transform(byte[] data, int intToFormat)
        {
            return Transform(data, intToFormat, 100);
        }

        /// <summary>
        /// 图片格式转换
        /// </summary>
        /// <param name="data">原始图像数据</param>
        /// <param name="intToFormat">目标图像格式</param>
        /// <param name="level">图像清晰度（1～100）</param>
        /// <returns></returns>
        public static byte[] Transform(byte[] data, int intToFormat, int level)
        {
            // load the image from the file..
            Image imgInFile = Image.FromStream(new MemoryStream(data));
            MemoryStream ms = new MemoryStream();

            switch (intToFormat)
            {
                case 1: // BMP
                    imgInFile.Save(ms, ImageFormat.Bmp);
                    SavePicture(imgInFile, ms, ImageFormat.Bmp, level);
                    break;
                case 2: // EXIF
                    SavePicture(imgInFile, ms, ImageFormat.Exif, level);
                    break;
                case 3: // EMF
                    SavePicture(imgInFile, ms, ImageFormat.Emf, level);
                    break;
                case 4: // GIF
                    SavePicture(imgInFile, ms, ImageFormat.Gif, level);
                    break;
                case 5: // ICO
                    SavePicture(imgInFile, ms, ImageFormat.Icon, level);
                    break;
                case 6: // JPEG
                    SavePicture(imgInFile, ms, ImageFormat.Jpeg, level);
                    break;
                case 7: // PNG
                    SavePicture(imgInFile, ms, ImageFormat.Png, level);
                    break;
                case 8: // TIFF
                    SavePicture(imgInFile, ms, ImageFormat.Tiff, level);
                    break;
                case 9: // WMF
                    SavePicture(imgInFile, ms, ImageFormat.Wmf, level);
                    break;
                default:
                    SavePicture(imgInFile, ms, ImageFormat.Jpeg, level);
                    break;
            }

            ms.Position = 0;
            byte[] buffer = new byte[ms.Length];
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            ms.Dispose();
            

            return buffer;
        }

        private static void SavePicture(Image imgInFile, MemoryStream ms, ImageFormat format, int level)
        {
            if (format == ImageFormat.Jpeg)
            {
                Bitmap bt = new Bitmap(imgInFile);
                Graphics g = Graphics.FromImage(bt);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                        ici = codec;
                }
                EncoderParameters ep = new EncoderParameters();
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)level);
                bt.Save(ms, ici, ep);

                //释放位图缓存
                bt.Dispose();
            }
            else
            {
                imgInFile.Save(ms, format);
            }

            imgInFile.Dispose();
        }
    }
}
