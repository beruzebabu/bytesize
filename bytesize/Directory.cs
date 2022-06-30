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
                foreach (string subdirPath in System.IO.Directory.EnumerateDirectories(this.Path))
                {
                    Directory subDirectory = new Directory(subdirPath);
                    subDirectory.ScanSync();
                    this.SubDirectories.Add(subDirectory);
                }

                foreach (string filePath in System.IO.Directory.GetFiles(this.Path))
                {
                    FileInfo fi = new FileInfo(filePath);
                    File file = new File(filePath, fi.Length);
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
                Task t = Task.Run(async () =>
                {
                    foreach (string subdirPath in System.IO.Directory.EnumerateDirectories(this.Path))
                    {
                        Directory subDirectory = new Directory(subdirPath);
                        await subDirectory.ScanAsync();
                        SubDirectories.Add(subDirectory);
                    }
                });

                foreach (string filePath in System.IO.Directory.GetFiles(this.Path))
                {
                    FileInfo fi = new FileInfo(filePath);
                    File file = new File(filePath, fi.Length);
                    this.DirectoryFiles.Add(file);
                }

                await t;

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
