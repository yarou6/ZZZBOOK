using MySqlConnector;
using MySqlStart1125.Model;
using System.Windows;

namespace ZZZBOOK.Model
{
    public class BookTable
    {

        private readonly DbConnection myConnection;


        public List<Book> SearchBook(string search)
        {
            List<Book> result = new();
            List<Author> authors = new();

            string query = $"SELECT books.Id AS 'bookid', title, year_published, genre, is_available, author_id, a.last_name, a.first_name, a.patronymic, a.birthday FROM books JOIN author a ON books.author_id = a.id WHERE title LIKE @search OR a.last_name LIKE @search";

            if (myConnection.OpenConnection())
            {// using уничтожает объект после окончания блока (вызывает Dispose)
                using (var mc = myConnection.CreateCommand(query))
                {
                    // передача поиска через переменную в запрос
                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // создание книги на каждую строку в результате
                            var book = new Book();
                            book.Id = dr.GetInt32("bookid");
                            book.YearPublished = dr.GetInt32("year_published");
                            book.Title = dr.GetString("title");
                            book.AuthorID = dr.GetInt32("author_id");
                            book.Genre = dr.GetString("genre");
                            book.IsAvailable = dr.GetBoolean("is_available");

                            // поиск объекта-автора в коллекции authors по ID
                            var author = authors.FirstOrDefault(s => s.Id == book.AuthorID);
                            if (author == null)
                            {
                                // создание автора, если его не было в коллекции
                                author = new Author();
                                author.Id = book.AuthorID;
                                author.FirstName = dr.GetString("first_name");
                                author.LastName = dr.GetString("last_name");
                                author.Patronymic = dr.GetString("patronymic");
                                author.Birthday = dr.GetDateTime("birthday");
                                // добавление автора в коллекцию
                                authors.Add(author);
                            }
                            // добавление книги автору
                            author.Books.Add(book);
                            // указание книге автора
                            book.Author = author;

                            // добавление книги в результат запроса
                            result.Add(book);
                        }
                    }
                }
                myConnection.CloseConnection();
            }
            return result;

        }

        // синглтон start
        static BookTable table;
        private BookTable(DbConnection myConnection)
        {
            this.myConnection = myConnection;
        }
        public static BookTable GetTable()
        {
            if (table == null)
                table = new BookTable(DbConnection.GetDbConnection());
            return table;
        }



    }

}