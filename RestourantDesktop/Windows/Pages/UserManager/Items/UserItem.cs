using mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RestourantDesktop.Windows.Pages.UserManager.Items
{
    internal class UserItem : NotifyPropertyChanged
    {
        public int UserID;

        public string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeUserStats();
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                //TODO: Диалог с пользователем точно ли он хочет сменить пароль
                if (true)
                {
                    password = value;
                    ChangeUserPassword();
                }
                else
                {
                    password = password;
                }
                OnPropertyChanged();
            }
        }

        public string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeUserStats();
            }
        }

        public string passport;
        public string Passport
        {
            get => passport;
            set
            {
                passport = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeUserStats();
            }
        }
        
        public string phoneNum;
        public string PhoneNum
        {
            get => phoneNum;
            set
            {
                phoneNum = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeUserStats();
            }
        }

        private ObservableCollection<UserRoleItem> roles;
        public ObservableCollection<UserRoleItem> Roles
        {
            get => roles;
            set
            {
                roles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PositionItem> Positions { get => UserManagerModel.PositionsList; }

        public PositionItem selectedPosItem;
        public PositionItem SelectedPosItem
        { 
            get => selectedPosItem;
            set
            {
                if (value == null) return;
                selectedPosItem = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeUserStats();
            }
        }

        private Command deleteCommand;
        public Command DeleteCommand
        {
            get => deleteCommand;
            set
            {
                deleteCommand = value;
                OnPropertyChanged();
            }
        }

        private async void ChangeUserStats() => await UserManagerModel.ChangeUserStatsAsync(this);
        private async void ChangeUserPassword() => await UserManagerModel.ChangeUserPasswordAsync(this);

        public UserItem(int ID, string login, string passport,string FullName, string phoneNumber, PositionItem position, IEnumerable<UserRoleItem> items)
        {
            DeleteCommand = new Command(async (obj) => await UserManagerModel.DeleteUserAsync(this));

            this.Roles = new ObservableCollection<UserRoleItem>(items);

            for (int i = 0; i < Roles.Count; i++)
                Roles[i].UserID = ID;

            this.UserID = ID;
            this.login = login;
            this.passport = passport;
            this.fullName = FullName;
            this.phoneNum = phoneNumber;
            this.selectedPosItem = position;
        }
    }

    internal sealed class UserRoleItem : NotifyPropertyChanged
    {
        public int UserID;

        public int RoleID;

        private string roleName;
        public string RoleName
        {
            get => roleName;
            set
            {
                roleName = value;
                OnPropertyChanged();
            }
        }

        public bool isCan;
        public bool IsCan
        {
            get => isCan;
            set
            {
                isCan = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeRight();
            }
        }

        private async void ChangeRight() => await UserManagerModel.ChangeUserRoleAsync(this);

        public UserRoleItem(int ID, int RoleID, string RoleName, bool RoleStatusForUser)
        {
            this.UserID = ID;
            this.RoleID = RoleID;
            this.roleName = RoleName;
            this.isCan = RoleStatusForUser;
        }
    }
}
