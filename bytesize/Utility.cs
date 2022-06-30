namespace ByteSize
{
    public static class Utility
    {
        public static void HelloWorld()
        {
            Console.WriteLine("Hello world!");
        }

        public static List<Directory> ScanDirectories(string[] paths)
        {
            List<Directory> result = new List<Directory>();

            foreach (string path in paths)
            {
                Directory dir = new Directory(path);
                dir.ScanSync();
                result.Add(dir);
            }

            return result;
        }

        public static async Task<List<Directory>> ScanDirectoriesAsync(string[] paths)
        {
            List<Directory> result = new List<Directory>();
            List<Task> quequedTasks = new List<Task>();

            foreach (string path in paths)
            {
                Directory dir = new Directory(path);
                Task t = Task.Run(async () =>
                {
                    await dir.ScanAsync();
                });
                quequedTasks.Add(t);
                result.Add(dir);
            }

            await Task.WhenAll(quequedTasks);

            return result;
        }
    }
}