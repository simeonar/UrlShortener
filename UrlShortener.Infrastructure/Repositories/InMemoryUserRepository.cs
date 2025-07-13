using System.Collections.Generic;
using System.Linq;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of IUserRepository for development and testing.
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public InMemoryUserRepository()
        {
            // Add a default test user for development and tests
            if (!_users.Any())
            {
                _users.Add(new User
                {
                    UserName = "testuser",
                    PasswordHash = "testhash",
                    ApiKey = "test-api-key"
                });
            }
        }

        public User? GetByUserName(string userName) => _users.FirstOrDefault(u => u.UserName == userName);
        public User? GetByApiKey(string apiKey) => _users.FirstOrDefault(u => u.ApiKey == apiKey);
        public void Add(User user) => _users.Add(user);
        public IEnumerable<User> GetAll() => _users;
    }
}
