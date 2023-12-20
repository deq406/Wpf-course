using Autoshop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Autoshop.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminOrdersPage.xaml
    /// </summary>
    public partial class AdminOrdersPage : Page
    {
        public AdminOrdersPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                db.Orders.Load();
                ordersGrid.ItemsSource = db.Orders.Local.ToBindingList();
            }
            statuses.ItemsSource = new List<string> { "In processing", "Accepted", "Rejected", "Sent", "Awaiting receipt", "Delivered" };
            statusesSort.ItemsSource = new List<string> { "In processing", "Accepted", "Rejected", "Sent", "Awaiting receipt", "Delivered" };
        }

        private void ordersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ordersGrid.SelectedItem != null)
            {
                orderGrid.Visibility = Visibility.Collapsed;
                selectedGrid.Visibility = Visibility.Visible;
                Order order = (Order)ordersGrid.SelectedItem;
                selectedOrderGrid.DataContext = order;
                statuses.SelectedItem = order.Status;
            }
        }

        private void SaveSneaker_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                Order order = (Order)ordersGrid.SelectedItem;
                Order myOrder = db.Orders.Find(order.ID);

                myOrder.Status = statuses.SelectedItem.ToString();

                db.SaveChanges();

                ordersGrid.ItemsSource = null;
                db.Orders.Load();
                ordersGrid.ItemsSource = db.Orders.Local.ToBindingList().OrderBy(u => u.ID);

                selectedGrid.Visibility = Visibility.Collapsed;
                orderGrid.Visibility = Visibility.Visible;
            }
        }

        private void ClearGrid_Click(object sender, RoutedEventArgs e)
        {
            selectedGrid.Visibility = Visibility.Collapsed;
            orderGrid.Visibility = Visibility.Visible;
        }

        private void HideDelivered_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                ordersGrid.ItemsSource = null;
                db.Orders.Load();
                ordersGrid.ItemsSource = db.Orders.Local.ToBindingList().Where(u => u.Status != "Delivered");
            }
        }

        private void RestoreSort_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                statusesSort.SelectedItem = null;
                ordersGrid.ItemsSource = null;
                db.Orders.Load();
                ordersGrid.ItemsSource = db.Orders.Local.ToBindingList();
            }
        }

        private void statusesSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (statusesSort.SelectedItem != null)
            {
                using (UserContext db = new UserContext())
                {
                    ordersGrid.ItemsSource = null;
                    db.Orders.Load();
                    ordersGrid.ItemsSource = db.Orders.Local.ToBindingList().Where(u => u.Status == statusesSort.SelectedItem.ToString());
                }
            }
        }
    }
}
