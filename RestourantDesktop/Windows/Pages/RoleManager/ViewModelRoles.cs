using mvvm;
using RestourantDesktop.Windows.Pages.RoleManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    internal sealed class ViewModelRoles : NotifyPropertyChanged
    {
        public ObservableCollection<PageItem> Pages { get => RolesModel.PagesList; }

        private Command addNewPageCommand;
        public Command AddNewPageCommand
        {
            get => addNewPageCommand;
            set
            {
                addNewPageCommand = value;
                OnPropertyChanged();
            }
        }

        public ViewModelRoles()
        {
            //Создаём пустую запись с новым ID
            AddNewPageCommand = new Command
            (
                async (obj) =>
                {
                    await RolesModel.CreateNewEmptyPageAsync();
                    OnPropertyChanged(nameof(Pages));
                }
            );
        }

        public async Task InitVM()
        {
            await RolesModel.GetPagesListAsync();
            OnPropertyChanged(nameof(Pages));
        }
    }
}