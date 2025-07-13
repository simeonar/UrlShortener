using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    public class FileUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly List<User> _users;

        public FileUserRepository(string filePath)
        {
            _filePath = filePath;
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                _users = new List<User>();
            }
        }

        public User? GetByUserName(string userName) => _users.FirstOrDefault(u => u.UserName == userName);
        public User? GetByApiKey(string apiKey) => _users.FirstOrDefault(u => u.ApiKey == apiKey);
        public IEnumerable<User> GetAll() => _users;
        public void Add(User user)
        {
            _users.Add(user);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_users));
        }
    }
}
