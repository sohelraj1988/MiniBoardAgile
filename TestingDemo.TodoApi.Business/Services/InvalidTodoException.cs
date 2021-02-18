using System;

namespace TestingDemo.TodoApi.Business.Services
{
    public class InvalidTodoException : Exception
    {
        public InvalidTodoException(string message)
            : base(message)
        { }
    }
}