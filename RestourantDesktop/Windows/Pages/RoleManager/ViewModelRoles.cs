﻿using mvvm;
using RestourantDesktop.Windows.Pages.RoleManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    internal sealed class ViewModelRoles : NotifyPropertyChanged
    {
        public ObservableCollection<RoleItem> RolesList { get => RolesModel.RoleList;}    
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
        
        private Command addNewRoleCommand;
        public Command AddNewRoleCommand
        {
            get => addNewRoleCommand;
            set
            {
                addNewRoleCommand = value;
                OnPropertyChanged();
            }
        }


        //TODO: Удаление ролей
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

            AddNewRoleCommand = new Command
            (
                async (obj) =>
                {
                    await RolesModel.CreateNewEmptyRole();
                    OnPropertyChanged(nameof(RolesList));
                }
            );
        }

        public async Task DeletePageItem(PageItem item)
        {
            //TODO: добавить предупреждение пользователю о каскадном удалении 
            await RolesModel.DeletePageAsync(item);
            OnPropertyChanged(nameof(Pages));
        }

        public async Task InitVM()
        {
            await RolesModel.InitModel();
            OnPropertyChanged(nameof(Pages));
            OnPropertyChanged(nameof(RolesList));
        }
    }
}