using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace APA2UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            InitializeComponent();
            cerrulan.Foreground = new SolidColorBrush(Color.FromRgb(255, 241, 230));
            cerrulan.Background = new SolidColorBrush(Color.FromRgb(11, 142, 194));
            cerrulan.Background.Opacity = 0.7;
        }

        private void Random_generator(object sender, RoutedEventArgs e)
        {
            Array.Clear();

            Random rnd = new();
            for (int i = 0; i < GridManipulation.Children.Count; i++)
            {
                int num = rnd.Next(101);
                Array.Add(num);
            }

            fill_cols(GridManipulation);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Column_generator(GridManipulation, false);
        }
        private void Column_generator(Grid gridman, bool isreadonly)
        {
            VB2.MaxHeight = 100;

            if (count != 0)
            {
                gridman.Children.Clear();
            }
            int c = int.Parse(Amount_of_elements.Text);

            ColumnDefinition[] coldef = new ColumnDefinition[c];

            for (int j = 0; j < c; j++)
            {
                coldef[j] = new ColumnDefinition();
                gridman.ColumnDefinitions.Add(coldef[j]);
            }
            for (int j = 0; j < c; j++)
            {
                TextBox tempik = new();
                tempik.TextAlignment = TextAlignment.Center;
                tempik.Foreground = Brushes.Black;
                tempik.Background = new SolidColorBrush(Colors.White) { Opacity = 0.75 };
                tempik.BorderBrush = new SolidColorBrush(Color.FromRgb(168, 152, 124));
                tempik.BorderThickness = new Thickness(2, 2, 2, 2);
                tempik.MinWidth = 50;
                tempik.MaxHeight = 30;
                tempik.IsReadOnly = isreadonly;
                Grid.SetColumn(tempik, j);
                gridman.Children.Add(tempik);
            }
            count++;
        }
        private void fill_cols(Grid gridman)
        {
            for (int i = 0; i < gridman.Children.Count; i++)
            {
                TextBox child = (TextBox)gridman.Children[i];
                child.Text = Array[i].ToString();
            }
        }
        private void Sort(object sender, RoutedEventArgs e)
        {
            VB3.MaxHeight = 100;
            if (Array.Count == 0)
            {
                for (int i = 0; i < GridManipulation.Children.Count; i++)
                {
                    TextBox tempik = new();
                    tempik = (TextBox)GridManipulation.Children[i];
                    Array.Add(Convert.ToInt32(tempik.Text));
                }
            }
            Array.Sort();
            Column_generator(SortGrid, true);
            fill_cols(SortGrid);
        }
        private void Export_to_Word(object sender, RoutedEventArgs e)
        {
            if (Array.Count == 0)
            {
                for (int i = 0; i < GridManipulation.Children.Count; i++)
                {
                    TextBox tempik = new TextBox();
                    tempik = (TextBox)GridManipulation.Children[i];
                    Array.Add(Convert.ToInt32(tempik.Text));
                }
            }

            WordProc wordproc = new();
            wordproc.word(Array, ref Time_t);

            //wordproc.Graphs();
            List<string> sortsnames = new() { "ShellSort", "QuickSort", "RadixSort" };

            for (int i = 0; i < Time_t.Count; i++)
            {
                TextBlock textBlock = new();
                textBlock.FontSize = 30;
                textBlock.Foreground = Brushes.Beige;
                textBlock.Text = sortsnames[i] + " : " + Time_t[i].ToString();
                StackP1.Children.Add(textBlock);
            }
        }
        private void Window_main_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void Window_main_Deactivated(object sender, EventArgs e)
        {
            this.Activate();
        }

        private const string V = "\\";
        public List<object> Array = new();
        private List<TextBox> box = new();
        private int count = 0;
        private int clicked_sort = 0;
        public List<double> Time_t = new();

        private void OpenPdf(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + V;
            string pathString = System.IO.Path.Combine(path, "data");
            if (!System.IO.Directory.Exists(pathString))
            {
                System.IO.Directory.CreateDirectory(pathString);
            }

            using (Process compiler = new())
            {
                compiler.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
                compiler.StartInfo.Arguments = path + "report.pdf";
                compiler.Start();
                compiler.WaitForExit();
            }


        }
    }
}