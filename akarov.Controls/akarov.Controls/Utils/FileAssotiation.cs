using System;
using Microsoft.Win32;
using System.Reflection;
using System.IO;

namespace akarov.Controls.Utils
{
    public class FileAssotiation
    {
        public static void AssotiateExtention(string ext)
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly();
                var appName = Path.GetFileNameWithoutExtension(assembly.Location).Replace(" ", "");


                RegistryKey key = Registry.ClassesRoot.OpenSubKey(ext, true) ?? Registry.ClassesRoot.CreateSubKey(ext);
                key.SetValue("", ext.Replace(".", "") + "_auto_file");
                key.Close();

                key = Registry.ClassesRoot.OpenSubKey(ext.Replace(".", "") + "_auto_file", true) ??
                        Registry.ClassesRoot.CreateSubKey(ext.Replace(".", "") + "_auto_file");
                key.SetValue("", "");

                var iconKey = key.OpenSubKey("DefaultIcon",true) ?? key.CreateSubKey("DefaultIcon");
                iconKey.SetValue("", assembly.Location);

                key = key.OpenSubKey("Shell\\Open\\command", true) ??
                        key.CreateSubKey("Shell\\Open\\command");

                key.SetValue("", "\"" + assembly.Location + "\" \"%1\"");
                key.Close();

               
                key.Close();
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Не могу зарегистрировать расширение файла " + ext + 
                                                    "\r\nПрограмма должна быть запущена с правами администратора", 
                                                    ex);
            }

        }
    }
}
