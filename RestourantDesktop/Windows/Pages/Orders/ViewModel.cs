using mvvm;
using RestourantDesktop.Windows.Pages.Orders.Items;
using RestourantDesktop.Windows.Pages.ProductDishesManager;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.Orders
{
    internal class ViewModel : NotifyPropertyChanged
    {
        public Command AddDish { get; set; }
        public Command CreateOrder { get; set; }
        public ObservableCollection<SelecteDish> Dishes { get; set; }

        private string info;
        public string Info 
        { 
            get => info;
            set
            { 
                info = value;
                OnPropertyChanged();
            }
        }
        public int Time
        {
            get
            {
                int result = 0;
                foreach (var item in Dishes)
                {
                    //Количество поваров которые будут заниматься одним блюдом - 5
                    //Значение взято из головы, в реале зависит от оборудования кухни
                    //Потенциально можно внедрить систему учёта занятости поваров, и расчитывать уже на их основе + ещё учёт специализации поваров
                    result += (item.SelectedDish?.cookingTime ?? 0) * (int)Math.Ceiling(item.Count / 5.0 );
                }

                return result;
            }
        }

        public double Cost
        {
            get
            {
                double result = 0;
                foreach (var item in Dishes)
                    result += (item.SelectedDish?.Cost ?? 0 ) * item.Count;

                return result;
            }
        }

        public ObservableCollection<OpenedOrderItem> OrderList { get => OrdersModel.OrderList; }

        public ViewModel()
        {
            Dishes = new ObservableCollection<SelecteDish>();
            AddDish = new Command
            (
                (obj) => 
                {
                    SelecteDish item = new SelecteDish();
                    item.PropertyChanged += (sender, e) =>
                    {
                        OnPropertyChanged(nameof(Cost));
                        OnPropertyChanged(nameof(Time));
                    };
                    item.RemoveDish = new Command((o) => Dishes.Remove(item));
                    Dishes.Add(item);
                }
            );
            CreateOrder = new Command
            (
                async (obj) =>
                {
                    await OrdersModel.CreateOpenedOrder(Dishes, int.Parse(UserController.UserController.AuthorizedUser.UserID), Info, Time, Cost);
                    Dishes.Clear();
                    Info = string.Empty;
                    OnPropertyChanged(nameof(Cost));
                    OnPropertyChanged(nameof(Time));
                }
            );

            DishesModel.DishChanged += (sender, e) =>
            {
                foreach (var item in Dishes)
                {
                    var temp = item.Dishes;
                    var temp2 = item.SelectedDish;
                    item.Dishes = null;
                    item.OnPropertyChanged("Dishes");
                    item.Dishes = temp;
                    item.OnPropertyChanged("Dishes");

                    item.selectedDish = null;
                    item.OnPropertyChanged("SelectedDish");
                    item.selectedDish = temp2;
                    item.OnPropertyChanged("SelectedDish");
                    item.OnPropertyChanged("Picture");

                }
            };
        }

        public async Task InitModel()
        {
            await DishesModel.InitModel();
            await OrdersModel.InitOrderList();
            OnPropertyChanged("OrderList");
        }
    }
}
