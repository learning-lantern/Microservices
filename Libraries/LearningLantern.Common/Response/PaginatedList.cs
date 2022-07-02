namespace LearningLantern.Common.Response;

public static class PaginatedList
{
    public static IQueryable<T> ToPaginatedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        => source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
}