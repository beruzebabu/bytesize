using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteSize
{
    public class Directory : IItem
    {
        public string Name { get; }
        public string Path { get; }
        public long Size { get; set; }
        public List<IItem> SubItems { get; }
        public bool Scanned { get; set; }

        public Directory(string path)
        {
            this.Name = path;
            this.Path = path;
            this.SubItems = new List<IItem>();
        }

        public void ScanSync()
        {
            try
            {
                DirectoryInfo di = new System.IO.DirectoryInfo(this.Path);
                foreach (DirectoryInfo subdir in di.EnumerateDirectories())
                {
                    Directory subDirectory = new Directory(subdir.FullName);
                    subDirectory.ScanSync();
                    this.SubItems.Add(subDirectory);
                }

                foreach (FileInfo fi in di.GetFiles())
                {
                    File file = new File(fi.Name, fi.Length);
                    this.SubItems.Add(file);
                }

                this.Size = this.SubItems.Sum(i => i.Size);
                this.Scanned = true;
            }
            catch (Exception e)
            {
                this.Scanned = false;
                Console.WriteLine($"Directory will be excluded from scanning: {e.Message}");
            }
        }

        public async Task ScanAsync()
        {
            try
            {
                List<Task> quequedTasks = new List<Task>();

                DirectoryInfo di = new System.IO.DirectoryInfo(this.Path);
                foreach (DirectoryInfo subdir in di.EnumerateDirectories())
                {
                    Directory subDirectory = new Directory(subdir.FullName);
                    Task t = Task.Run(async () =>
                    {
                        await subDirectory.ScanAsync();
                    });
                    quequedTasks.Add(t);
                    this.SubItems.Add(subDirectory);
                }

                await Task.WhenAll(quequedTasks);

                foreach (FileInfo fi in di.GetFiles())
                {
                    File file = new File(fi.Name, fi.Length);
                    this.SubItems.Add(file);
                }

                this.Size = this.SubItems.Sum(i => i.Size);
                this.Scanned = true;
            }
            catch (Exception e)
            {
                this.Scanned = false;
                Console.WriteLine($"Directory will be excluded from scanning: {e.Message}");
            }
        }
    }
}
