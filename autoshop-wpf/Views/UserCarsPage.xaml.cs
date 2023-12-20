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
    /// Логика взаимодействия для UserSneakersPage.xaml
    /// </summary>
    public partial class UserCarsPage : Page
    {
        public UserCarsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                db.Cars.Load();
                sneakersGrid.ItemsSource = db.Cars.Local.ToBindingList();
            }
        }

        private void sneakersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Car sneaker = (Car)sneakersGrid.SelectedItem;
            MainWindow window = Window.GetWindow(this) as MainWindow;
            window.setCurrentSneaker(sneaker);
            UserNewOrderPage userNewOrderPage = new UserNewOrderPage();
            NavigationService.Navigate(userNewOrderPage);
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                Button cmd = (Button)sender;
                if (cmd.DataContext is Car)
                {
                    MainWindow window = Window.GetWindow(this) as MainWindow;
                    User usr = window.getCurrentUser();
                    Car snk = (Car)cmd.DataContext;

                    db.Users.Load();
                    User user = db.Users.Include(u => u.Car).First(u => u.ID == usr.ID);

                    db.Cars.Load();
                    Car car = db.Cars.Include(u => u.User).First(u => u.ID == snk.ID);

                    if (user.Car.Contains(car))
                    {
                        MessageBox.Show("Car already in your wish list", "Autoshop", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        user.Car.Add(car);
                        db.SaveChanges();
                        MessageBox.Show("Car is added to your wish list", "Autoshop", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
