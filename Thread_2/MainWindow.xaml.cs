using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MultiThreadingApp
{
    public partial class MainWindow : Window
    {
        private Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartTask1_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i <= 50; i++)
                {
                    Dispatcher.Invoke(() => OutputTextBox.AppendText(i + Environment.NewLine));
                    Thread.Sleep(50);
                }
            });
        }

        private void StartTask2_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RangeStartTextBox.Text, out int start) && int.TryParse(RangeEndTextBox.Text, out int end))
            {
                Task.Run(() =>
                {
                    for (int i = start; i <= end; i++)
                    {
                        Dispatcher.Invoke(() => OutputTextBox.AppendText(i + Environment.NewLine));
                        Thread.Sleep(50);
                    }
                });
            }
        }

        private void StartTask3_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ThreadCountTextBox.Text, out int threadCount) &&
                int.TryParse(MultiRangeStartTextBox.Text, out int start) &&
                int.TryParse(MultiRangeEndTextBox.Text, out int end))
            {
                int range = (end - start + 1) / threadCount;
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < threadCount; i++)
                {
                    int threadStart = start + i * range;
                    int threadEnd = (i == threadCount - 1) ? end : threadStart + range - 1;

                    tasks.Add(Task.Run(() =>
                    {
                        for (int j = threadStart; j <= threadEnd; j++)
                        {
                            Dispatcher.Invoke(() => OutputTextBox.AppendText(j + Environment.NewLine));
                            Thread.Sleep(50);
                        }
                    }));
                }

                Task.WhenAll(tasks).ContinueWith(t => Dispatcher.Invoke(() => OutputTextBox.AppendText("All threads completed" + Environment.NewLine)));
            }
        }

        private void StartTask4_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                int[] numbers = new int[10000];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = _random.Next(0, 10000);
                }

                int max = numbers.Max();
                int min = numbers.Min();
                double average = numbers.Average();

                Dispatcher.Invoke(() =>
                {
                    OutputTextBox.AppendText("Max: " + max + Environment.NewLine);
                    OutputTextBox.AppendText("Min: " + min + Environment.NewLine);
                    OutputTextBox.AppendText("Average: " + average + Environment.NewLine);
                });
            });
        }

        private void StartTask5_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                int[] numbers = new int[10000];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = _random.Next(0, 10000);
                }

                int max = numbers.Max();
                int min = numbers.Min();
                double average = numbers.Average();

                string output = $"Max: {max}\nMin: {min}\nAverage: {average}\nNumbers: {string.Join(", ", numbers)}";

                string filePath = "output.txt";
                File.WriteAllText(filePath, output);

                Dispatcher.Invoke(() =>
                {
                    OutputTextBox.AppendText("Data saved to output.txt" + Environment.NewLine);
                });
            });
        }
    }
}