using OxyPlot;
using OxyPlot.Core.Drawing;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using WTemplates;
using Word = Microsoft.Office.Interop.Word;

namespace APA2UI
{
    public class WordProc
    {
        private const string V = "\\";

        public void word(List<object> A, ref List<double> times)
        {
            Sorts sorts = new(0);

            int k = 0;
            var Shell_time = System.Diagnostics.Stopwatch.StartNew();
            var Quick_time = System.Diagnostics.Stopwatch.StartNew();
            var Radix_time = System.Diagnostics.Stopwatch.StartNew();
            Shell_time.Stop();
            Quick_time.Stop();
            Radix_time.Stop();

            #region ShellSort
            sorts.InsertText2("SHELLSORT", 24, 1);
            sorts.ShellSortTable(A, A.Count, true);
            Shell_time.Reset();
            Shell_time.Start();
            sorts.ShellSort(A, A.Count, ref k);
            Shell_time.Stop();
            sorts.InsertText($"Отсортированный массив:");
            sorts.InsertTable1(A);
            sorts.InsertText($"Понадобилось {Shell_time.Elapsed.TotalSeconds} секунд на сортировку");
            #endregion

            #region QuickSort
            sorts.InsertText2("QUICKSORT", 24, 1);
            sorts.QuickSortTable(A, 0, A.Count - 1, true);
            Quick_time.Reset();
            Quick_time.Start();
            sorts.QuickSort(A, 0, A.Count - 1, ref k);
            Quick_time.Stop();
            sorts.InsertText($"Отсортированный массив:");
            sorts.InsertTable1(A);
            sorts.InsertText($"Понадобилось {Quick_time.Elapsed.TotalSeconds} секунд на сортировку");
            #endregion

            #region RadixSort
            sorts.InsertText2("RADIXSORT", 24, 1);
            sorts.RadixSortTable(A, A.Count, true);
            Radix_time.Reset();
            Radix_time.Start();
            sorts.RadixSort(A, A.Count, ref k);
            Radix_time.Stop();
            sorts.InsertText("Отсорированный массив:");
            sorts.InsertTable1(A);
            sorts.InsertText($"Понадобилось {Radix_time.Elapsed.TotalSeconds} секунд на сортировку");
            #endregion

            #region Results
            sorts.InsertText2("Result:", 24, 1);
            sorts.InsertText($"ShellSort: {Shell_time.Elapsed.TotalSeconds} секунд");
            sorts.InsertText($"QuickSort: {Quick_time.Elapsed.TotalSeconds} секунд");
            sorts.InsertText($"RadixSort: {Radix_time.Elapsed.TotalSeconds} секунд");
            #endregion

            #region Timers
            times.Add(Shell_time.Elapsed.TotalSeconds);
            times.Add(Quick_time.Elapsed.TotalSeconds);
            times.Add(Radix_time.Elapsed.TotalSeconds);
            #endregion

            #region Path Creation
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + V;
            string folder = System.IO.Path.Combine(path1, "data" + V);
            if (!Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            #endregion

            Graphs(folder);

            sorts.InsertImage(folder + "iterations.png");
            sorts.InsertImage(folder + "Time.png");

            sorts.Save(folder);
            sorts.Close();
        }
        public void Graphs(string path)
        {
            Sorts sorts = new Sorts(1);

            var width = 1024;
            var height = 768;
            var resolution = 96d;
            string folder = path;

            #region File Names
            string Shell_Time_File = "Time_ShellSort.txt";
            string Quick_Time_File = "Time_QuickSort.txt";
            string Radix_Time_File = "Time_RadixSort.txt";

            string Shell_Iterations_File = "Iterations_ShellSort.txt";
            string Quick_Iterations_File = "Iterations_QuickSort.txt";
            string Radix_Iterations_File = "Iterations_RadixSort.txt";
            #endregion

            #region #region Files Paths
            string Shell_Time_fullPath = folder + Shell_Time_File;
            string Quick_Time_fullPath = folder + Quick_Time_File;
            string Radix_Time_fullPath = folder + Radix_Time_File;

            string Shell_Iterations_fullPath = folder + Shell_Iterations_File;
            string Quick_Iterations_fullPath = folder + Quick_Iterations_File;
            string Radix_Iterations_fullPath = folder + Radix_Iterations_File;
            #endregion

            OxyPlot.PlotModel plotModel = new();
            OxyPlot.PlotModel plotModel1 = new();

            #region Legends
            plotModel.Legends.Add(new Legend()
            {
                LegendSymbolLength = 60,
                LegendFontSize = 24
            });
            plotModel1.Legends.Add(new Legend()
            {
                LegendSymbolLength = 60,
                LegendFontSize = 24
            });
            #endregion

            plotModel.PlotType = OxyPlot.PlotType.Cartesian;
            plotModel1.PlotType = OxyPlot.PlotType.Cartesian;

            #region Title Setup
            plotModel.Title = "Практический график зависимости времени от количества элементов";
            plotModel1.Title = "Практический график зависимости количества итераций от количества элементов";
            plotModel.TitleFontSize = 24;
            plotModel1.TitleFontSize = 24;
            #endregion

            #region Time LineSeries
            OxyPlot.Series.LineSeries Shell_Time_lineSeries = new()
            {
                Title = $"Shell Time",
                Color = OxyColor.FromRgb(255, 10, 255),
                StrokeThickness = 10,
            };
            OxyPlot.Series.LineSeries Quick_Time_lineSeries = new()
            {
                Title = $"Quick Time",
                Color = OxyColor.FromRgb(200, 100, 10),
                StrokeThickness = 10,
            };
            OxyPlot.Series.LineSeries Radix_Time_lineSeries = new()
            {
                Title = $"Radix Time",
                Color = OxyColor.FromRgb(0, 0, 255),
                StrokeThickness = 10,
            };
            #endregion

            #region Iterations LineSeries
            OxyPlot.Series.LineSeries Shell_Iterations_lineSeries = new()
            {
                Title = $"Shell Iterations",
                Color = OxyColor.FromRgb(62, 0, 74),
                StrokeThickness = 10
            };
            OxyPlot.Series.LineSeries Quick_Iterations_lineSeries = new()
            {
                Title = $"Quick Iterations",
                Color = OxyColor.FromRgb(54, 214, 231),
                StrokeThickness = 10
            };
            OxyPlot.Series.LineSeries Radix_Iterations_lineSeries = new()
            {
                Title = $"Radix Iterations",
                Color = OxyColor.FromRgb(182, 208, 132),
                StrokeThickness = 10
            };
            #endregion

            #region Opening Files
            using StreamWriter ShellStream_Time = new(new FileStream(Shell_Time_fullPath, FileMode.Create));
            using StreamWriter QuickStream_Time = new(new FileStream(Quick_Time_fullPath, FileMode.Create));
            using StreamWriter RadixStream_Time = new(new FileStream(Radix_Time_fullPath, FileMode.Create));

            using StreamWriter ShellStream_Iterations = new(new FileStream(Shell_Iterations_fullPath, FileMode.Create));
            using StreamWriter QuickStream_Iterations = new(new FileStream(Quick_Iterations_fullPath, FileMode.Create));
            using StreamWriter RadixStream_Iterations = new(new FileStream(Radix_Iterations_fullPath, FileMode.Create));
            #endregion

            for (int i = 100; i <= 1000; i += 50)
            {
                int iterationsShell = 0;
                int iterationsQuick = 0;
                int iterationsRadix = 0;

                var Shell_time = System.Diagnostics.Stopwatch.StartNew();
                var Quick_time = System.Diagnostics.Stopwatch.StartNew();
                var Radix_time = System.Diagnostics.Stopwatch.StartNew();
                Shell_time.Stop();
                Quick_time.Stop();
                Radix_time.Stop();

                List<object> arr = new();
                GenerateNewArray(ref arr, i);

                #region Timers
                Shell_time.Reset();
                Shell_time.Start();
                sorts.ShellSort(arr, arr.Count, ref iterationsShell);
                Shell_time.Stop();
                Quick_time.Reset();
                Quick_time.Start();
                sorts.QuickSort(arr, 0, arr.Count - 1, ref iterationsQuick);
                Quick_time.Stop();
                Radix_time.Reset();
                Radix_time.Start();
                sorts.RadixSort(arr, arr.Count, ref iterationsRadix);
                Radix_time.Stop();
                #endregion

                #region Adding Points
                Shell_Time_lineSeries.Points.Add(new DataPoint(i, Shell_time.Elapsed.TotalSeconds));
                Quick_Time_lineSeries.Points.Add(new DataPoint(i, Quick_time.Elapsed.TotalSeconds));
                Radix_Time_lineSeries.Points.Add(new DataPoint(i, Radix_time.Elapsed.TotalSeconds));

                Shell_Iterations_lineSeries.Points.Add(new DataPoint(i, iterationsShell));
                Quick_Iterations_lineSeries.Points.Add(new DataPoint(i, iterationsQuick));
                Radix_Iterations_lineSeries.Points.Add(new DataPoint(i, iterationsRadix));
                #endregion

                #region Writing To Files
                ShellStream_Time.WriteLine(i.ToString() + "\t" + Shell_time.Elapsed.TotalSeconds.ToString());
                QuickStream_Time.WriteLine(i.ToString() + "\t" + Quick_time.Elapsed.TotalSeconds.ToString());
                RadixStream_Time.WriteLine(i.ToString() + "\t" + Radix_time.Elapsed.TotalSeconds.ToString());

                ShellStream_Iterations.WriteLine(i.ToString() + "\t" + iterationsShell.ToString());
                QuickStream_Iterations.WriteLine(i.ToString() + "\t" + iterationsQuick.ToString());
                RadixStream_Iterations.WriteLine(i.ToString() + "\t" + iterationsRadix.ToString());
                #endregion
            }

            #region Points
            List<LineSeries> timePoints = new()
            {
                Shell_Time_lineSeries,
                Quick_Time_lineSeries,
                Radix_Time_lineSeries
            };

            List<LineSeries> iterationPoints = new()
            {
                Shell_Iterations_lineSeries,
                Quick_Iterations_lineSeries,
                Radix_Iterations_lineSeries
            };
            #endregion

            string Timetofile = folder + "Time.png";
            string Iterationstofile = folder + "Iterations.png";

            GraphGeneration(ref plotModel, timePoints);
            GraphGeneration2(ref plotModel1, iterationPoints);

            #region Exporting To PNG
            var TimePngExporter = new PngExporter
            {
                Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth,
                Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight
            };

            TimePngExporter.ExportToFile(plotModel, Timetofile);

            var IterationsPngExporter = new PngExporter
            {
                Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth,
                Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight
            };
            IterationsPngExporter.ExportToFile(plotModel1, Iterationstofile);
            #endregion

        }
        private void GraphGeneration(ref OxyPlot.PlotModel tmp, List<LineSeries> time)
        {
            #region X Axis Setup
            OxyPlot.Axes.LinearAxis Xaxis = new();
            Xaxis.Title = "Размерность массива";
            Xaxis.Position = OxyPlot.Axes.AxisPosition.Bottom;
            Xaxis.FontSize = 24;
            Xaxis.AbsoluteMaximum = 1000;
            Xaxis.AbsoluteMinimum = 100;
            Xaxis.MajorStep = 100;
            tmp.Axes.Add(Xaxis);
            #endregion

            #region Y Axis Setup
            OxyPlot.Axes.LinearAxis Yaxis = new();
            Yaxis.Title = "Время выполнения";
            Yaxis.Position = OxyPlot.Axes.AxisPosition.Left;
            Yaxis.FontSize = 24;
            Yaxis.AbsoluteMinimum = 0;
            Yaxis.AbsoluteMaximum = 0.0025;
            Yaxis.MajorStep = 0.0001;
            tmp.Axes.Add(Yaxis);
            #endregion

            foreach (var pointo in time)
            {
                tmp.Series.Add(pointo);
            }
        }
        private void GraphGeneration2(ref OxyPlot.PlotModel tmp, List<LineSeries> time)
        {
            #region X Axis Setup
            OxyPlot.Axes.LinearAxis Xaxis = new();
            Xaxis.Title = "Размерность массива";
            Xaxis.FontSize = 24;
            Xaxis.Position = OxyPlot.Axes.AxisPosition.Bottom;
            Xaxis.AbsoluteMaximum = 1000;
            Xaxis.AbsoluteMinimum = 100;
            Xaxis.MajorStep = 100;
            tmp.Axes.Add(Xaxis);
            #endregion

            #region Y Axis Setup
            OxyPlot.Axes.LinearAxis Yaxis = new();
            Yaxis.Title = "Количество итераций";
            Yaxis.FontSize = 24;
            Yaxis.Position = OxyPlot.Axes.AxisPosition.Left;
            Yaxis.AbsoluteMinimum = 1000;
            Yaxis.AbsoluteMaximum = 100000;
            Yaxis.MajorStep = 5000;
            tmp.Axes.Add(Yaxis);
            #endregion

            foreach (var pointo in time)
            {
                tmp.Series.Add(pointo);
            }
        }
        private void GenerateNewArray(ref List<object> arr, int size)
        {
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                arr.Add(random.Next(0, 1000));
            }
        }

