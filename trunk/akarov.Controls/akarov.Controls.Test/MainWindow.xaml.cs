using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using akarov.Controls.Utils;

namespace akarov.Controls.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImportAttribute("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        public bool IsTrue { get { return true; } }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           

            SingleInstance.SendMessage("hello");


        }

        void SingleInstance_MessageReceiveFromOtherInstance(string message)
        {
            MessageBox.Show(message);
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            SingleInstance.ListenFromOtherInstance(SingleInstance_MessageReceiveFromOtherInstance);
        }

        private void activate_Click(object sender, RoutedEventArgs e)
        {
            SingleInstance.ActivatePreviousInstance();
        }

        private void registerExt_Click(object sender, RoutedEventArgs e)
        {
            FileAssotiation.AssotiateExtention(".frtx");
        }
    }
}
