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
				string path = args.Length == 1 ? args[0].Trim() : System.IO.Directory.GetCurrentDirectory();

				path = Path.GetFullPath(path);

				System.Console.WriteLine($"Scanning {path}");

				List<Directory> dirs = Utility.ScanDirectoriesAsync(System.IO.Directory.GetDirectories(path)).Result;

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