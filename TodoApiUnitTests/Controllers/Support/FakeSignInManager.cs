using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TodoApi.Models;

namespace TodoApi.UnitTests.Controllers.Support
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
                : base(new FakeUserManager(),
                     new Mock<IHttpContextAccessor>().Object,
                     new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object,
                     new Mock<IUserConfirmation<ApplicationUser>>().Object)
        { }
    }
}