        public class Fuac
        {
            public void makeit()
            {
                wordGen = new APA2UI.WordGen();
                wordGen.iniitialize();
                wordGen.oword.Visible = true;
                wordGen.odoc.SpellingChecked = false;
                wordTemplates.MarginsOfPage(ref wordGen.odoc, 24, 24, 24, 24);
                wordGen.odoc.PageSetup.PaperSize = Word.WdPaperSize.wdPaperA4;
            }
            public void InsertText(string text)
            {
                object Range = wordGen.odoc.Bookmarks[ref wordGen.oendofdoc].Range;
                Word.Paragraph paragraph = wordGen.odoc.Content.Paragraphs.Add(ref Range);
                paragraph.Range.Text = text;
                paragraph.Range.Font.Size = 16;
                paragraph.Range.Font.Name = "Times New Roman";
                paragraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                paragraph.SpaceAfter = 6;
                paragraph.SpaceBefore = 6;
                paragraph.Range.InsertParagraphAfter();
            }
            public void InsertText2(string text, int font_size, int alignment)
            {
                object Range = wordGen.odoc.Bookmarks[ref wordGen.oendofdoc].Range;
                Word.Paragraph paragraph = wordGen.odoc.Content.Paragraphs.Add(ref Range);
                paragraph.Range.Text = text;
                paragraph.Range.Font.Size = font_size;
                paragraph.Range.Font.Name = "Castellar";
                paragraph.Alignment = (Word.WdParagraphAlignment)alignment;
                paragraph.Range.InsertParagraphAfter();
                paragraph.SpaceAfter = 6;
                paragraph.SpaceBefore = 6;
            }
            public void InsertTable(List<object> A, int mark1, int mark2, Word.WdColor color)
            {
                int cols = A.Count;
                Word.Table otable;
                Word.Range wrdrng = wordGen.odoc.Bookmarks.get_Item(ref wordGen.oendofdoc).Range;
                otable = wordGen.odoc.Tables.Add(wrdrng, 1, cols, 1, 2);
                otable.Range.ParagraphFormat.SpaceAfter = 6;

                for (int i = 1; i <= cols; i++)
                {
                    otable.Cell(1, i).Range.Text = A[i - 1].ToString();
                }
                if (mark1 != 0 && mark2 != 0)
                {
                    otable.Cell(1, mark1).Range.Font.Color = Word.WdColor.wdColorWhite;
                    otable.Cell(1, mark2).Range.Font.Color = Word.WdColor.wdColorWhite;
                }
                if (mark1 != 0 && mark2 != 0)
                {
                    otable.Cell(1, mark1).Shading.BackgroundPatternColor = color;
                    otable.Cell(1, mark2).Shading.BackgroundPatternColor = color;
                }
            }
            public void InsertTable1(List<object> A)
            {
                int cols = A.Count;
                Word.Table otable;
                Word.Range wrdrng = wordGen.odoc.Bookmarks.get_Item(ref wordGen.oendofdoc).Range;
                otable = wordGen.odoc.Tables.Add(wrdrng, 1, cols, 1, 2);
                otable.Range.ParagraphFormat.SpaceAfter = 6;
                otable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleTriple;
                otable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleTriple;

                for (int i = 1; i <= cols; i++)
                {
                    otable.Cell(1, i).Range.Text = A[i - 1].ToString();
                }
            }

