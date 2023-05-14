using mvvm;
using RestourantDesktop.Windows.Pages.UserManager.Items;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    internal partial class ViewModelUsers : NotifyPropertyChanged
    {
        private string login;
        public string RedLogin
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string fullName;
        public string RedFullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged();
            }
        }

        private string passport;
        public string RedPassport
        {
            get => passport;
            set
            {
                passport = value;
                OnPropertyChanged();
            }
        }

        private string phoneNum;
        public string RedPhoneNum
        {
            get => phoneNum;
            set
            {
                phoneNum = value;
                OnPropertyChanged();
            }
        }

        public PositionItem selectedPosItem;
        public PositionItem SelectedPosItem
        {
            get => selectedPosItem;
            set
            {
                selectedPosItem = value;
                OnPropertyChanged();
            }
        }

        private Command addNewUserCommand;
        public Command AddNewUserCommand
        {
            get => addNewUserCommand;
            set
            {
                addNewUserCommand = value;
                OnPropertyChanged();
            }
        }

        public void InitUserCreator() =>
            AddNewUserCommand = new Command
            ( 
                async (obj) =>
                {
                    await UserManagerModel.AddNewUserAsync(RedLogin, RedFullName, RedPassport, RedPhoneNum, SelectedPosItem);
                    RedLogin = string.Empty;
                    RedFullName = string.Empty;
                    RedPassport = string.Empty;
                    RedPhoneNum = string.Empty;
                    SelectedPosItem = null;
                    this.OpenMenuCommand?.Execute(null);
                }
            );
    }
}
