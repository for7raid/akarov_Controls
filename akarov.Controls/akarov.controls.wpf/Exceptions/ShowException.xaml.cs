using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Reflection;
using System.IO;
using System.Windows.Input;

namespace akarov.Controls.Exceptions
{
    /// <summary>
    /// Interaction logic for ShowException.xaml
    /// </summary>
    public partial class ShowException : Window
    {
        private static string AppName;
        public string WindowTitle { get; private set; }
        public Exception Exception { get; private set; }

        public string Message { get { return Exception.Message; } }
        public string InnerMessage { get { return Exception.ToString(); } }
        public string MyTitle { get { return WindowTitle + " - " + AppName; } }

        public ICommand CopyCommand
        {
            get
            {

                return new MVVM.Commands.SimpleCommand(_ => { Copy(); });
            }
        }

        public ICommand CloseCommand
        {
            get
            {

                return new MVVM.Commands.SimpleCommand(_ => { this.Close(); });
            }
        }

        public static void Show(Exception ex)
        {
            new ShowException(ex).ShowDialog();
        }

        public static void Show(string windwowTtile, Exception ex)
        {
            new ShowException(windwowTtile, ex).ShowDialog();
        }

        public static void Show(string windwowTtile, string message)
        {
            new ShowException(windwowTtile, new Exception(message)).ShowDialog();
        }

        public static void Show( string message)
        {
            new ShowException(new Exception(message)).ShowDialog();
        }

        public static void Show(string windwowTtile, string message, Exception ex)
        {
            new ShowException(windwowTtile, new Exception(message,ex)).ShowDialog();
        }

        static ShowException()
        {
            //Получим и сохраним имя программы
            AppName = Utils.AssemblyAttributes.ProductTitle;
        }

        private ShowException()
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.IsActive)
                {
                    this.Owner = item;
                    break;
                }
            }

            //InitializeComponent();
            WindowTitle = "Ошибка";
        }

        private ShowException(Exception ex)
            : this()
        {
            this.Exception = ex;
            this.DataContext = this;
        }

        private ShowException(string windwowTtile, Exception ex)
            : this(ex)
        {
            this.WindowTitle = windwowTtile;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void Copy()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} Machine: {1} User: {2}\r\n", DateTime.Now.ToString("s"), Environment.MachineName, Environment.UserName);

            sb.AppendLine(MyTitle);
            sb.AppendFormat("Version: {0}\r\n", Utils.AssemblyAttributes.Version);
            sb.AppendLine(Exception.Source);
            sb.AppendLine(Exception.Message);
            sb.AppendLine(Exception.StackTrace);

            var inner = Exception.InnerException;
            while (inner != null)
            {
                sb.AppendLine(inner.Message);

                inner = inner.InnerException;
            }

            Clipboard.SetText(sb.ToString());
        }
    }
}
