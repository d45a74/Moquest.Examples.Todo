using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.BusinessLogic
{
    public interface ITodoService
    {
        Task<bool> DeleteIfCompleted(int id);
        Task<IEnumerable<Todo>> GetMyTodos(int userId);
    }

    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        public TodoService(string serverUrl)
        {
            _todoRepository = new TodoRepository(serverUrl);
        }
        public async Task<bool> DeleteIfCompleted(int id)
        {
            var result = false;
            var entity = await _todoRepository.GetById(id);
            if (entity != null && entity.completed)
                result  = await _todoRepository.Delete(entity.id);
            return result;
        }

        public async Task<IEnumerable<Todo>> GetMyTodos(int userId)
        {
            var entities = await _todoRepository.GetAll();
            return entities.Where(x => x.userId == userId);
        }
    }
}