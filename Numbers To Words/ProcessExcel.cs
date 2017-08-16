using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;


namespace Numbers_To_Words
{
    public static class ProcessExcel
    {
        public static string ReadCellValue(string filePath, int rowNo, int columnNo)
        {
            Workbook wb = null;
            try
            {
                Application excel = new Application();
                wb = excel.Workbooks.Open(filePath);
                Worksheet excelSheet = wb.ActiveSheet;
                string value = excelSheet.Cells[rowNo, columnNo].Value.ToString(); //[Row, Column]
                string regex = @"\d+,\d+";
                Regex r = new Regex(regex, RegexOptions.IgnoreCase);
                Match m = r.Match(value);
                if (m.Captures.Count != 0)
                    return m.Groups[0].Value;
                else
                    throw new Exception("Nieprawidłowa wartość w komórce.");       
            }
            finally
            {
                wb.Close();
            }
        }

        public static void SaveToCell(string filePath, int row, int column, string amount)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                Workbook workbook = excel.Workbooks.Open(filePath, ReadOnly: false, Editable: true);
                Worksheet worksheet = workbook.Worksheets.Item[1] as Worksheet;
                if (worksheet == null)
                    return;

                Range cell = worksheet.Cells[row, column];

                cell.Value = amount;
                excel.Application.ActiveWorkbook.Save();
            }
            catch (Exception e)
            {

            }
            finally
            {                
                excel.Application.Quit();
                excel.Quit();
            }
        }

        public static Dictionary<string, int> ReadCellFromTxt()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>(2);
            string line;

            // Read the file and display it line by line.
            using (System.IO.StreamReader file = new System.IO.StreamReader("Dane.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Split(':')[0] == "Rząd")
                        dict.Add("Row", Convert.ToInt32(line.Split(':')[1]));
                    if (line.Split(':')[0] == "Kolumna")
                        dict.Add("Column", Convert.ToInt32(line.Split(':')[1]));
                }
            }
            return dict;
        }
    }
}
