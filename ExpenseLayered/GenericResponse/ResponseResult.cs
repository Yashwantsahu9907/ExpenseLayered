namespace ExpenseLayeredApi.GenericResponse;

public class ResponseResult<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; }
    public static ResponseResult<T> Success(T data, string message)  // static method 
    {
        return new()
        {
            StatusCode = 200,
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
    public static ResponseResult<T> Failure(string message, int statusCode = 400)
    {
        return new()
        {
            StatusCode = statusCode,
            IsSuccess = false,
            Message = message
        };
    }
    public static ResponseResult<T> Created(T data, string message)
    {
        return new()
        {
            StatusCode = 201,
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
    public static ResponseResult<T> Conflict(string message)
    {
        return new()
        {
            StatusCode = 409,
            IsSuccess = false,
            Message = message
        };
    }
}