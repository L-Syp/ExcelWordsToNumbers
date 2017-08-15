using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using Numbers_To_Words;
using LanguageModelFilter;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        Dictionary<string, int> dict;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            dict = ProcessExcel.ReadCellFromTxt();
            if (openFileDialog.ShowDialog() == true)
            {
                file_txtbox.Text = openFileDialog.FileName;
                save_btn.IsEnabled = true;
            }
            currentValue_txtbox.Text = ProcessExcel.ReadCellValue(openFileDialog.FileName, dict["Row"], dict["Column"]);
            valueToSave_txtbox.Text = NumbersToText.ConvertAmountInPLN(ProcessExcel.ReadCellValue(openFileDialog.FileName, dict["Row"], dict["Column"]));
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            ProcessExcel.SaveToCell(openFileDialog.FileName, dict["Row"], dict["Column"], NumbersToText.ConvertAmountInPLN(ProcessExcel.ReadCellValue(openFileDialog.FileName, dict["Row"], dict["Column"])));
            save_btn.IsEnabled = false;
           if (openAfterSave_chkbox.IsChecked == true)
                System.Diagnostics.Process.Start(openFileDialog.FileName);
        }
    }
}
