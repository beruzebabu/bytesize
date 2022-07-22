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
        private ObservableCollection<IItem> directoryList = new ObservableCollection<IItem>();
        public HierarchicalTreeDataGridSource<IItem> HierarchicalTreeDataGridSource { get; set; }

        public MainWindow()
        {
            this.ClientSize = new Avalonia.Size(600, 700);
            InitializeComponent();
            this.UpdateProgressBar += MainWindow_UpdateProgressBar;

            HierarchicalTreeDataGridSource = new HierarchicalTreeDataGridSource<IItem>(directoryList)
            {
                Columns =
{
                    new HierarchicalExpanderColumn<IItem>(
                        new TextColumn<IItem, string>("Path", d => d.Name),
                    d => d.SubItems),
                    new TextColumn<IItem, double>("Size", d => sizeCalculation.Calculate(d.Size)),
                    new TextColumn<IItem, string>("", d => sizeCalculation.Suffix),
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
