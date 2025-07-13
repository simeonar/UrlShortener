using System.Collections.Generic;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repositories
{
    public interface IUserRepository
    {
        User? GetByUserName(string userName);
        User? GetByApiKey(string apiKey);
        void Add(User user);
        IEnumerable<User> GetAll();
    }
}
