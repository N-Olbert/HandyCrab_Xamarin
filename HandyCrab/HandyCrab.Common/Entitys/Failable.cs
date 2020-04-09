using System;
using System.Net;

namespace HandyCrab.Common.Entitys
{
    public class Failable<T>
    {
        public const int GenericErrorCode = int.MaxValue;

        public T Value { get; }

        public int ErrorCode { get; }

        public HttpStatusCode StatusCode { get; }

        public Exception ThrownException { get; }

        public bool IsSucceeded() => ThrownException == null && ErrorCode == 0 && Value != null;

        public Failable(T value, int errorCode, HttpStatusCode statusCode)
        {
            Value = value;
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public Failable(T value, HttpStatusCode statusCode)
        {
            Value = value;
            StatusCode = statusCode;
        }

        public Failable(Exception exception)
        {
            ErrorCode = GenericErrorCode;
            ThrownException = exception;
        }
    }
}