using MySqlConnector;
using MySqlStart1125.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZZZBOOK.Model
{
    internal class AuthorDB
    {
        DbConnection connection;

        private AuthorDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Author author)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `author` Values (0, @FirstName, @Patronymic, @LastName, @Birthday);select LAST_INSERT_ID();");

                // путем добавления значений в запрос через параметры мы используем экранирование опасных символов
                cmd.Parameters.Add(new MySqlParameter("FirstName", author.FirstName));
                cmd.Parameters.Add(new MySqlParameter("Patronymic", author.Patronymic));
                cmd.Parameters.Add(new MySqlParameter("Birthday", author.Birthday));
                // можно указать параметр через отдельную переменную
                MySqlParameter lastname = new MySqlParameter("LastName", author.LastName);
                cmd.Parameters.Add(lastname);
                try
                {
                    // выполняем запрос через ExecuteScalar, получаем id вставленной записи
                    // если нам не нужен id, то в запросе убираем часть select LAST_INSERT_ID(); и выполняем команду через ExecuteNonQuery
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        // назначаем полученный id обратно в объект для дальнейшей работы
                        author.Id = id;
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

        internal List<Author> SelectAll()
        {
            List<Author> authors = new List<Author>();
            if (connection == null)
                return authors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `first_name`, `patronymic`, `last_name`, `birthday` from `author` ");
                try
                {
                    // выполнение запроса, который возвращает результат-таблицу
                    MySqlDataReader dr = command.ExecuteReader();
                    // в цикле читаем построчно всю таблицу
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string first_name = string.Empty;
                        if (!dr.IsDBNull(1))
                            first_name = dr.GetString("first_name");
                        string patronymic = string.Empty;
                        if (!dr.IsDBNull(2))
                            patronymic = dr.GetString("patronymic");
                        string last_name = string.Empty;
                        if (!dr.IsDBNull(3))
                            last_name = dr.GetString("last_name");
                        DateTime birthday = new DateTime();
                        if (!dr.IsDBNull(4))
                            birthday = dr.GetDateTime("birthday");

                        authors.Add(new Author
                        {
                            Id = id,
                            FirstName = first_name,
                            Patronymic = patronymic,
                            LastName = last_name,
                            Birthday = birthday
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return authors;
        }

        internal bool Update(Author edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `author` set `first_name`=@first_name, `patronymic`=@patronymic, `last_name`=@last_name, `birthday`=@birthday where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("first_name", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("patronymic", edit.Patronymic));
                mc.Parameters.Add(new MySqlParameter("last_name", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("birthday", edit.Birthday));

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


        internal bool Remove(Author remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `author` where `id` = {remove.Id}");
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

        static AuthorDB db;
        public static AuthorDB GetDb()
        {
            if (db == null)
                db = new AuthorDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
