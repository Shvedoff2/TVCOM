using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVCOM.Model
{
    public interface ILoginService
    {
        int? ValidateUser(string userName, string password);
    }

    public class LoginService : ILoginService
    {
        public int? ValidateUser(string userName, string password)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT ID_Сотрудника FROM Сотрудники WHERE Логин='{userName}' AND Пароль='{password}'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return null;
                }
            }
        }
    }
    public interface IProverkaService
    {
        bool ValidateUser(string userName);
    }

    public class ProverkaService : IProverkaService
    {
        public bool ValidateUser(string userName)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Сотрудники WHERE login='{userName}'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
        }
    }
    public interface IRegistService
    {
        bool RegistUser(string userName, string password, string name);
    }

    public class RegistService : IRegistService
    {
        public bool RegistUser(string userName, string password, string name)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO Сотрудники ([Логин], [Пароль], [Имя], [Фамилия], [Фамилия], [ID_Должности]) VALUES('{userName}', '{password}', '{name}', 'user')", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
        }
    }
    public interface IInsertService
    {
        bool InsertUser(string Title, SqlDateTime Date, int ID_form, int ID_author, int ID_t, int ID_h, int ID_r, int ID_s);
    }

    public class InsertService : IInsertService
    {
        public bool InsertUser(string Title, SqlDateTime Date, int ID_form, int ID_author, int ID_t, int ID_h, int ID_r, int ID_s)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO Работы (Название, Дата, ID_формы, ID_автора, ID_трёхмин, ID_худзар, ID_рекламы, ID_штраф) VALUES ('{Title}','{Date}',{ID_form},{ID_author},{ID_t},{ID_h},{ID_r},{ID_s})", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
        }
    }
    public interface IReportService
    {
        bool ReportUser(int ID, DateTime start, DateTime end);
    }

    public class ReportService : IReportService
    {
        public bool ReportUser(int ID, DateTime start, DateTime end)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo("\\Resources\\DocPattern\\!_ТАБЛИЦА  Режиссеры.xlsx")))
            {
                // Выбираем существующий лист
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];

                // Подключаемся к базе данных и выполняем SQL процедуру
                using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand($"EXEC AUTHOR_REPORT {ID},'{start}','{end}'", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Заполняем лист данными, начиная с 6 строки
                    worksheet.Cells["A6"].LoadFromDataTable(dt, true);
                }
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "!_ТАБЛИЦА  Режиссеры"; // Имя по умолчанию
                dlg.DefaultExt = ".xlsx"; // Расширение файла по умолчанию
                dlg.Filter = "Excel documents (.xlsx)|*.xlsx"; // Фильтр файлов для отображения

                // Показываем диалоговое окно
                Nullable<bool> result = dlg.ShowDialog();

                // Получаем выбранный путь
                if (result == true)
                {
                    // Путь к выбранному файлу
                    string filename = dlg.FileName;

                    // Здесь вы можете сохранить свой файл по пути, указанному пользователем
                    // Например, используя EPPlus:
                    package.SaveAs(new FileInfo(filename));
                }
            }
            return true;
        }
    }
}
