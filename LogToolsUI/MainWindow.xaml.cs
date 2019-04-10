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

namespace LogToolsUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        class T1
        {
            public string PathStr { get; set; }
            public T1(string path)
            {
                PathStr = path;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dataGrid.DataContext = makeTable();
                listBox.ItemsSource = new List<T1>() { new T1("1"), new T1("t2") };
            };
        }
        DataTable makeTable()
        {
            var ta = new DataTable();
            new List<string>() { "CommandID", "ActionMode", "ServerID", "Time" }.ForEach(header => ta.Columns.Add(header));
            new List<string>() { "40005", "20", "3" }.ForEach(row => ta.Rows.Add(row, "10", "a", "c"));
            return ta;
        }

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var txtStr = txt.Text;
            var a1 = txtStr.Split('&');
            foreach (var item in a1)
            {

            }
        }

        DataTable SelectTable(DataTable table,string SelectStr)
        {
            var ctable = table.Clone();
            var rows = table.Select(SelectStr);
            foreach (var row in rows)
            {
                ctable.Rows.Add(row.ItemArray);
            }
            return ctable;
        }
    }
}
