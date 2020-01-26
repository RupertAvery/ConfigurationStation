using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace ConfigurationStation.WPF.Actions
{
    public static class Compression
    {
        public static void UnzipFileFromStream(Stream zipStream, string srcFile, string outFile)
        {
            using (var zipInputStream = new ZipInputStream(zipStream))
            {
                while (zipInputStream.GetNextEntry() is ZipEntry zipEntry)
                {
                    var entryFileName = zipEntry.Name;
                    if(zipEntry.IsFile && Path.GetFileName(entryFileName) == srcFile)
                    {
                        // To remove the folder from the entry:
                        //var entryFileName = Path.GetFileName(entryFileName);
                        // Optionally match entrynames against a selection list here
                        // to skip as desired.
                        // The unpacked length is available in the zipEntry.Size property.

                        // 4K is optimum
                        var buffer = new byte[4096];

                        // Manipulate the output filename here as desired.
                        var directoryName = Path.GetDirectoryName(outFile);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        // Unzip file in buffered chunks. This is just as fast as unpacking
                        // to a buffer the full size of the file, but does not waste memory.
                        // The "using" will close the stream even if an exception occurs.
                        using (FileStream streamWriter = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                        {
                            StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                        }

                    }

                }

            }
        }

        public static void UnzipFromStream(Stream zipStream, string srcFolder, string outFolder, Action<string> onunpack)
        {
            using (var zipInputStream = new ZipInputStream(zipStream))
            {
                while (zipInputStream.GetNextEntry() is ZipEntry zipEntry)
                {
                    var entryFileName = zipEntry.Name;
                    if (entryFileName.StartsWith(srcFolder))
                    {
                        entryFileName = entryFileName.Substring(srcFolder.Length);
                        // To remove the folder from the entry:
                        //var entryFileName = Path.GetFileName(entryFileName);
                        // Optionally match entrynames against a selection list here
                        // to skip as desired.
                        // The unpacked length is available in the zipEntry.Size property.

                        // 4K is optimum
                        var buffer = new byte[4096];

                        if (entryFileName.StartsWith("/"))
                        {
                            entryFileName = "." + entryFileName;
                        }

                        // Manipulate the output filename here as desired.
                        var fullZipToPath = Path.Combine(outFolder, entryFileName);
                        var directoryName = Path.GetDirectoryName(fullZipToPath);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        // Skip directory entry
                        if (Path.GetFileName(fullZipToPath).Length == 0)
                        {
                            continue;
                        }

                        onunpack?.Invoke(entryFileName);
                        // Unzip file in buffered chunks. This is just as fast as unpacking
                        // to a buffer the full size of the file, but does not waste memory.
                        // The "using" will close the stream even if an exception occurs.
                        using (FileStream streamWriter = new FileStream(fullZipToPath, FileMode.Create, FileAccess.Write))
                        {
                            StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                        }

                    }

                }
            }
        }
    }
}
