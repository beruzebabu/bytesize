using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace ByteSize.GUI
{
    public partial class MainWindow : Window
    {
        private ISizeCalculation sizeCalculation = new GigabyteSizeCalculation();
        private ObservableCollection<Directory> directoryList = new ObservableCollection<Directory>();
        public HierarchicalTreeDataGridSource<Directory> HierarchicalTreeDataGridSource { get; set; }

        public MainWindow()
        {
            this.ClientSize = new Avalonia.Size(600, 700);
            InitializeComponent();
            this.UpdateProgressBar += MainWindow_UpdateProgressBar;

            HierarchicalTreeDataGridSource = new HierarchicalTreeDataGridSource<Directory>(directoryList)
            {
                Columns =
{
                    new HierarchicalExpanderColumn<Directory>(
                        new TextColumn<Directory, string>("Path", d => d.Path),
                    d => d.SubDirectories),
                    new TextColumn<Directory, double>("Size", d => sizeCalculation.Calculate(d.Size)),
                    new TextColumn<Directory, string>("", d => sizeCalculation.Suffix),
                }
            };

            treeDataGridView.Source = HierarchicalTreeDataGridSource;
        }

        private void MainWindow_UpdateProgressBar()
        {
            progressbar.Value = progressbar.Value + 5;
        }

        private async void button_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            Button button = (Button)sender;
            
            try
            {
                string inputPath = Path.GetFullPath(path.Text.Trim());

                Directory directory = new Directory(inputPath);

                button.Content = "Scanning";
                progressbar.IsIndeterminate = true;

                await Task.Run(async () =>
                {
                    Task t = directory.ScanAsync();
                    while(!t.IsCompleted)
                    {
                        await Task.Delay(100);
                        //await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(UpdateProgressBar);
                    }
                    await t;
                });

                directoryList.Add(directory);

                progressbar.IsIndeterminate = false;
                button.Content = "Scan";
            } catch (Exception ex)
            {
                console.Text = $"{ex.Message}";
            }
        }

        public event Action UpdateProgressBar;
    }
}
