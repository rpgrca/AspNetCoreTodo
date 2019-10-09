using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TodoApi.Controllers;
using TodoApi.DTO;
using TodoApi.Models;
using TodoApi.Services;
using TodoApiUnitTests.Controllers.Support;
using Xunit;

namespace TodoApi.UnitTests.Services
{
    public class AccountControllerShould
    {
        [Theory]
        [InlineData("e@mail.com", "password")]
        public async Task Login_WithFailedLogin_ShouldThrowApplicationException(string email, string password)
        {
            // Arrange
            var mockUserManager = new FakeUserManager();
            var mockConfiguration = new Mock<IConfiguration>();

            var mockSignInManager = new Mock<FakeSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(email, password, false, false))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            using (var controller = new AccountController(mockUserManager, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act/Assert
                await Assert.ThrowsAsync<ApplicationException>(() => controller.Login(new LoginDTO { Email = email, Password = password }));
            }
        }

        [Theory]
        [InlineData("e@mail.com", "password")]
        public async Task Login_WithCorrectLogin_ShouldGenerateToken(string email, string password)
        {
            // Arrange
            var mockUsers = new Mock<IQueryable<ApplicationUser>>();
            mockUsers.Setup(x => x.SingleOrDefault(It.IsAny<Func<ApplicationUser, bool>>()))
                     .Returns(new ApplicationUser(email));

            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.SetupGet(x => x.Users)
                           .Returns(mockUsers.Object);

            var mockConfiguration = new Mock<IConfiguration>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(email, password, false, false))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            using (var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act
                var result = await controller.Login(new LoginDTO { Email = email, Password = password });
            }
        }

        [Theory]
        [InlineData("r@gmail.com", "Super_Password123")]
        public async Task Register_WithValidInformation_ShouldGenerateToken(string email, string password)
        {
            // Arrange
            var identityUser = new ApplicationUser { UserName = email, Email = email };
            var mockConfiguration = new Mock<IConfiguration>();

            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.Setup(x => x.CreateAsync(It.Is<ApplicationUser>(p => p.Email == email && p.UserName == email), password))
                           .ReturnsAsync(IdentityResult.Success);

            var mockSignInManager = new Mock<FakeSignInManager>();
            mockSignInManager.Setup(x => x.SignInAsync(identityUser, false, null));

            using (var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act/Assert
                var result = await controller.Register(new RegisterDTO { Email = email, Password = password });

            }
        }
    }
}
