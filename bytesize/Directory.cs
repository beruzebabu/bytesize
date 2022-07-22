using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteSize
{
    public class Directory
    {
        public string Path { get; }
        public long Size { get; set; }
        public List<Directory> SubDirectories { get; }
        public List<File> DirectoryFiles { get; }
        public bool Scanned { get; set; }

        public Directory(string path)
        {
            this.Path = path;
            this.SubDirectories = new List<Directory>();
            this.DirectoryFiles = new List<File>();
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
                    this.SubDirectories.Add(subDirectory);
                }

                foreach (FileInfo fi in di.GetFiles())
                {
                    File file = new File(fi.Name, fi.Length);
                    this.DirectoryFiles.Add(file);
                }

                this.Size = this.DirectoryFiles.Sum(f => f.Size) + this.SubDirectories.Sum(d => d.Size);
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
                    SubDirectories.Add(subDirectory);
                }

                await Task.WhenAll(quequedTasks);

                foreach (FileInfo fi in di.GetFiles())
                {
                    File file = new File(fi.Name, fi.Length);
                    this.DirectoryFiles.Add(file);
                }

                this.Size = this.DirectoryFiles.Sum(f => f.Size) + this.SubDirectories.Sum(d => d.Size);
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
