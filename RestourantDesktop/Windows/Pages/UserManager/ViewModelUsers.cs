using mvvm;
using RestourantDesktop.Windows.Pages.UserManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    internal partial class ViewModelUsers : NotifyPropertyChanged
    {
        public ObservableCollection<PositionItem> Positions { get => UserManagerModel.PositionsList; }
        public ObservableCollection<UserItem> Users { get => UserManagerModel.UsersList; }

        private Command addNewPositionCommand;
        public Command AddNewPositionCommand
        {
            get => addNewPositionCommand;
            set
            { 
                addNewPositionCommand = value;
                OnPropertyChanged();
            }
        }

        private Command openMenuCommand;
        public Command OpenMenuCommand
        {
            get => openMenuCommand;
            set
            {
                openMenuCommand = value;
                OnPropertyChanged();
            }
        }

        private bool isMenuOpened;
        public bool IsMenuOpened
        {
            get => isMenuOpened;
            set
            { 
                isMenuOpened = value;
                OnPropertyChanged();
            }
        }

        private string btnOpenMenuText;
        public string BtnOpenMenuText
        { 
            get => btnOpenMenuText;
            set
            { 
                btnOpenMenuText = value;
                OnPropertyChanged();
            }
        }

        public ViewModelUsers()
        {
            AddNewPositionCommand = new Command(async (obj) => await UserManagerModel.AddNewEmptyPositionAsync());
            IsMenuOpened = false;
            BtnOpenMenuText = "Добавить пользователя";
            OpenMenuCommand = new Command
            (
                (obj) =>
                {
                    IsMenuOpened = !IsMenuOpened;
                    BtnOpenMenuText = IsMenuOpened ? "Закрыть меню" : "Добавить пользователя";
                }
            );
            InitUserCreator();
        }

        public async Task InitVM()
        {
            await UserManagerModel.InitModelAsync();
            OnPropertyChanged(nameof(Positions));
            OnPropertyChanged(nameof(Users));
        }
    }
}
