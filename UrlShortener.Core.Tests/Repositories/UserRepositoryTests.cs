using System.Collections.Generic;
using System.Linq;
using Xunit;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Tests.Repositories
{
    public class InMemoryUserRepository
    {
        private readonly List<User> _users = new();

        public void Add(User user) => _users.Add(user);
        public User? GetByUserName(string userName) => _users.FirstOrDefault(u => u.UserName == userName);
        public User? GetByApiKey(string apiKey) => _users.FirstOrDefault(u => u.ApiKey == apiKey);
        public IEnumerable<User> GetAll() => _users;
    }

    public class UserRepositoryTests
    {
        [Fact]
        public void Add_And_GetByUserName_Works()
        {
            var repo = new InMemoryUserRepository();
            var user = new User { UserName = "test", ApiKey = "key1" };
            repo.Add(user);
            var found = repo.GetByUserName("test");
            Assert.NotNull(found);
            Assert.Equal("key1", found.ApiKey);
        }

        [Fact]
        public void GetByApiKey_ReturnsCorrectUser()
        {
            var repo = new InMemoryUserRepository();
            repo.Add(new User { UserName = "a", ApiKey = "k1" });
            repo.Add(new User { UserName = "b", ApiKey = "k2" });
            var found = repo.GetByApiKey("k2");
            Assert.NotNull(found);
            Assert.Equal("b", found.UserName);
        }

        [Fact]
        public void GetAll_ReturnsAllUsers()
        {
            var repo = new InMemoryUserRepository();
            repo.Add(new User { UserName = "a" });
            repo.Add(new User { UserName = "b" });
            var all = repo.GetAll().ToList();
            Assert.Equal(2, all.Count);
        }
    }
}
