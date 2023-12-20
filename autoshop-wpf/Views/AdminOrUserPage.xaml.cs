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
    /// Логика взаимодействия для AdminOrUserView.xaml
    /// </summary>
    public partial class AdminOrUserPage : Page
    {
        public AdminOrUserPage()
        {
            InitializeComponent();
        }

        private void GoToUserView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/UserPage.xaml", UriKind.Relative));
        }

        private void GoToAdminView_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/AdminPage.xaml", UriKind.Relative));
        }
    }
}
