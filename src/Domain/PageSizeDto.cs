
namespace Domain
{
    public class PageSizeDto
    {
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public PageSizeDto(decimal total, decimal pageSize)
        {
            decimal totalPage = Math.Ceiling((decimal)(total / pageSize));

            TotalPage = (int)totalPage;
            PageSize = (int)pageSize;
            Total = (int)total;
        }
    }
}