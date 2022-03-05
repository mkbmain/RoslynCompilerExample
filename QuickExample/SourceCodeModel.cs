namespace QuickExample
{
    public class SourceCodeModel<T>
    {
        public string NameSpace { get; set; }
        public string Code { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }

        public T Item { get; set; }
    }
}
