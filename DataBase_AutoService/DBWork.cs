using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using System.Data; // Для возможности использования типа данных DataSet

namespace DataBase_AutoService
{
    internal class DBWork
    {
        static private string dbname = "AutoServise.db";
        static private string path = $"Data Source=AutoServise.db";
        static public bool MakeDB(string _dbname = "AutoServise.db")
        {
            bool result = false;
            string path = $"Data Source={_dbname};";
            string create_table_Mechanices = "CREATE TABLE IF NOT EXISTS " +
                "Mechanices " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "number INTEGER, " +
                "name VARCHAR);";
            string init_data_Mechanices = "INSERT INTO Mechanices (number, name) " +
                "VALUES " +
                "(1, 'Иванов'), " +
                "(2, 'Петров'), " +
                "(3, 'Сидоров'), " +
                "(4, 'Кузнецов');";
            string create_table_Jobs = "CREATE TABLE IF NOT EXISTS " +
                "Jobs " +
                "(Jobs_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "number INTEGER, " +
                "name VARCHAR, " +
                "standartHours REAL, " +
                "costHour DECIMAL, " +
                "client VARCHAR, " +
                "Mechanics_id INTEGER, " +
                "FOREIGN KEY (Mechanics_id) REFERENCES Mechanices (id));";
            string init_data_Jobs = "INSERT INTO Jobs(number, name, standartHours, costHour, client, Mechanics_id)" +
                "VALUES " +
                "(1, 'Прокачка тормозной системы', 1.5, 3000, 'x001xx', 1), " +
                "(2, 'Замена масла в двигателе', 1, 3000, 'x001xx', 3), " +
                "(3, 'Консультация', 0.5, 3000, 'x852xx', 2), " +
                "(4, 'Замена лампочки поворотника', 0.5, 3000, 'x456xx', 4);";
            SQLiteConnection conn = new SQLiteConnection(path);
            SQLiteCommand cmd01 = conn.CreateCommand();
            SQLiteCommand cmd02 = conn.CreateCommand();
            SQLiteCommand cmd03 = conn.CreateCommand();
            SQLiteCommand cmd04 = conn.CreateCommand();
            cmd01.CommandText = create_table_Mechanices;
            cmd02.CommandText = init_data_Mechanices;
            cmd03.CommandText = create_table_Jobs;
            cmd04.CommandText = init_data_Jobs;
            conn.Open();
            cmd01.ExecuteNonQuery();
            cmd02.ExecuteNonQuery();
            cmd03.ExecuteNonQuery();
            cmd04.ExecuteNonQuery();
            conn.Close();
            result = true;
            return result;
        }
        // Метод для получения из базы набора имён мастеров
        static public List<string> GetMechanics()
        {
            List<string> result = new List<string>();
            string get_mechanics = "SELECT name FROM Mechanices;";
            using(SQLiteConnection conn = new SQLiteConnection(path))
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = get_mechanics;
                var reader = cmd.ExecuteReader();
                if (reader.HasRows) // Поля reader имеет поля
                {
                    while (reader.Read()) // Читает значения и переходит на следующие
                    {
                        result.Add(reader.GetString(0)); // Добавляет по строчке
                    }    
                }
            }
            return result;
        }
        static public void AddData(string _newCategoryInsert, string _dbname = "AutoServise.db")
        {
            string path = $"Data Source={_dbname}";
            // Выделяем ресурс, который должен финализироваться по завершении
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn); // cmd - это ссылка                
                cmd.CommandText = _newCategoryInsert;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        static public DataSet Refresh(string _dbname = "AutoServise.db")
        {
            DataSet result = new DataSet();
            string path = $"Data Source={_dbname}";
            string show_all_data = "SELECT * FROM Category;"; // SQL-запрос для вывода всех данных
            // Описываем время существования соединения (создаём соединение и финализируем его)
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(show_all_data, conn); // Создаём адаптер и наполняем его данными
                adapter.Fill(result);
            }
            return result;
        }
        static public void Save(DataTable ds, out string _query, string _dbname = "AutoServise.db")
        {
            // Описание запросов (как раздел var в Pascale)
            string path = $"Data Source={_dbname}";
            string show_all_data = "SELECT * FROM Category;";
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(show_all_data, conn);
                SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(adapter); // Берёт инфу о структуре таблицы
                adapter.Update(ds); // Обновляем данные
                _query = commandBuilder.GetUpdateCommand().CommandText; // Обновлённый текст
            }
        }
    }
}
