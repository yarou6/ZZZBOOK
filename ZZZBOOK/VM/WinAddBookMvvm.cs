using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZBOOK.Model;
using ZZZBOOK.VM;


namespace ZZZBOOK.VM
{
    internal class WinAddBookMvvm: BaseVM
    {
       
        private Book newBook = new();

        public Book NewBook
        {
            get => newBook;
            set
            {
                newBook = value;
                Signal();
            }
        }

        public List<Author> Authors
        {
            get => authors; set
            { 
                 authors = value;
                Signal();
            }
        }


        public CommandMvvm InsertBook { get; set; }
        public WinAddBookMvvm()
        {

            InsertBook = new CommandMvvm(() =>
            {
                NewBook.AuthorID = NewBook.Author.Id;

                if (newBook.Id == 0)
                {
                    BookDB.GetDb().Insert(NewBook);
                }
                else BookDB.GetDb().Update(NewBook);
                close?.Invoke();

            },
                () =>
                NewBook != null &&
                !string.IsNullOrEmpty(NewBook.Title) &&
                NewBook.YearPublished > 0 &&
                NewBook.YearPublished <= 2025 &&
                !string.IsNullOrEmpty(NewBook.Genre) && NewBook.Author != null
                && NewBook.Title.Length < 256 && NewBook.Genre.Length < 101);


        }
        public void SetBook(Book selectedBook)
        {
            NewBook = selectedBook;
            SelectAuthor();
        }

        Action close;
        private List<Author> authors;

        internal void SetClose(Action close)
        {
            this.close = close;
        }

        public void SelectAuthor()
        {
            Authors = AuthorDB.GetDb().SelectAll();
            if (NewBook.Author != null)
            {
                NewBook.Author = Authors.FirstOrDefault(s=> s.Id == NewBook.AuthorID);
            }
        }

        
    }
}