            public void Close()
            {
                wordGen.oword.Quit();
            }
            public void Save(string path)
            {
                wordGen.odoc.SaveAs2(path + "report", 17);
                wordGen.odoc.SaveAs2(path + "report", 16);
            }

            public void InsertImage(string path)
            {
                object rng = wordGen.odoc.Bookmarks.get_Item(ref wordGen.oendofdoc).Range;
                Word.InlineShape image;
                image = wordGen.odoc.InlineShapes.AddPicture(path, ref wordGen.omissing, ref wordGen.omissing, ref rng);

            }
            APA2UI.WordGen wordGen;
            WordTemplates wordTemplates = new();
        }

        public class Sorts : Fuac
        {
            public Sorts(int choice)
            {
                if (choice == 0)
                {
                    makeit();
                }
            }
            public void ShellSort(List<object> A, int n, ref int iterations)
            {
                static void swap(ref List<Object> A, int pos1, int pos2)
                {
                    var temp = A[pos1];
                    A[pos1] = A[pos2];
                    A[pos2] = temp;
                }
                int gap, j, k;
                for (gap = n / 2; gap > 0; gap /= 2)
                {
                    //initially gap = n/2, decreasing by gap / 2
                    iterations++;
                    for (j = gap; j < n; j++)
                    {
                        iterations++;
                        for (k = j - gap; k >= 0; k -= gap)
                        {
                            iterations++;
                            if ((int)A[k + gap] >= (int)A[k])
                            {
                                break;
                            }
                            else
                            {
                                swap(ref A, k + gap, k);
                            }
                        }
                    }
                }
            }
            public void QuickSort(List<object> A, int start, int end, ref int iterations)
            {
                // base case
                if (start >= end)
                    return;

                // partitioning the array
                int p = partition(A, start, end, ref iterations);

                // Sorting the left part
                QuickSort(A, start, p - 1, ref iterations);

                // Sorting the right part
                QuickSort(A, p + 1, end, ref iterations);

                #region Derivatives
                void swap(ref List<Object> A, int pos1, int pos2)
                {
                    var temp = A[pos1];
                    A[pos1] = A[pos2];
                    A[pos2] = temp;
                }

                int partition(List<object> A, int start, int end, ref int iterations)
                {
                    int pivot = (int)A[start];

                    int count = 0;
                    for (int l = start + 1; l <= end; l++)
                    {
                        iterations++;

                        if ((int)A[l] <= pivot)
                            count++;

                    }

                    // Giving pivot element its correct position
                    int pivotIndex = start + count;

                    swap(ref A, pivotIndex, start);

                    // Sorting left and right parts of the pivot element
                    int k = start, j = end;

                    while (k < pivotIndex && j > pivotIndex)
                    {
                        iterations++;
                        while ((int)A[k] <= pivot)
                        {
                            iterations++;
                            k++;
                        }

                        while ((int)A[j] > pivot)
                        {
                            iterations++;
                            j--;
                        }

                        if (k < pivotIndex && j > pivotIndex)
                        {
                            iterations++;
                            swap(ref A, k++, j--);
                        }
                    }

                    return pivotIndex;
                }
                #endregion
            }
            public void RadixSort(List<object> A, int cols, ref int iterations)
            {
                // Find the maximum number to know number of digits
                int m = getMax(A, cols, ref iterations);


                // Do counting sort for every digit. Note that instead
                // of passing digit number, exp is passed. exp is 10^i
                // where i is current digit number
                for (int exp = 1; m / exp > 0; exp *= 10)
                {
                    iterations++;
                    countSort(A, cols, exp, ref iterations);

                }
                #region Derivatives
                int getMax(List<object> A, int cols, ref int iterations)
                {
                    int mx = (int)A[0];
                    for (int i = 1; i < cols; i++)
                    {
                        iterations++;
                        if ((int)A[i] > mx)
                        {
                            mx = (int)A[i];
                        }
                    }
                    return mx;
                }

                // A function to do counting sort of A[] according to
                // the digit represented by exp.
                void countSort(List<object> A, int cols, int exp, ref int iterations)
                {
                    int[] output = new int[cols]; // output array
                    int i;
                    int[] count = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    // Store count of occurrences in count[]
                    for (i = 0; i < cols; i++)
                    {
                        iterations++;
                        count[((int)A[i] / exp) % 10]++;
                    }

                    // Change count[i] so that count[i] now contains actual
                    //  position of this digit in output[]
                    for (i = 1; i < 10; i++)
                    {
                        iterations++;
                        count[i] += count[i - 1];
                    }

                    // Build the output array
                    for (i = cols - 1; i >= 0; i--)
                    {
                        iterations++;
                        output[count[((int)A[i] / exp) % 10] - 1] = (int)A[i];
                        count[((int)A[i] / exp) % 10]--;
                    }

                    // Copy the output array to A[], so that A[] now
                    // contains sorted numbers according to current digit
                    for (i = 0; i < cols; i++)
                    {
                        iterations++;
                        A[i] = output[i];
                    }
                }
                #endregion
            }

