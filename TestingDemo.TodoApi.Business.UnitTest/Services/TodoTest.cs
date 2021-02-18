using System;
using NUnit.Framework;
using TestingDemo.TodoApi.Business.Services;

namespace TestingDemo.TodoApi.Business.UnitTest.Services
{
    [TestFixture]
    public class TodoTest
    {
        [Test]
        public void WhenCreatingTodoWithoutAName_ThenItThrows()
        {
            TestDelegate act = () => ToDo.Create(null, "hoi", 34, DateTime.UtcNow.AddDays(1), "demo@demo.com");

            Assert.Throws<InvalidTodoException>(act, ToDo.NameNotNullEmptyMessage);
        }
    }
}
