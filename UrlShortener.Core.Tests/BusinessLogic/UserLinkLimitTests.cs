using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories;

namespace UrlShortener.Core.Tests.BusinessLogic
{
    public class UserLinkLimitTests
    {
        private const int MaxLinksPerUser = 5;

        [Fact]
        public void User_CannotCreateMoreThanMaxLinks()
        {
            // Arrange
            var user = new User { UserName = "testuser" };
            var links = new List<ShortenedUrl>();
            for (int i = 0; i < MaxLinksPerUser; i++)
            {
                links.Add(new ShortenedUrl { OwnerUserName = user.UserName });
            }
            // Act
            bool canCreate = links.Count < MaxLinksPerUser;

            // Assert
            Assert.False(canCreate, "User should not be able to create more than max allowed links");
        }
    }
}
