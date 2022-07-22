using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteSize
{
    public class File
    {
        public string Name { get; }
        public long Size { get; }

        public File(string name, long size)
        {
            this.Name = name;
            this.Size = size;
        }
    }
}
