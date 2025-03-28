namespace Immutability.Exception
{
	public class Result
	{
		public bool IsSuccess { get; }
		public ErrorType? ErrorType { get; }
		public bool IsFailure => !IsSuccess;

		protected Result(bool isSuccess, ErrorType? error)
		{
			if (isSuccess && error != null 
			    || !isSuccess && error == null)
				throw new InvalidOperationException();

			IsSuccess = isSuccess;
			ErrorType = error;
		}

		public static Result Fail(ErrorType? message) => new Result(false, message);

		public static Result<T> Fail<T>(ErrorType? message) => new Result<T>(default(T), false, message);

		public static Result Ok() => new Result(true, null);

		public static Result<T> Ok<T>(T value) => new Result<T>(value, true, null);
	}

	public enum ErrorType
	{
		DataBaseError,
		CustomerAlreadyExists
	}

	public class Result<T> : Result
	{
		private readonly T _value;

		public T Value
		{
			get
			{
				if (!IsSuccess) throw new InvalidOperationException();

				return _value;
			}
		}

		protected internal Result(T value, bool isSuccess, ErrorType? error)
			: base(isSuccess, error) =>
			_value = value;
	}
}