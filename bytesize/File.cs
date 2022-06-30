using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteSize
{
    public class File
    {
        public string Path { get; }
        public long Size { get; }

        public File(string path, long size)
        {
            this.Path = path;
            this.Size = size;
        }
    }
}
