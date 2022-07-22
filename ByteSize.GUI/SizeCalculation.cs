using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteSize.GUI
{
    internal interface ISizeCalculation
    {
        public string Suffix { get; }
        public double Calculate(long bytes)
        {
            return bytes;
        }

    }

    internal class ByteSizeCalculation : ISizeCalculation
    {
        public string Suffix => "B";
    }

    internal class KilobyteSizeCalculation : ISizeCalculation
    {
        public string Suffix => "KiB";

        public double Calculate(long bytes)
        {
            return Math.Round(bytes / 1024.0, 3);
        }
    }

    internal class MegabyteSizeCalculation : ISizeCalculation
    {
        public string Suffix => "MiB";
        public double Calculate(long bytes)
        {
            return Math.Round(bytes / Math.Pow(1024, 2), 3);
        }
    }

    internal class GigabyteSizeCalculation : ISizeCalculation
    {
        public string Suffix => "GiB";
        public double Calculate(long bytes)
        {
            return Math.Round(bytes / Math.Pow(1024, 3), 3);
        }
    }
}
