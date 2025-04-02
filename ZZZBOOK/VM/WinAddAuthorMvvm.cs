using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlStart1125.Model;
using ZZZBOOK.Model;
using ZZZBOOK.VM;

namespace ZZZBOOK.VM
{
    internal class WinAddAuthorMvvm : BaseVM
    {
        private Author newAuthor = new();

        public Author NewAuthor
        {
            get => newAuthor;
            set
            {
                newAuthor = value;
                Signal();
            }
        }

        public CommandMvvm InsertAuthor { get; set; }
        public WinAddAuthorMvvm()
        {
            InsertAuthor = new CommandMvvm(() =>
            {
                if (newAuthor.Id == 0)
                {
                    AuthorDB.GetDb().Insert(NewAuthor);
                    close?.Invoke();
                } else AuthorDB.GetDb().Update(NewAuthor);
                close?.Invoke();


            },
                () =>

                !string.IsNullOrEmpty(newAuthor.FirstName) &&
                !string.IsNullOrEmpty(newAuthor.Patronymic) &&
                !string.IsNullOrEmpty(newAuthor.LastName) &&
                newAuthor.Birthday != null &&
                NewAuthor.FirstName.Length < 256 &&
                NewAuthor.Patronymic.Length < 256 &&
                NewAuthor.LastName.Length < 256);

        }

        public void SetAuthor(Author selectedAuthor)
        { 
           NewAuthor = selectedAuthor;
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}