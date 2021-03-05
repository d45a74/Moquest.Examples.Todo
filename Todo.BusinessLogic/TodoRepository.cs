using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace Todo.BusinessLogic
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAll();
        Task<Todo> GetById(int id);
        Task<bool> Delete(int id);
    }

    public class TodoRepository : ITodoRepository
    {
        public static string GetAllUrl = "/todos?_limit=10";
        public static string GetByIdUrl(int id) => $"/todos/{id}";
        public static string DeleteByIdUrl(int id) => $"/todos/{id}";
     
        private readonly string _serverUrl;
        
        public TodoRepository(string serverUrl)
        {
            _serverUrl = serverUrl ?? throw new ArgumentNullException(nameof(serverUrl));
        }
        
        public async Task<IEnumerable<Todo>> GetAll()
        {
            var client = new RestClient(_serverUrl);
            var request = new RestRequest(GetAllUrl, Method.GET);
            var response = await client.ExecuteAsync<IEnumerable<Todo>>(request);
            return response.Data;
        }
        
        public async Task<Todo> GetById(int id)
        {
            var client = new RestClient(_serverUrl);
            var request = new RestRequest(GetByIdUrl(id), Method.GET);
            var response = await client.ExecuteAsync<Todo>(request);
            return response.Data;
        }
        
        public async Task<bool> Delete(int id)
        {
            var client = new RestClient(_serverUrl);
            var request = new RestRequest(DeleteByIdUrl(id), Method.DELETE);
            var response = await client.ExecuteAsync<Todo>(request);
            return response.StatusCode==HttpStatusCode.OK;
        }
    }
}