            public void ShellSortTable(List<object> A, int n, bool print)
            {
                static void swap(ref List<Object> A, int pos1, int pos2)
                {
                    var temp = A[pos1];
                    A[pos1] = A[pos2];
                    A[pos2] = temp;
                }
                int gap, j, k;
                for (gap = n / 2; gap > 0; gap /= 2)
                {
                    //initially gap = n/2, decreasing by gap / 2
                    for (j = gap; j < n; j++)
                    {
                        for (k = j - gap; k >= 0; k -= gap)
                        {
                            if (print)
                            {
                                InsertText($"Сравниваю {A[k + gap]} и {A[k]}");
                                InsertTable(A, k + gap + 1, k + 1, Word.WdColor.wdColorPlum);
                            }
                            if ((int)A[k + gap] >= (int)A[k])
                            {
                                break;
                            }
                            else
                            {
                                if (print)
                                {
                                    InsertText($"Меняем {A[k + gap]} и {A[k]} местами");
                                    InsertTable(A, k + gap + 1, k + 1, Word.WdColor.wdColorDarkTeal);
                                }

                                swap(ref A, k + gap, k);
                            }
                        }
                    }
                }
            }
            public void RadixSortTable(List<object> A, int cols, bool print)
            {

                // Find the maximum number to know number of digits
                int m = getMax(A, cols);

                if (print)
                {
                    InsertText($"Максимальное число : {m}");
                }
                // Do counting sort for every digit. Note that instead
                // of passing digit number, exp is passed. exp is 10^i
                // where i is current digit number
                for (int exp = 1; m / exp > 0; exp *= 10)
                {
                    countSort(A, cols, exp);
                    if (print)
                    {
                        InsertText($"Учитывая {exp} разряд");
                        InsertTable(A, 0, 0, 0);
                    }

                }
                #region Derivatives
                int getMax(List<object> A, int cols)
                {
                    int mx = (int)A[0];
                    for (int i = 1; i < cols; i++)
                    {
                        if ((int)A[i] > mx)
                        {
                            mx = (int)A[i];
                        }
                    }
                    return mx;
                }

                // A function to do counting sort of A[] according to
                // the digit represented by exp.
                void countSort(List<object> A, int cols, int exp)
                {
                    int[] output = new int[cols]; // output array
                    int i;
                    int[] count = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    // Store count of occurrences in count[]
                    for (i = 0; i < cols; i++)
                    {
                        count[((int)A[i] / exp) % 10]++;
                    }

                    // Change count[i] so that count[i] now contains actual
                    //  position of this digit in output[]
                    for (i = 1; i < 10; i++)
                    {
                        count[i] += count[i - 1];
                    }

                    // Build the output array
                    for (i = cols - 1; i >= 0; i--)
                    {
                        //InsertText($"FOR {i} ");
                        //InsertTable(output, 0, 0, 0);
                        output[count[((int)A[i] / exp) % 10] - 1] = (int)A[i];
                        count[((int)A[i] / exp) % 10]--;
                    }

                    // Copy the output array to A[], so that A[] now
                    // contains sorted numbers according to current digit
                    for (i = 0; i < cols; i++)
                    {
                        A[i] = output[i];
                    }
                }
                #endregion

            }

