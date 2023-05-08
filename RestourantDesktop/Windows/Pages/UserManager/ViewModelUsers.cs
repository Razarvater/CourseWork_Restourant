using mvvm;
using RestourantDesktop.Windows.Pages.UserManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    internal class ViewModelUsers : NotifyPropertyChanged
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

        public ViewModelUsers()
        {
            AddNewPositionCommand = new Command(async (obj) => await UserManagerModel.AddNewEmptyPositionAsync());
        }

        public async Task InitVM()
        {
            await UserManagerModel.InitModel();
            OnPropertyChanged(nameof(Positions));
            OnPropertyChanged(nameof(Users));
        }
    }
}
