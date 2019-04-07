using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Linq;
using Android.Content.Res;

namespace PublicTransport.Xamarin.Droid.Services
{
    public static class FileManager
    {
        public static void Delete(string filename, string dirPath)
        {
            // удаляем файл
            File.Delete(GetFilePath(filename, dirPath));
        }

        public static bool Exists(string filename, string dirPath)
        {
            string filepath = GetFilePath(filename, dirPath);
            bool exists = File.Exists(filepath);
            return exists;
        }

        public static IEnumerable<string> GetFiles(string dirPath)
        {
            IEnumerable<string> filenames = from filepath in Directory.EnumerateFiles(dirPath)
                                            select Path.GetFileName(filepath);
            return filenames;
        }

        public static string LoadText(string filename, string dirPath)
        {
            string filepath = GetFilePath(filename, dirPath);
            using (StreamReader reader = File.OpenText(filepath))
            {
                return reader.ReadToEnd();
            }
        }

        public static void SaveText(string filename, string text, string dirPath)
        {
            string filepath = GetFilePath(filename, dirPath);
            using (StreamWriter writer = File.CreateText(filepath))
            {
                writer.Write(text);
            }
        }

        public static string GetFilePath(string filename, string dirPath)
        {
            return Path.Combine(dirPath, filename);
        }
    }
}