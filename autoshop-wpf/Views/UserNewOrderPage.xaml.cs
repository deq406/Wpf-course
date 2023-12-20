using Autoshop.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для UserNewOrderPage.xaml
    /// </summary>
    public partial class UserNewOrderPage : Page
    {
        public UserNewOrderPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            Car sneaker = window.getCurrentSneaker();
            orderGrid.DataContext = sneaker;
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (isEmpty())
            {
                using (UserContext db = new UserContext())
                {
                    Order order = new Order();
                    order.Comment = Comment.Text;
                    order.Adress = Adress.Text;
                    order.Status = "In processing";

                    MainWindow window = Window.GetWindow(this) as MainWindow;
                    Car sneaker = window.getCurrentSneaker();
                    User user = window.getCurrentUser();

                    order.CarId = sneaker.ID;
                    order.Brand = sneaker.Brand;
                    order.Model = sneaker.Model;
                    order.Year = sneaker.Year;
                    order.EngineType = sneaker.EngineType;
                    order.Mileage = sneaker.Mileage;
                    order.Price = sneaker.Price;
                    order.Image = sneaker.Image;

                    var users = db.Users;
                    User myUser = users.Where(u => u.ID == user.ID).Select(s => s).First();
                    order.UserId = user.ID;
                    order.User = myUser;

                    db.Orders.Add(order);
                    db.SaveChanges();

                    UserOrdersPage userOrdersPage = new UserOrdersPage();
                    NavigationService.Navigate(userOrdersPage);
                }
            }
        }

        public bool isEmpty()
        {
            if (Adress.Text == "")
            {
                ToolTip tooltip = new ToolTip { Content = "Required field" };
                Adress.ToolTip = tooltip;
                Adress.Background = Brushes.DarkOrange;
                return false;
            }
            else
            {
                Adress.ToolTip = null;
                Adress.Background = null;
                return true;
            }
        }
    }
}
