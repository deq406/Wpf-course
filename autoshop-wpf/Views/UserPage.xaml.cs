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
    /// Логика взаимодействия для SneakersPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            UserFrame.Navigate(new UserCarsPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            window.username.Text = "Autoshop - " + window.getCurrentUser().Login;
            if (window.getCurrentUser().IsAdmin)
            {
                gotoAdmin.Visibility = Visibility.Visible;
            }
        }

        private void Sneakers_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.NavigationService.Navigate(new Uri("Views/UserCarsPage.xaml", UriKind.Relative));
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.NavigationService.Navigate(new Uri("Views/UserProfilePage.xaml", UriKind.Relative));
        }

        private void Likes_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.NavigationService.Navigate(new Uri("Views/UserLikesPage.xaml", UriKind.Relative));
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.NavigationService.Navigate(new Uri("Views/UserOrdersPage.xaml", UriKind.Relative));
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.NavigationService.Navigate(new Uri("Views/CreateRequirement.xaml", UriKind.Relative));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/LogInPage.xaml", UriKind.Relative));
        }

        private void GoToAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/AdminPage.xaml", UriKind.Relative));
        }
    }
}
