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
using ZZZBOOK.Model;
using ZZZBOOK.VM;

namespace ZZZBOOK.View
{
    /// <summary>
    /// Логика взаимодействия для EditAuthor.xaml
    /// </summary>
    public partial class EditAuthor : Window
    {
        public EditAuthor(Author selectedAuthor)
        {
            InitializeComponent();

            ((WinAddAuthorMvvm)this.DataContext).SetClose(Close);
            ((WinAddAuthorMvvm)this.DataContext).SetAuthor(selectedAuthor);
        }
    }
}
