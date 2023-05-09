using mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RestourantDesktop.Windows.Pages.UserManager.Items
{
    internal class UserItem : NotifyPropertyChanged
    {
        public int UserID;

        private string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeRoleName();
            }
        }
        
        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeRoleName();
            }
        }

        private string passport;
        public string Passport
        {
            get => passport;
            set
            {
                passport = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeRoleName();
            }
        }
        
        private string phoneNum;
        public string PhoneNum
        {
            get => phoneNum;
            set
            {
                phoneNum = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeRoleName();
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
                selectedPosItem = value;
                OnPropertyChanged();
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

        private async void ChangeRoleName() => await UserManagerModel.ChangeUserAsync(this);

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

        private bool isCan;
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

        private async void ChangeRight() => await UserManagerModel.ChangeUserRole(this);

        public UserRoleItem(int ID, string PageName, bool PageStatusForRole)
        {
            this.RoleID = ID;
            this.roleName = PageName;
            this.isCan = PageStatusForRole;
        }
    }
}
