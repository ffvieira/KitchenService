namespace KitchenService.Application.Abstractions
{
    public abstract class ResultBase
    {
        protected ResultBase(bool isSuccess, string message, bool isFound = true)
        {
            IsSuccess = isSuccess;
            Message = message;
            IsFound = isFound;
        }
        public bool IsSuccess { get; }
        public bool IsFound { get; }
        public string Message { get; }
    }

    public class Result<T> : ResultBase
    {
        public Result(bool isSuccess, string message, bool isFound = true, T data = default) : base(isSuccess, message, isFound)
        {
            Data = data;
        }

        public T Data { get; }

        public static Result<T> Success(T data, string message = "Success Operation")
        {
            return new Result<T>(true, message, data: data);
        }

        public static Result<T> Failure(string message = "Failed Operation")
        {
            return new Result<T>(false, message);
        }

        public static Result<T> NotFound(string message = "Item Not Found")
        {
            return new Result<T>(false, message, false);
        }

    }

    public class Result : ResultBase
    {
        private Result(bool isSuccess, string message, bool isFound = true) : base(isSuccess, message, isFound)
        {
        }

        public static Result Success(string message = "Success Operation")
            => new Result(true, message);

        public static Result Failure(string message = "Failed Operation")
            => new Result(false, message);

        public static Result NotFound(string message = "Item Not Found")
            => new Result(false, message, isFound: false);
    }
}
