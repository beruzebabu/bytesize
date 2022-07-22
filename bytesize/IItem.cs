namespace ByteSize
{
    public interface IItem
    {
        public string Name { get; }
        public long Size { get; }
        public List<IItem> SubItems { get; }
    }
}