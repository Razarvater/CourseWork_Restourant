using mvvm;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.RoleManager.Items
{
    internal sealed class PageItem : NotifyPropertyChanged
    {
        private int iD;
        /// <summary>
        /// ID страницы
        /// </summary>
        public int ID
        {
            get => iD;
            set
            {
                iD = value;
                OnPropertyChanged();
            }
        }

        public string pageName;
        /// <summary>
        /// Имя страницы
        /// </summary>
        public string PageName
        {
            get => pageName;
            set
            {
                pageName = value;
                
                OnPropertyChanged();

                #pragma warning disable
                UpdatePage();
            }
        }

        private Command deletePageCommand;
        public Command DeletePageCommand
        {
            get => deletePageCommand;
            set
            {
                deletePageCommand = value;
                OnPropertyChanged();
            }
        }

        private async Task UpdatePage() => await RolesModel.UpdatePageAsync(this);

        /// <summary>
        /// Создане нового PageItem
        /// </summary>
        /// <param name="ID">ID страницы</param>
        /// <param name="PageName">Имя страницы</param>
        ///  <param name="deleteAction">Action для удаления элемента</param>
        public PageItem(int ID, string PageName)
        {
            this.iD = ID;
            this.pageName = PageName;
            DeletePageCommand = new Command(async (obj) => await RolesModel.DeletePageAsync(this));
        }
    }
}