            public void QuickSortTable(List<object> A, int start, int end, bool print)
            {
                // base case
                if (start >= end)
                    return;

                // partitioning the array
                int p = partition(A, start, end);

                // Sorting the left part
                QuickSortTable(A, start, p - 1, print);

                // Sorting the right part
                QuickSortTable(A, p + 1, end, print);

                #region Derivatives
                void swap(ref List<Object> A, int pos1, int pos2)
                {
                    var temp = A[pos1];
                    A[pos1] = A[pos2];
                    A[pos2] = temp;
                }

                int partition(List<object> A, int start, int end)
                {
                    int pivot = (int)A[start];

                    if (print)
                    {
                        InsertText($"Опорная точка {pivot}");
                        InsertTable(A, start + 1, start + 1, Word.WdColor.wdColorPlum);
                    }

                    int count = 0;
                    for (int l = start + 1; l <= end; l++)
                    {
                        if ((int)A[l] <= pivot)
                            count++;

                        if (print)
                        {
                            InsertText($"Сравниваю {A[l]} и {pivot} | Счётчик = {count} ");
                            InsertTable(A, l + 1, start + 1, Word.WdColor.wdColorDarkTeal);
                        }
                    }

                    // Giving pivot element its correct position
                    int pivotIndex = start + count;

                    if (print)
                    {
                        InsertText($"Меняем элементы на позициях {pivotIndex} ({A[pivotIndex]}) и {start} ({A[start]}) местами");
                        InsertTable(A, pivotIndex + 1, start + 1, Word.WdColor.wdColorGold);
                    }

                    swap(ref A, pivotIndex, start);

                    // Sorting left and right parts of the pivot element
                    int k = start, j = end;

                    while (k < pivotIndex && j > pivotIndex)
                    {
                        while ((int)A[k] <= pivot)
                        {
                            k++;
                        }

                        while ((int)A[j] > pivot)
                        {
                            j--;
                        }

                        if (k < pivotIndex && j > pivotIndex)
                        {
                            swap(ref A, k++, j--);
                        }
                    }

                    return pivotIndex;
                }
                #endregion
            }

        }
    }
}
