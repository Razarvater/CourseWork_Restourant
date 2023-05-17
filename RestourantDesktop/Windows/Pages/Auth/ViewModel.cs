using mvvm;
using System.Linq;

namespace RestourantDesktop.DialogManager.Dialogs.AuthorizeDialog.Pages
{
    public class ViewModel : NotifyPropertyChanged
    {
        private string login = string.Empty;
        public string Login
        { 
            get => login;
            set
            { 
                login = value;
                OnPropertyChanged();
            }
        }

        private string password = string.Empty;
        public string Password
        {
            get => new string('*', password.Length);
            set
            {
                if(value.Length == password.Length + 1)
                    password += value.Last();
                else if (value.Length < password.Length)
                    password = password.Remove(password.Length - (password.Length - value.Length));
                else
                    password = value;
                OnPropertyChanged();
            }
        }

        private Command authCommand;
        public Command AuthCommand
        {
            get => authCommand;
            set
            { 
                authCommand = value;
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            AuthCommand = new Command(async (obj) => 
            {
                if (await UserController.UserController.TryAuthorizeUser(password, login))
                    Auth.SendAuth(true);
            });
        }
    }
}
