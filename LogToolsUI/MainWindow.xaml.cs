using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LogDataLib;

namespace LogToolsUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(txt.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var txtStr = txt.Text;
            var a1 = txtStr.Split('&');
            if (dataGrid.DataContext is DataTable dable)
            {
                foreach (var item in a1)
                {
                    dataGrid.DataContext = SelectTable(dable, item);
                }
            }
        }
        DataTable CurrentTable = new DataTable();
        DataTable SelectTable(DataTable table, string SelectStr)
        {
            try
            {
                var ctable = table.Clone();
                var rows = table.Select(SelectStr);
                foreach (var row in rows)
                {
                    ctable.Rows.Add(row.ItemArray);
                }
                return ctable;
            }
            catch (Exception)
            {
                MessageBox.Show("输入字段错误");
                return table;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = CurrentTable;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "请选择文件夹";
            dialog.Filter = "文本文件(*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                listBox.ItemsSource = dialog.FileNames.Select(it => new PathMode(it.Split('\\').LastOrDefault()) { Path = it });
            }
        }

        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is PathMode pathm)
            {
                var table = LogMoudle.GetTableByPath(pathm.Path);
                dataGrid.DataContext = table;
                CurrentTable = table;
            }
        }
    }
}
