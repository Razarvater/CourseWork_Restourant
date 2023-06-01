using mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.Orders
{
    internal class ViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<OpenedOrderItem> OrderList { get => OrdersModel.OrderList; }
        public async Task InitModel()
        { 
            await OrdersModel.InitOrderList();
            OnPropertyChanged("OrderList");
        }
    }
}
