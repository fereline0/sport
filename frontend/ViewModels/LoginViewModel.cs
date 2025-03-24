using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using frontend.Commands;
using frontend.Services;
using shared.Models;

namespace frontend.ViewModels
{
    public class LoginViewModel : NotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private string _name;
        private string _email;
        public string Password { private get; set; }
        public TaskHandler LoginTaskHandler { get; set; }
        public AsyncRelayCommand LoginCommand { get; set; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginTaskHandler = new TaskHandler(Login, 1000);
            LoginCommand = new AsyncRelayCommand(LoginTaskHandler.Execute);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public async Task Login()
        {
            User user = new User
            {
                Name = Name,
                Email = Email,
                Password = Password,
            };

            ServiceResult<Auth> auth = await _authService.PostAuthAsync(user);
        }
    }
}
