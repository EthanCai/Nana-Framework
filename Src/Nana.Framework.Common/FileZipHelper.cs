using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace Nana.Framework.Common
{
    public class FileZipHelper
    {
        /// <summary>
        /// 创建一个压缩文件 
        /// </summary>
        /// <param name="zipFilename">压缩后的文件名</param>
        /// <param name="sourceDirectory">待压缩文件的所在目录</param>
        public static void PackFiles(string zipFilename, string sourceDirectory)
        {
            FastZip fz = new FastZip();
            fz.CreateEmptyDirectories = true;
            fz.CreateZip(zipFilename, sourceDirectory, true, "");

            fz = null;
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="zipFile">待解压缩的文件</param>
        /// <param name="directory">解压缩后文件存放的目录</param>
        public static bool UnpackFiles(string zipFile, string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            ZipInputStream zis = new ZipInputStream(File.OpenRead(zipFile));
            ZipEntry theEntry = null;

            while ((theEntry = zis.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);
                if (directoryName != string.Empty)
                    Directory.CreateDirectory(directory + directoryName);

                if (fileName != string.Empty)
                {
                    string spath = Path.Combine(directory, theEntry.Name);
                    if (File.Exists(spath))
                    {
                        File.Delete(spath);
                    }
                    FileStream streamWriter = File.Create(spath);
                    int size = 2048;
                    byte[] data = new byte[size];
                    while (true)
                    {
                        size = zis.Read(data, 0, data.Length);
                        if (size > 0)
                            streamWriter.Write(data, 0, size);
                        else
                            break;
                    }

                    streamWriter.Close();
                }
            }

            zis.Close();
            return true;
        }
    }
}
