using OfficeOpenXml;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

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
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Сотрудники WHERE Логин='{userName}'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
        }
    }
    public interface IRegistService
    {
        bool RegistUser(string userName, string password, string name, string lname, string otch, int ID);
    }

    public class RegistService : IRegistService
    {
        public bool RegistUser(string userName, string password, string name, string lname, string otch, int ID)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO Сотрудники ([Имя], [Фамилия], [Отчество], [Логин], [Пароль], [ID_Должности]) VALUES('{name}','{lname}','{otch}','{userName}', '{password}', {ID})", conn);
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = "E:\\bl\\TVCOM\\TVCOM\\Resources\\DocPattern\\!_ТАБЛИЦА_Режиссеры.xlsx";
            FileInfo existingFile = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet"];

                // Подключаемся к базе данных и выполняем SQL процедуру
                using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC AUTHOR_REPORT @ID, @start, @end", conn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Заполняем лист данными, начиная с 6 строки
                    worksheet.Cells["A6"].LoadFromDataTable(dt, false);
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
                    package.SaveAs(new FileInfo(filename));
                }
            }
            return true;
        }
    }
    public interface IReportDoljService
    {
        bool ReportDolj(int ID, DateTime start, DateTime end);
    }

    public class ReportDoljService : IReportDoljService
    {
        public bool ReportDolj(int ID, DateTime start, DateTime end)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = "E:\\bl\\TVCOM\\TVCOM\\Resources\\DocPattern\\!_ОБЩАЯ_ТАБЛИЦА_Режиссеры.xlsx";
            FileInfo existingFile = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];

                // Подключаемся к базе данных и выполняем SQL процедуру
                using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC POST_REPORT @ID, @start, @end", conn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Заполняем лист данными, начиная с 6 строки
                    worksheet.Cells["A6"].LoadFromDataTable(dt, false);
                }
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "!__ОБЩАЯ_ТАБЛИЦА Режиссеры"; // Имя по умолчанию
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
    public interface IReportAllService
    {
        bool ReportAll(DateTime start, DateTime end);
    }

    public class ReportAllService : IReportAllService
    {
        public bool ReportAll(DateTime start, DateTime end)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = "E:\\bl\\TVCOM\\TVCOM\\Resources\\DocPattern\\!_ОБЩАЯ_ТАБЛИЦА_Режиссеры.xlsx";
            FileInfo existingFile = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];

                // Подключаемся к базе данных и выполняем SQL процедуру
                using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=Тивиком;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EXEC ALL_REPORT @start, @end", conn);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Заполняем лист данными, начиная с 6 строки
                    worksheet.Cells["A6"].LoadFromDataTable(dt, false);
                }
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "!_ОБЩАЯ_ТАБЛИЦА Режиссеры"; // Имя по умолчанию
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
