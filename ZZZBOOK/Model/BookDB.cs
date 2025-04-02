using MySqlConnector;
using MySqlStart1125.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZZZBOOK.Model
{
    internal class BookDB
    {
        DbConnection connection;

        private BookDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Book book)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `books` Values (0, @Title, @AuthorID, @YearPublished, @Genre, @IsAvailable);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("Title", book.Title));
                cmd.Parameters.Add(new MySqlParameter("AuthorID", book.AuthorID));
                cmd.Parameters.Add(new MySqlParameter("YearPublished", book.YearPublished));
                cmd.Parameters.Add(new MySqlParameter("Genre", book.Genre));
                // можно указать параметр через отдельную переменную
                MySqlParameter isAvailable = new MySqlParameter("IsAvailable", book.IsAvailable);
                cmd.Parameters.Add(isAvailable);
                try
                {
                    // выполняем запрос через ExecuteScalar, получаем id вставленной записи
                    // если нам не нужен id, то в запросе убираем часть select LAST_INSERT_ID(); и выполняем команду через ExecuteNonQuery
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        // назначаем полученный id обратно в объект для дальнейшей работы
                        book.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Book> SelectAll()
        {
            List<Book> books = new List<Book>();
            if (connection == null)
                return books;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT b.id, b.author_id, b.title, b.year_published, b.genre, b.is_available, a.first_name, a.patronymic, a.last_name, birthday FROM books b JOIN author a ON b.author_id = a.id ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        
                        int author_id = dr.GetInt32(1);
                       
                        string title = string.Empty;
                        if (!dr.IsDBNull(2))
                            title = dr.GetString("title");
                        
                        int year_published = 0;
                        if (!dr.IsDBNull(3))
                        year_published = dr.GetInt32("year_published");

                        string genre = string.Empty;
                        if (!dr.IsDBNull(4))
                        genre = dr.GetString("genre");

                        bool is_available = dr.GetBoolean("is_available");

                        string first_name = string.Empty;
                        if (!dr.IsDBNull(5))
                            first_name = dr.GetString("first_name");

                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(6))
                            patronymic = dr.GetString("patronymic");

                        string last_name = string.Empty;
                        if (!dr.IsDBNull(7))
                            last_name = dr.GetString("last_name");

                        DateTime birthday = new DateTime();
                        if (!dr.IsDBNull(8))
                            birthday = dr.GetDateTime("birthday");


                        Author author = new Author 
                        {
                            Id = author_id,
                            FirstName = first_name,
                            LastName = last_name,
                            Birthday = birthday,
                            Patronymic = patronymic,
                        };



                        books.Add(new Book
                        {
                            Id = id,
                            Title = title,
                            AuthorID = author_id,
                            YearPublished = year_published,
                            Genre = genre,
                            IsAvailable = is_available,
                            Author = author,
                        });

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return books;
        }

        internal bool Update(Book edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `books` set `title`=@title, `year_published`=@year_published, `genre`=@genre, `is_available`=@is_available where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("year_published", edit.YearPublished));
                mc.Parameters.Add(new MySqlParameter("genre", edit.Genre));
                mc.Parameters.Add(new MySqlParameter("is_available", edit.IsAvailable));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Book remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `books` where `id` = {remove.Id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static BookDB db;
        public static BookDB GetDb()
        {
            if (db == null)
                db = new BookDB(DbConnection.GetDbConnection());
            return db;
        }

        
    }
}
