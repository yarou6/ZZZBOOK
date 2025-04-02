using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZZBOOK.Model
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public string FIO => FirstName + " " + LastName + " " + Patronymic;

        public List<Book> Books { get; set; } = new();
    }
}
