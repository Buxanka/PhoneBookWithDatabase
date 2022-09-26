using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace PhoneBookWithDatabase
{
    class PhoneBook
    {
        private string Name;

        public string name
        {
            get { return Name; }
            set { Name = value; }
        }
        private string Number01;

        public string number01
        {
            get { return Number01; }
            set { Number01 = value; }
        }
        private  string Number02;

        public string number02
        {
            get { return Number02; }
            set { Number02 = value; }
        }
    }
    class Sqlite : ListOfNumbers
    {
        public string CreateTable = "CREATE TABLE PhoneBook (id INTEGER PRIMARY " +
            "KEY ASC AUTOINCREMENT, Name VARCHAR (1, 50), Number01 INTEGER (4, 20), " +
            "Number02 INTEGER (4, 20));";
        public void Print(string ConnectParametr, string SQLCommand)
        {
            SQLiteConnection _sqlite = new SQLiteConnection(ConnectParametr);
            _sqlite.Open();
            SQLiteCommand cmd = _sqlite.CreateCommand();
            cmd.CommandText = (SQLCommand);
            SQLiteDataReader _sql = cmd.ExecuteReader();
            if (_sql.HasRows)
            {
                string _text = "";
                while (_sql.Read())
                {
                    _text += "id: " + _sql["id"]
                        + "\tname: " + _sql["Name"]
                        + "\tNumber01: " + _sql["Number01"]
                        + "\tNumber02: " + _sql["Number02"]
                        + "\n";
                }
                Console.WriteLine(_text);
            }
            else
            {
                Console.WriteLine("Ничего не найдено...");
            }
            _sqlite.Close();
        }
        public void PutInInsert()
        {
            for (int i = 0; i < myList.Count; i++)
            {
                SQLiteConnection _sqlite = new SQLiteConnection("Data source = Phone.db; Version = 3; Mode=ReadWriteCreate;");
                _sqlite.Open();
                SQLiteCommand cmd = _sqlite.CreateCommand();
                cmd.CommandText = "INSERT INTO PhoneBook " +
                "(Name, Number01, Number02) " +
                "VALUES ( '" + myList[i].name +"', '" + myList[i].number01 + "', '" + myList[i].number02 + "');";
                SQLiteDataReader _sql = cmd.ExecuteReader();
                _sqlite.Close();
            }
        }
        public void Request(string SQLCommand)
        {
            SQLiteConnection _sqlite = new SQLiteConnection("Data source = Phone.db; Version = 3; Mode=ReadWriteCreate;");
            _sqlite.Open();
            SQLiteCommand cmd = _sqlite.CreateCommand();
            cmd.CommandText = (SQLCommand);
            SQLiteDataReader _sql = cmd.ExecuteReader();
            _sqlite.Close();
        }
    }
    class ListOfNumbers
    {
        public static List<PhoneBook> myList = new List<PhoneBook>();
        public void myPhoneBook()
        {
            PhoneBook phoneBook = new PhoneBook();
            Console.WriteLine("Введите имя: ");
            phoneBook.name = Console.ReadLine();
            Console.WriteLine("Введите первый номер телефона: ");
            phoneBook.number01 = Console.ReadLine();
            Console.WriteLine("Введие второй номер телефона: ");
            phoneBook.number02 = Console.ReadLine();
            myList.Add(phoneBook);
        }
        public void Print()
        {
            for (int i = 0; i < myList.Count; i++)
            {
                Console.WriteLine("Имя: {0}\tНомер: {1}\tНомер: {2}", myList[i].name, myList[i].number01, myList[i].number02);
            }
        }
        public void File()
        {
            StreamWriter myStream = new StreamWriter("FilePhone.txt");
            for (int i = 0; i < myList.Count; i++)
            {
                myStream.WriteLine("Имя: {0}\tНомер: {1}\tНомер: {2}", myList[i].name, myList[i].number01, myList[i].number02);
            }
            myStream.Close();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ListOfNumbers listOfNumbers = new ListOfNumbers();
            Sqlite sqlite = new Sqlite();
            string number = "";
            do
            {
                Console.WriteLine("Выберите действие:\n1.Добавить запись в телефонную книгу.\t2.Вывести телефонную книгу на экран.\n" +
                    "3.Записать телефонную книгу в файл\t4.Создать базу данных.\n5.Записать телефонную книгу в базу данных.\nQ - выход.");
                number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        try
                        {
                            listOfNumbers.myPhoneBook();
                            Console.WriteLine("Успешно!");
                        }
                        catch
                        {
                            Console.WriteLine("Что-то пошло не так...");
                        }
                        break;
                    case "2":
                        try
                        {
                            listOfNumbers.Print();
                        }
                        catch
                        {
                            Console.WriteLine("Что-то пошло не так...");
                        }
                        break;
                    case "3":
                        try
                        {
                            listOfNumbers.File();
                        }
                        catch
                        {
                            Console.WriteLine("Что-то пошло не так...");
                        }
                        break;
                    case "4":
                        try
                        {
                            sqlite.Request(sqlite.CreateTable);
                        }
                        catch
                        {
                            Console.WriteLine("Что-то пошло не так при создании таблицы базы данных...\n" +
                                "Если вы создавали базу данных, то она уже существует.");
                        }
                        break;
                    case "5":
                        try
                        {
                            sqlite.PutInInsert();
                        }
                        catch
                        {
                            Console.WriteLine("Что-то пошло не так...");
                            throw;
                        }
                        break;
                    default: break;
                }
            } while (number != "q" & number != "Q");
        }
    }
}
