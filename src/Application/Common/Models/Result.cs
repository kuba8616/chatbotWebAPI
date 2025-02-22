namespace ChatbotAI.Application.Common.Models;
public class Result<T>
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public T? Value { get; }
    public List<string> Errors { get; } = new();

    private Result(bool isSuccess, string message, T? value = default, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Value = value;
        if (errors != null)
        {
            Errors = errors;
        }
    }

    public static Result<T> Ok(T value, string message = "Success")
        => new Result<T>(true, message, value);

    public static Result<T> Fail(string message, params string[] errors)
        => new Result<T>(false, message, default, errors.ToList());
}
