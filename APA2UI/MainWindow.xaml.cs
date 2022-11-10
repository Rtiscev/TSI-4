using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Word = Microsoft.Office.Interop.Word;

namespace APA2UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Column_generator(GridManipulation);
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

        private void Column_generator(Grid gridman)
        {
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
            Viewbox vb3 = new() { Stretch = Stretch.Uniform, VerticalAlignment = VerticalAlignment.Top };
            StackPanel stackPanel = new() { Orientation = Orientation.Vertical, Margin = new Thickness(1, 5, 1, 5) };
            Grid grid = new();
            TextBlock text = new();

            StackP1.Children.Add(vb3);
            vb3.Child = stackPanel;
            stackPanel.Children.Add(grid);


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

            Column_generator(grid);
            fill_cols(grid);

            StackP1.Children.Add(text);

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

            wordproc.Graphs();
            List<string> sortsnames = new() { "ShellSort", "QuickSort", "RadixSort" };
            for (int i = 0; i < Time_t.Count; i++)
            {
                TextBlock textBlock = new();
                textBlock.FontSize = 20;
                textBlock.Text = sortsnames[i] + " : " + Time_t[i].ToString();
                StackP1.Children.Add(textBlock);
            }
        }


        public List<object> Array = new List<object>();
        private List<TextBox> box = new();
        private int count = 0;
        private int clicked_sort = 0;
        public int Count { get { return count; } }
        public List<TextBox> Box { get { return box; } }

        public List<double> Time_t = new();

        private void Window_main_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

    }
}
