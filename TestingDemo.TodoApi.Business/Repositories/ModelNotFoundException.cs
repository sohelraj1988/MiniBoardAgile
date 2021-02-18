using System;

namespace TestingDemo.TodoApi.Business.Repositories
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException(string message)
            : base(message)
        { }
    }
}
