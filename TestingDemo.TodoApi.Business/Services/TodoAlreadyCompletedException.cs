using System;

namespace TestingDemo.TodoApi.Business.Services
{
    public class TodoAlreadyCompletedException : Exception
    {
        public TodoAlreadyCompletedException(string message)
            : base(message)
        { }
    }
}