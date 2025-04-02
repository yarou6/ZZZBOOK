using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZZZBOOK.Model;
using ZZZBOOK.View;
using ZZZBOOK.VM;

namespace ZZZBOOK.VM
{
    internal class MainMVVMBook: BaseVM
    {
        private Book selectedbook;
        private ObservableCollection<Book> books = new();
        private string search;

        public string Search { 
        
            get => search;
            set 
            {
                search = value;
                SearchBook(search);
            }
        }

        public ObservableCollection<Book> Books
        {
            get => books;
            set
            {
                books = value;
                Signal();
            }
        }
        public Book SelectedBook
        {
            get => selectedbook;
            set
            {
                selectedbook = value;
                Signal();
            }
        }
        public CommandMvvm AddBook { get; set; }
        public CommandMvvm RemoveBook { get; set; }
        public CommandMvvm EditBook { get; set; }
        public CommandMvvm UpdateBook { get; set; }

        public MainMVVMBook()
        {
            SelectAll();

            EditBook = new CommandMvvm(() =>
            {
                   new EditBooks(SelectedBook).ShowDialog();
                   SelectAll();
            }, () => SelectedBook != null);

            RemoveBook = new CommandMvvm(() =>
            {
                var bookvozvrat = MessageBox.Show("Вы уверены что хотите удалить кигу?", "Подтверждение", MessageBoxButton.YesNo);
                
                if(bookvozvrat == MessageBoxResult.Yes)
                {
                    BookDB.GetDb().Remove(SelectedBook);
                }
                SelectAll();
            }, () => SelectedBook != null);

            AddBook = new CommandMvvm(() =>
            {
                new EditBooks(new Book()).ShowDialog();
                SelectAll();
            }, () => true);

            UpdateBook = new CommandMvvm(() =>
            {
                SelectAll();
            }, () => true);

        }

        private void SelectAll()
        {
            Books = new ObservableCollection<Book>(BookDB.GetDb().SelectAll());
        }

        private void SearchBook(string search)
        {

            Books = new ObservableCollection<Book>(BookTable.GetTable().SearchBook(search));
        }
    }
}
