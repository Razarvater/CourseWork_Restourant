using mvvm;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.RoleManager.Items
{
    internal sealed class PageItem : NotifyPropertyChanged
    {
        private int iD;
        public int ID
        {
            get => iD;
            set
            {
                iD = value;
                OnPropertyChanged();
            }
        }

        private string pageName;
        public string PageName
        {
            get => pageName;
            set
            {
                pageName = value;
                
                OnPropertyChanged();
                UpdatePage();
            }
        }
        private async Task UpdatePage()
        {
            await RolesModel.UpdatePageAsync(this);
        }

        public PageItem(int ID, string PageName)
        {
            this.iD = ID;
            this.pageName = PageName;
        }
    }
}
