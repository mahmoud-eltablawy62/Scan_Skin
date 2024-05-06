namespace ScanSkin.Api.Helpers
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageSize, int pageIndex, IReadOnlyList<T> data, int count)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            PageCount = count;
            Data = data;
        }
    }
}
