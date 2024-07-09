namespace TrueCode.Todos.Models;

/// <summary>
/// Response model
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginationModel<T>
{
    //might be redundant but ok
    public int PageSize { get; set; }

    //might be redundant but ok
    public int PageNumber { get; set; }

    public T[] PageData { get; set; }

    public int PageCount { get; set; }

    public int TotalCount { get; set; }
}