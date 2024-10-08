namespace Mango.Services.CouponAPI.Abstraction;

public class Result
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; } = Error.None;

    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new InvalidOperationException("Invalid operation");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    public static Result<T> Success<T>(T data) => new Result<T>(true, Error.None, data);
    public static Result<T> Failure<T>(Error error) => new Result<T>(false, error, default!);

}

public class Result<T> : Result
{
    private T Data { get; }

    public Result(bool isSuccess, Error error, T data) : base(isSuccess, error)
    {
        Data = data;
    }

    public T Value => IsSuccess ? Data : throw new InvalidOperationException("Failure results can't have value");
}
