using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Windows.Threading;

namespace akarov.Controls.Utils
{
    public class SingleInstance
    {
        private static readonly string PipeName = "akarov.ru/controls/pipename=" + Process.GetCurrentProcess().ProcessName;

        [DllImportAttribute("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Проверяет и активирует окно программы, запущенной до этого экзмепляра
        /// </summary>
        /// <returns>true - если уже было запущено приложение и его окно было активировано</returns>
        public static bool ActivatePreviousInstance()
        {

            Process proc = Process.GetCurrentProcess();
            Process[] otherProcesses = Process.GetProcessesByName(proc.ProcessName);
            if (otherProcesses.Length > 1)
            {
                proc = otherProcesses.First(p => p.Id != proc.Id);
                
                if (proc == null)
                    return false;

                return SetForegroundWindow(proc.MainWindowHandle) > 0;
            }
            else
                return false;
        }

        public static bool IsFirstInstance
        {
            get {

                try
                {
                    var namedPipeStreamASynch =
                                         new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    namedPipeStreamASynch.Dispose();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }

            }
        }

        static NamedPipeServerStream namedPipeStreamASynch;
        static Action<string> messageCallback;
        public static void ListenFromOtherInstance(Action<string> callback)
        {
            try
            {
                namedPipeStreamASynch =
                          new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                messageCallback = callback;
                namedPipeStreamASynch.BeginWaitForConnection(PipeAsyncCallback, null);
            }
            catch (Exception ex)
            {

                throw new Exception("Не могу зарегистрировать слушателя. Вероятно запущено больше одного приложения!",ex);
            }
        }


        private static void PipeAsyncCallback(IAsyncResult resultAsynch)
        {
            try
            {
                string message = "";
                namedPipeStreamASynch.EndWaitForConnection(resultAsynch);
                using (StreamReader streamReader = new StreamReader(namedPipeStreamASynch, Encoding.Unicode))
                {
                    message = streamReader.ReadToEnd().Replace("\0", "");
                    namedPipeStreamASynch.Disconnect();
                   
                }

                namedPipeStreamASynch =
                  new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                namedPipeStreamASynch.BeginWaitForConnection(PipeAsyncCallback, null);

                Dispatcher.CurrentDispatcher.Invoke(messageCallback, message);
                //messageCallback.Invoke(message);
                //messageCallback(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Не могу принять сообщение", ex);
                //Exceptions.ShowException.Show(ex);
            }
        }


        public static void SendMessage(string message)
        {
            using (var namedPipeClientStream = new NamedPipeClientStream(".",PipeName, PipeDirection.Out, PipeOptions.Asynchronous))
            {
                try
                {
                    
                    namedPipeClientStream.Connect(10);
                    var bytes = Encoding.Unicode.GetBytes(message);
                    namedPipeClientStream.Write(bytes, 0, bytes.Length);
                    namedPipeClientStream.Flush();

                }
                catch (TimeoutException ex)
                {
                    throw new Exception("Не могу отправить сообщение. Вероятно не запущен сервер", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Не могу отправить сообщение", ex);
                }
            }
        }
    }
}
