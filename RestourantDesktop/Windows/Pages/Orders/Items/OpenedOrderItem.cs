using mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RestourantDesktop.Windows.Pages.Orders.Items
{
    internal class OpenedOrderItem
    {
        public int ID { get; set; }
        public int EmpID { get; set; }
        public DateTime CreateDateTime { get; private set; }
        public string TableInfo { get; private set; }
        public int CookingTime { get; private set; }
        public double Sum { get; private set; }
        public ObservableCollection<OrderDishItem> Dishes { get; private set; }

        public Command CloseOrder { get; private set; }

        public OpenedOrderItem(int ID, int EmpID, DateTime CreateDateTime, string TableInfo, int CookingTime, double Sum, IEnumerable<OrderDishItem> dishesList)
        {
            this.ID = ID;
            this.EmpID = EmpID;
            this.CreateDateTime = CreateDateTime;
            this.TableInfo = TableInfo;
            this.CookingTime = CookingTime;
            this.Sum = Sum;

            this.Dishes = new ObservableCollection<OrderDishItem>(dishesList);

            CloseOrder = new Command
            (
                async (obj) => await OrdersModel.CloseOrderAsync(this)
            );
        }
    }
}
