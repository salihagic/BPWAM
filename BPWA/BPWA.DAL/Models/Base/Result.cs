using System.Collections.Generic;
using System.Linq;

namespace TFM.DAL.Models
{
    /// <summary>
    /// Extends base existing class with additional generic memeber
    /// </summary>
    public class Result<T> : Result
    {
        public Result(bool isSuccess)
            : this(default, isSuccess)
        {
        }

        public Result(bool isSuccess, params string[] errors)
            : base(isSuccess, errors)
        {
        }

        public Result(Result result)
            : base(result)
        {
        }

        public Result(T item, bool isSuccess)
            : base(isSuccess)
        {
            Item = item;
        }

        public T Item { get; set; }
    }

    /// <summary>
    /// Can be used as a result of any operation
    /// </summary>
    public class Result
    {
        public Result(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }

        public Result(Result result)
            : this(result.IsSuccess, result.Errors.ToArray())
        {
        }

        public Result(bool isSuccess, params string[] errors)
         : this(isSuccess, errors.Select(e => new ResultError(e)).ToArray())
        {
        }

        public Result(bool isSuccess, params ResultError[] errors)
            : this(isSuccess)
        {
            Errors = errors.ToList();
        }

        public bool IsSuccess { get; set; }
        public List<ResultError> Errors { get; set; } = new List<ResultError>();

        public List<string> GetErrorMessages() => Errors.Select(s => s.Message).ToList();

        #region Static helpers

        public static Result<T> Success<T>(T item) => new SuccessfulResult<T>(item);

        public static Result Success() => new SuccessfulResult();

        public static Result Failed(Result result) => new FailedResult(result.Errors);

        public static Result Failed(List<string> errors) => new FailedResult(errors.ToArray());

        public static Result Failed(List<ResultError> errors) => new FailedResult(errors);

        public static Result Failed(params string[] errors) => new FailedResult(errors);

        public static Result<T> Failed<T>(params string[] errors) => new FailedResult<T>(errors);

        public static Result<T> Failed<T>(List<string> errors) => new FailedResult<T>(errors.ToArray());

        public static Result<T> Failed<T>(List<ResultError> errors) => new FailedResult<T>(errors.ToArray());

        public static Result Failed(params ResultError[] errors) => new FailedResult(errors);

        public static Result<T> Failed<T>(params ResultError[] errors) => new FailedResult<T>(errors);

        #endregion Static helpers

        public override string ToString()
            => IsSuccess ? "Succeeded" : $"Failed, { Errors.Select(e => e.Message).ToList() }";
    }

    /// <summary>
    /// Wrapps successful results
    /// </summary>
    public class SuccessfulResult : Result
    {
        public SuccessfulResult()
            : base(isSuccess: true)
        {
        }
    }

    /// <summary>
    /// Wrapps successful results with generic param
    /// </summary>
    public class SuccessfulResult<T> : Result<T>
    {
        public SuccessfulResult()
            : base(isSuccess: true)
        {
        }

        public SuccessfulResult(T item = default)
            : base(isSuccess: true, item: item)
        {
        }
    }

    /// <summary>
    /// Extend if necessary
    /// </summary>
    public class FailedResult : Result
    {
        public FailedResult()
        : base(isSuccess: false)
        { }

        public FailedResult(params string[] errors)
            : base(isSuccess: false, errors)
        {
        }

        public FailedResult(List<ResultError> errors)
                  : base(isSuccess: false)
        {
            this.Errors = errors;
        }

        public FailedResult(params ResultError[] errors)
            : base(isSuccess: false, errors)
        {
        }
    }

    /// <summary>
    /// Wrapps generic <see cref="Result"/> for failed results
    /// Generic param is only needed so FailedResult can be returned as <see cref="Result{T}"/>
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    public class FailedResult<T> : Result<T>
    {
        public FailedResult(params string[] errors)
            : base(isSuccess: false)
        {
            Errors = errors.Select(message => new ResultError(message)).ToList();
        }

        public FailedResult(params ResultError[] errors)
            : base(isSuccess: false)
        {
            Errors = errors.ToList();
        }
    }

    /// <summary>
    /// Represents result error
    /// </summary>
    public class ResultError
    {
        public ResultError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public ResultError(string message)
            : this(string.Empty, message)
        {
        }

        /// <summary>
        /// Member that this error is associated to or a key used for translation.
        /// Useful in web scenarion for Model state dictionary
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Error description text
        /// </summary>
        public string Message { get; set; }
    }
}
