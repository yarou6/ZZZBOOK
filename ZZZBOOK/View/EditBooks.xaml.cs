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
    /// Логика взаимодействия для EditBooks.xaml
    /// </summary>
    public partial class EditBooks : Window
    {
        public EditBooks(Book selectedBook)
        {
            InitializeComponent();

            ((WinAddBookMvvm)this.DataContext).SetClose(Close);
            ((WinAddBookMvvm)this.DataContext).SetBook(selectedBook);
            
        }
    }
}
