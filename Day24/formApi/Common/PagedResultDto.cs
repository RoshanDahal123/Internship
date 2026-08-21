namespace formApi.Common;

    public class PagedResult<T>
    {
   public List<T> Items { get; set; } = new List<T>();
	public int Page { get; set; } = 1;
	public int PageSize { get; set; }
	public int TotalCount { get; set; }
	public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
	public bool HasNextPage => Page < TotalPages;
	public bool HasPreviousPage => Page > 1;
}
