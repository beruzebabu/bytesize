using System.Diagnostics;

namespace ByteSize.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				string path = args.Length >= 1 ? args[0].Trim() : System.IO.Directory.GetCurrentDirectory();
				bool sync = args.Length >= 2 && args[1].Trim() == "-s" ? true : false;

				path = Path.GetFullPath(path);

                List<Directory> dirs = new List<Directory>();
                if (sync)
				{
                    System.Console.WriteLine($"Sync scanning {path}");
                    dirs = Utility.ScanDirectories(new string[] { path });
                }
				else
				{
                    System.Console.WriteLine($"Scanning {path}");
                    dirs = Utility.ScanDirectoriesAsync(new string[] { path }).Result;
                }

				stopwatch.Stop();
				System.Console.WriteLine($"Scanning took {stopwatch.Elapsed}");

				System.Console.WriteLine($"Total size {dirs.Sum(d => d.Size)} bytes");
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.Message);
			}
        }
    }
}