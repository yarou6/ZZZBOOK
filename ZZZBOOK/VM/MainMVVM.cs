using System.Collections.ObjectModel;
using System.Windows;
using MySqlStart1125.Model;
using ZZZBOOK.Model;
using ZZZBOOK.View;
using ZZZBOOK.VM;

namespace ZZZBOOK.VM
{
    internal class MainMVVM : BaseVM
    {

        private Author selectedauthor;
        private ObservableCollection<Author> authors = new();


        public ObservableCollection<Author> Authors
        {
            get => authors;
            set
            {
                authors = value;
                Signal();
            }
        }
        public Author SelectedAuthor
        {
            get => selectedauthor;
            set
            {
                selectedauthor = value;
                Signal();
            }
        }
        public CommandMvvm AddAuthor { get; set; }
        public CommandMvvm RemoveAuthor { get; set; }
        public CommandMvvm EditAuthor { get; set; }

        public MainMVVM()
        {
            SelectAll();

            EditAuthor = new CommandMvvm(() =>
            {
                new EditAuthor(SelectedAuthor).ShowDialog();
                SelectAll();
            }, () => SelectedAuthor != null);

            RemoveAuthor = new CommandMvvm(() =>
            {
                List<Book> books = BookDB.GetDb().SelectAll();
                var a = books.FirstOrDefault(s => s.AuthorID == SelectedAuthor.Id);
                if (a != null)
                {
                    MessageBox.Show("Автор привязан к книге. Для начала удалите книгу.");
                }
                else
                {
                    AuthorDB.GetDb().Remove(SelectedAuthor);
                }

                SelectAll();
            }, () => SelectedAuthor != null);

            AddAuthor = new CommandMvvm(() =>
            {
                new EditAuthor(new Author()).ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            Authors = new ObservableCollection<Author>(AuthorDB.GetDb().SelectAll());
        }

    }
}