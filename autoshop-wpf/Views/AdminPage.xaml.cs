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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();

            AdminFrame.Navigate(new AdminCarsPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            window.username.Text = "Autoshop - " + window.getCurrentUser().Login;
        }

        private void Sneakers_Click(object sender, RoutedEventArgs e)
        {
            AdminFrame.NavigationService.Navigate(new Uri("Views/AdminCarsPage.xaml", UriKind.Relative));
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            AdminFrame.NavigationService.Navigate(new Uri("Views/AdminUsersPage.xaml", UriKind.Relative));
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            AdminFrame.NavigationService.Navigate(new Uri("Views/AdminOrdersPage.xaml", UriKind.Relative));
        }

        private void FindCar_Click(object sender, RoutedEventArgs e)
        {
            AdminFrame.NavigationService.Navigate(new Uri("Views/AdminFindCarForUser.xaml", UriKind.Relative));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/LogInPage.xaml", UriKind.Relative));
        }

        private void GoToUser_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/UserPage.xaml", UriKind.Relative));
        }
    }
}
