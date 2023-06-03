using mvvm;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Linq;

namespace RestourantDesktop.Windows.Pages.Orders.Items
{
    internal class OrderDishItem : NotifyPropertyChanged
    {
        public DishItem Dish { get; set; }
        public string Picture { get => Dish.Pictures.FirstOrDefault()?.Adress; }
        public string Name { get => Dish.Name; }
        public int Count { get; set; }


        public OrderDishItem(DishItem item, int count)
        {
            this.Dish = item;
            this.Count = count;

            Dish.PropertyChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(Picture));
                OnPropertyChanged(nameof(Name));
            };
        }
    }
}
