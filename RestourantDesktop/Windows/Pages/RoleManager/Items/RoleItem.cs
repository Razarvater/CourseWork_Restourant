using mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RestourantDesktop.Windows.Pages.RoleManager.Items
{
    internal sealed class RoleItem : NotifyPropertyChanged
    {
        public int roleID;

        public string roleName;
        public string RoleName
        { 
            get => roleName;
            set
            { 
                roleName = value;
                OnPropertyChanged();

                #pragma warning disable
                ChangeRoleName();
            }
        }

        private ObservableCollection<PageRoleItem> rights;
        public ObservableCollection<PageRoleItem> Rights
        { 
            get => rights;
            set
            { 
                rights = value;
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

        private async void ChangeRoleName() => await RolesModel.ChangeRoleName(this);

        public RoleItem(int ID, string Role, IEnumerable<PageRoleItem> items)
        {
            DeleteCommand = new Command(async (obj) => await RolesModel.DeleteRoleAsync(this));

            this.Rights = new ObservableCollection<PageRoleItem>(items);

            for (int i = 0; i < Rights.Count; i++)
                Rights[i].RoleID = ID;

            this.roleID = ID;
            this.roleName = Role;
        }
    }

    internal sealed class PageRoleItem : NotifyPropertyChanged 
    {
        public int RoleID;

        public int PageID;

        public string pageName;
        public string PageName
        {
            get => pageName;
            set
            {
                pageName = value;
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

        private async void ChangeRight() => await RolesModel.ChangeRight(this);

        public PageRoleItem(int ID, string PageName, bool PageStatusForRole)
        {
            this.PageID = ID;
            this.pageName = PageName;
            this.isCan = PageStatusForRole;
        }
    }
}