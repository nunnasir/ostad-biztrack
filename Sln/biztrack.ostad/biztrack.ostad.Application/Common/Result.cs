namespace biztrack.ostad.Application.Common;

public class Result<T>
{
    public bool Success { get; private set; }
    public string? Error { get; private set; }
    public T? Data { get; private set; }

    private Result() { }

    public static Result<T> SuccessResult(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static Result<T> FailResult(string error) => new()
    {
        Success = false,
        Error = error
    };
}
