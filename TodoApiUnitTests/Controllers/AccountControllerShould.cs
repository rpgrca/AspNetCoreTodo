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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TodoApi.Controllers;
using TodoApi.DTO;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.UnitTests.Controllers.Support;
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
        [InlineData("e@mail.com", "Super_Password123")]
        public async Task Login_WithCorrectLogin_ShouldGenerateToken(string email, string password)
        {
            // Arrange
            var data = new List<Models.ApplicationUser>() { new ApplicationUser { UserName = email, Email = email } };
            var dbSetMock = data.AsDbSetMock();

            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.SetupGet(x => x.Users)
                           .Returns(dbSetMock.Object);

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x["JwtKey"]).Returns("SOME_RANDOM_KEY_DO_NOT_SHARE");
            mockConfiguration.SetupGet(x => x["JwtIssuer"]).Returns("http://yourdomain.com");
            mockConfiguration.SetupGet(x => x["JwtExpireDays"]).Returns("30");

            var mockSignInManager = new Mock<FakeSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(email, password, false, false))
                             .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            using (var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act
                var result = await controller.Login(new LoginDTO { Email = email, Password = password });

                // Assert
                Assert.Null(result.Result);
                Assert.NotNull(result.Value);
            }
        }

        [Theory]
        [InlineData("r@gmail.com", "Super_Password123")]
        public async Task Register_WithValidInformation_ShouldGenerateToken(string email, string password)
        {
            // Arrange
            var identityUser = new ApplicationUser { UserName = email, Email = email };
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x["JwtKey"]).Returns("SOME_RANDOM_KEY_DO_NOT_SHARE");
            mockConfiguration.SetupGet(x => x["JwtIssuer"]).Returns("http://yourdomain.com");
            mockConfiguration.SetupGet(x => x["JwtExpireDays"]).Returns("30");

            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.Setup(x => x.CreateAsync(It.Is<ApplicationUser>(p => p.Email == email && p.UserName == email), password))
                           .ReturnsAsync(IdentityResult.Success);

            var mockSignInManager = new Mock<FakeSignInManager>();
            mockSignInManager.Setup(x => x.SignInAsync(identityUser, false, null));

            using (var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act
                var result = await controller.Register(new RegisterDTO { Email = email, Password = password });

                // Assert
                Assert.Null(result.Result);
                Assert.NotNull(result.Value);
            }
        }

        [Theory]
        [InlineData("r@gmail.com", "Super_Password123")]
        public async Task Register_WithInvalidInformation_ShouldThrowException(string email, string password) {
             // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.Setup(x => x.CreateAsync(It.Is<ApplicationUser>(p => p.Email == email && p.UserName == email), password))
                           .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Failed(new IdentityError[] { new IdentityError() {} }));

            using (var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act/Assert
                await Assert.ThrowsAsync<ApplicationException>(() => controller.Register(new RegisterDTO { Email = email, Password = password }));
            }
        }

        [Theory]
        [InlineData("r@gmail.com", "Super_Password123")]
        public async Task Register_WithInvalidInformationAndEmptyErrorList_ShouldThrowUnknownException(string email, string password) {
             // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSignInManager = new Mock<FakeSignInManager>();
            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.Setup(x => x.CreateAsync(It.Is<ApplicationUser>(p => p.Email == email && p.UserName == email), password))
                           .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Failed());

            using (var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockConfiguration.Object))
            {
                // Act/Assert
                await Assert.ThrowsAsync<ApplicationException>(() => controller.Register(new RegisterDTO { Email = email, Password = password }));
            }
        }
    }
}
