using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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

            HierarchicalTreeDataGridSource = new HierarchicalTreeDataGridSource<IItem>(directoryList)
            {
                Columns =
{
                    new HierarchicalExpanderColumn<IItem>(
                        new TextColumn<IItem, string>("Path", d => d.Name),
                        d => d.SubItems,
                        d => d.SubItems.Count > 0),
                    new TextColumn<IItem, double>("Size", d => sizeCalculation.Calculate(d.Size)),
                    new TextColumn<IItem, string>("", d => sizeCalculation.Suffix),
                }
            };

            HierarchicalTreeDataGridSource.SortBy(HierarchicalTreeDataGridSource.Columns[1], ListSortDirection.Descending);

            treeDataGridView.Source = HierarchicalTreeDataGridSource;
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

        private async void button_pathselect_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Title = "Select folder to scan";
            string? selectedPath = await openFolderDialog.ShowAsync(this);

            if (string.IsNullOrEmpty(selectedPath))
                return;

            path.Text = selectedPath.Trim();
        }

        private async void sizeDropDown_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count < 1)
                return;

            switch (((ComboBoxItem)selectionChangedEventArgs.AddedItems[0]).Content)
            {
                case "MiB":
                    sizeCalculation = new MegabyteSizeCalculation();
                    break;
                case "GiB":
                    sizeCalculation = new GigabyteSizeCalculation();
                    break;
                case "KiB":
                    sizeCalculation = new KilobyteSizeCalculation();
                    break;
                case "B":
                    sizeCalculation = new ByteSizeCalculation();
                    break;
                default:
                    break;
            }
            if (treeDataGridView != null)
            {
                ListSortDirection direction = treeDataGridView.Source.Columns[1].SortDirection != null ? treeDataGridView.Source.Columns[1].SortDirection.Value : ListSortDirection.Descending;
                treeDataGridView.Source.SortBy(treeDataGridView.Source.Columns[1], direction);
            }
        }
    }
}
