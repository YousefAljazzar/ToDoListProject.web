using System;

namespace ToDoList.Common.Extensions
{
    public class YousefException : Exception
    {
        public int ErrorCode { get; set; }

        public YousefException() : base("Yousef Exception")
        {
        }

        public YousefException(string message) : base(message)
        {
        }

        public YousefException(int statusCode, string message) : base(message)
        {
            ErrorCode = statusCode;
        }

        public YousefException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public YousefException(int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = statusCode;
        }
    }
}
