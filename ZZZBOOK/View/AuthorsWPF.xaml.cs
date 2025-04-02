using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZZZBOOK.VM;

namespace ZZZBOOK.View
{
    public partial class AuthorsWPF : Window
    {
        public AuthorsWPF()
        {
            InitializeComponent();
        }

        private void ListBook(object sender, RoutedEventArgs e)
        {
            MainWindow Main = new MainWindow();
            Main.Show();
            Close();
        }
    }
}
