using TodoApp.Services;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;


namespace TodoApp.Tests
{
    public class TodoServiceTestsBase
    {
        protected AppDbContext _context = null!;
        protected TodoService _service = null!;
        [SetUp]//MSTest equivalent :[TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            var mockLogger = new Mock<ILogger<TodoService>>();//fakelogger does nothing
            _service = new TodoService(_context, mockLogger.Object);
        }

        [TearDown]//MSTest equivalent :[TestCleanup]
        public void CleanUp()
        {
            _context.Dispose();
        }


    }
}
