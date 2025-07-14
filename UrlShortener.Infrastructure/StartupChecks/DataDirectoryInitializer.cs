using System;
using System.IO;

namespace UrlShortener.Infrastructure.StartupChecks
{
    public static class DataDirectoryInitializer
    {
        public static (string filePath, string usersFilePath) EnsureDataFilesExist()
        {
            var dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data");
            var filePath = Path.Combine(dataDir, "shortened_urls.json");
            var usersFilePath = Path.Combine(dataDir, "users.json");

            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
            if (!File.Exists(usersFilePath))
            {
                File.WriteAllText(usersFilePath, "[]");
            }
            return (filePath, usersFilePath);
        }
    }
}
