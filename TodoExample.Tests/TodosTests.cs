using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moquest;
using Todo.BusinessLogic;
using Xunit;

namespace TodoExample.Tests
{
    public class TodosTests
    {
        private static readonly string ExternalUrl = "http://localhost:8080";

        [Fact]
        public async Task DeleteIfCompleted_Should_CallGetById()
        {
            // arrange
            var service = new TodoService(ExternalUrl);
            const int id = 2;
            // mocking
            var path = TodoRepository.GetByIdUrl(id);
            var expectedTodo = new Todo.BusinessLogic.Todo()
            {
                id = id,
                completed = false
            };
            using var moquestGet = await MoquestClient.ConfigureGet(ExternalUrl, path, expectedTodo);
            // act
            var actual = await service.DeleteIfCompleted(id);
            // assert
            actual.Should().BeFalse();
            await moquestGet.Verify(Times.Once());
        }

        [Fact]
        public async Task DeleteIfCompleted_Should_NotCallDeleteIfNotCompleted()
        {
            // arrange
            var service = new TodoService(ExternalUrl);
            const int id = 3;
            // mocking
            var getPath = TodoRepository.GetByIdUrl(id);
            var deletePath = TodoRepository.DeleteByIdUrl(id);
            var expectedTodo = new Todo.BusinessLogic.Todo()
            {
                id = id,
                completed = false
            };
            using var moquestGet = await MoquestClient.ConfigureGet(ExternalUrl, getPath, expectedTodo);
            using var moquestDel = await MoquestClient.ConfigureDelete(ExternalUrl, deletePath, 200);
            // act
            var actual = await service.DeleteIfCompleted(id);
            // assert
            actual.Should().BeFalse();
            await moquestGet.Verify(
                Times.Once());
            await moquestDel.Verify( Times.Never());
        }

        [Fact]
        public async Task DeleteIfCompleted_Should_CallDeleteIfCompleted()
        {
            // arrange
            var service = new TodoService(ExternalUrl);
            const int id = 4;
            // mocking
            var getPath = TodoRepository.GetByIdUrl(id);
            var deletePath = TodoRepository.DeleteByIdUrl(id);
            var expectedTodo = new Todo.BusinessLogic.Todo()
            {
                id = id,
                completed = true
            };
            using var moquestGet = await MoquestClient.ConfigureGet(ExternalUrl, getPath, expectedTodo);
            using var moquestDel = await MoquestClient.ConfigureDelete(ExternalUrl, deletePath, 200);
            // act
            var actual = await service.DeleteIfCompleted(id);
            // assert
            actual.Should().BeTrue();
            await moquestGet.Verify(Times.Once());
            await moquestDel.Verify(Times.Once());
        }
    }
}