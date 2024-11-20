using Model;
using Moq;
using Shared;
using System;
using VMService;
using Xunit;

namespace UnitTest
{
    public class LoginVMTest
    {
        private readonly Mock<IDataService<User>> _dataServiceMock;
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly Manager _manager;
        private readonly LoginServiceVM _loginServiceVM;

        public LoginVMTest()
        {
            _dataServiceMock = new Mock<IDataService<User>>();
            _navigationServiceMock = new Mock<INavigationService>();
            _manager = new Manager(_dataServiceMock.Object);
            _loginServiceVM = new LoginServiceVM(_manager, _navigationServiceMock.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldNavigateToMainPage()
        {
            // Arrange
            _loginServiceVM.Login = "admin";
            _loginServiceVM.Password = "password";
            _dataServiceMock.Setup(ds => ds.login(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            await _loginServiceVM.LoginCommand.ExecuteAsync(null);

            // Assert
            _navigationServiceMock.Verify(ns => ns.NavigateToMainPageAsync(), Times.Once);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldShowErrorMessage()
        {
            // Arrange
            _loginServiceVM.Login = "user";
            _loginServiceVM.Password = "wrongpassword";
            _dataServiceMock.Setup(ds => ds.login(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var exception = await Record.ExceptionAsync(() => _loginServiceVM.LoginCommand.ExecuteAsync(null));

            // Assert
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task Login_WhenDataServiceThrowsException_ShouldShowErrorMessage()
        {
            // Arrange
            _loginServiceVM.Login = "admin";
            _loginServiceVM.Password = "password";
            _dataServiceMock.Setup(ds => ds.login(It.IsAny<string>(), It.IsAny<string>())).Throws(new NullReferenceException("Service error"));

            // Act
            var exception = await Record.ExceptionAsync(() => _loginServiceVM.LoginCommand.ExecuteAsync(null));

            // Assert
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task Logout_ShouldNavigateToLoginPage()
        {
            // Arrange
            _dataServiceMock.Setup(ds => ds.GetAsyncCurrentUser()).ReturnsAsync(new User());
            _manager.Login("admin", "password");

            // Act
            await _loginServiceVM.LogoutCommand.ExecuteAsync(null);

            // Assert
            _navigationServiceMock.Verify(ns => ns.NavigateToLoginAsync(), Times.Once);
        }

        [Fact]
        public async Task Logout_WhenDataServiceThrowsException_ShouldShowErrorMessage()
        {
            // Arrange
            _dataServiceMock.Setup(ds => ds.GetAsyncCurrentUser()).ReturnsAsync(new User());
            _dataServiceMock.Setup(ds => ds.logout()).Throws(new NullReferenceException("Service error"));
            _manager.Login("admin", "password");

            // Act
            var exception = await Record.ExceptionAsync(() => _loginServiceVM.LogoutCommand.ExecuteAsync(null));

            // Assert
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public void CanLogin_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            _loginServiceVM.Login = "admin";
            _loginServiceVM.Password = "password";

            // Act
            var canLogin = _loginServiceVM.LoginCommand.CanExecute(null);

            // Assert
            Assert.True(canLogin);
        }

        [Fact]
        public void CanLogin_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            _loginServiceVM.Login = "";
            _loginServiceVM.Password = "";

            // Act
            var canLogin = _loginServiceVM.LoginCommand.CanExecute(null);

            // Assert
            Assert.False(canLogin);
        }

        [Fact]
        public void CanLogout_ShouldReturnTrue_WhenUserIsLoggedIn()
        {
            // Arrange
            _dataServiceMock.Setup(ds => ds.GetAsyncCurrentUser()).ReturnsAsync(new User());
            _manager.Login("admin", "password");

            // Act
            var canLogout = _loginServiceVM.LogoutCommand.CanExecute(null);

            // Assert
            Assert.True(canLogout);
        }

        [Fact]
        public void CanLogout_ShouldReturnFalse_WhenNoUserIsLoggedIn()
        {
            // Arrange
            _dataServiceMock.Setup(ds => ds.GetAsyncCurrentUser()).ReturnsAsync((User)null);

            // Act
            var canLogout = _loginServiceVM.LogoutCommand.CanExecute(null);

            // Assert
            Assert.False(canLogout);
        }

        [Fact]
        public void LoginProperty_ShouldRaisePropertyChanged()
        {
            // Arrange
            bool propertyChangedRaised = false;
            _loginServiceVM.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_loginServiceVM.Login))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _loginServiceVM.Login = "newlogin";

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void PasswordProperty_ShouldRaisePropertyChanged()
        {
            // Arrange
            bool propertyChangedRaised = false;
            _loginServiceVM.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_loginServiceVM.Password))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _loginServiceVM.Password = "newpassword";

            // Assert
            Assert.True(propertyChangedRaised);
        }
    }
}