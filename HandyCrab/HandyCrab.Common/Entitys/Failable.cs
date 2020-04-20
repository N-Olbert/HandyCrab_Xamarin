using System;
using System.Net;

namespace HandyCrab.Common.Entitys
{
    public class Failable<T> : Failable
    {
        public T Value { get; }

        public override bool IsSucceeded() => base.IsSucceeded() && Value != null;

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

        public Failable(T value, HttpStatusCode statusCode, Exception exception)
        {
            Value = value;
            StatusCode = statusCode;
            ThrownException = exception;
        }

        public Failable(Exception exception)
        {
            ErrorCode = GenericErrorCode;
            ThrownException = exception;
        }
    }

    public class Failable
    {
        public const int GenericErrorCode = int.MaxValue;

        public int ErrorCode { get; protected set; }

        public HttpStatusCode StatusCode { get; protected set; }

        public Exception ThrownException { get; protected set; }

        public virtual bool IsSucceeded() => ThrownException == null && ErrorCode == 0;

        protected Failable()
        {
        }

        public Failable(Exception exception)
        {
            ErrorCode = GenericErrorCode;
            ThrownException = exception;
        }
    }
}