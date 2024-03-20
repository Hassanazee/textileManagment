namespace textileManagment.Domain.Helper
{
    public class Pagination
    {
        public int? TotalPages { get; set; }
        public int? Total { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public double? RefId { get; set; } = 0;
        public string? SearchQuery { get; set; }
    }
}
