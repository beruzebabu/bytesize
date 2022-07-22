using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteSize
{
    public class File : IItem
    {
        public string Name { get; }
        public long Size { get; }
        public List<IItem> SubItems { get; }

        public File(string name, long size)
        {
            this.Name = name;
            this.Size = size;
            this.SubItems = new List<IItem>();
        }
    }
}
