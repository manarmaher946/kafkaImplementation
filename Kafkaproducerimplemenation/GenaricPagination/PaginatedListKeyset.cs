namespace Kafkaproducerimplemenation.GenaricPagination
{
    public class PaginatedListKeyset<T>
    {
        public PaginatedListKeyset(List<T> items, bool hasNextPage)
        {
            Items = items;
            HasNextPage = hasNextPage;
        }

        public List<T> Items { get; set; }
        public bool HasNextPage { get; set; }
    }
}
