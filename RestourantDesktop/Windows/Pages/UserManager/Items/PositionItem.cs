using mvvm;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.UserManager.Items
{
    internal class PositionItem : NotifyPropertyChanged
    {
        private int iD;
        /// <summary>
        /// ID должности
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

        public string positionName;
        /// <summary>
        /// Имя должности
        /// </summary>
        public string PositionName
        {
            get => positionName;
            set
            {
                positionName = value;

                OnPropertyChanged();

                #pragma warning disable
                UpdateSalary();
            }
        }

        public double salary;
        public double Salary
        { 
            get => salary;
            set
            {
                salary = value;

                OnPropertyChanged();

                #pragma warning disable
                UpdateSalary();
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

        private async Task UpdateSalary() => await UserManagerModel.UpdatePositionAsync(this);

        /// <summary>
        /// Создание нового PositionItem
        /// </summary>
        /// <param name="ID">ID страницы</param>
        /// <param name="PositionName">Название должности</param>
        ///  <param name="deleteAction">Action для удаления элемента</param>
        public PositionItem(int ID, string PositionName, double Salary)
        {
            this.iD = ID;
            this.positionName = PositionName;
            this.salary = Salary;
            DeletePageCommand = new Command(async (obj) => await UserManagerModel.DeletePositionAsync(this));
        }

        public override string ToString() => PositionName;
    }
}