using System.Threading.Tasks;

namespace UrlShortener.Core.Services
{
    public interface IQRCodeCache
    {
        Task<byte[]> GetOrAddAsync(string key, Func<Task<byte[]>> valueFactory);
    }
}
