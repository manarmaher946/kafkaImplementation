using Microsoft.EntityFrameworkCore;
namespace Kafkaproducerimplemenation.GenaricPagination
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public List<T> Items { get; private set; }

        public PaginatedList(int pageNumber, List<T> items,int count,int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages =(int) Math.Ceiling(count/(double) pageSize);
            Items = items;
        }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public static async Task<PaginatedList<T>> paginatedList(IQueryable<T> Source,int pageNumber, int PageSize)
        {
            var count = await Source.CountAsync();
            var items =await Source.Skip((pageNumber-1)*PageSize).Take(PageSize).ToListAsync();
            return new PaginatedList<T>(pageNumber,items,count, PageSize);
            

        }
    }
}
