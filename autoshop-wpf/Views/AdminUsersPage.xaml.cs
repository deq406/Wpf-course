using Autoshop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для AdminUsersPage.xaml
    /// </summary>
    public partial class AdminUsersPage : Page
    {
        public AdminUsersPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                db.Users.Load();
                usersGrid.ItemsSource = db.Users.Local;
            }
        }

        private void usersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersGrid.SelectedItem != null)
            {
                User user = (User)usersGrid.SelectedItem;
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User myUser = window.getCurrentUser();
                if (myUser.ID == user.ID || user.ID == 1) isAdmin.IsEnabled = false;
                else isAdmin.IsEnabled = true;

                sortingGrid.Visibility = Visibility.Collapsed;
                userGrid.Visibility = Visibility.Visible;
                userGrid.DataContext = user;
            }
        }

        private void ShowAdmins_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                usersGrid.ItemsSource = null;
                db.Users.Load();
                usersGrid.ItemsSource = db.Users.Local.Where(u => u.IsAdmin);
            }
        }

        private void RestoreSort_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                usersGrid.ItemsSource = null;
                db.Users.Load();
                usersGrid.ItemsSource = db.Users.Local;
            }
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                User user = (User)usersGrid.SelectedItem;
                User myUser = db.Users.Find(user.ID);

                //myUser.FIO = FIO.Text;
                //myUser.Mail = Mail.Text;
                //myUser.Phone = Phone.Text;
                myUser.IsAdmin = isAdmin.IsChecked.Value;

                db.SaveChanges();

                usersGrid.ItemsSource = null;
                db.Users.Load();
                usersGrid.ItemsSource = db.Users.Local.OrderBy(u => u.ID);

                sortingGrid.Visibility = Visibility.Visible;
                userGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ClearGrid_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                usersGrid.ItemsSource = null;
                db.Users.Load();
                usersGrid.ItemsSource = db.Users.Local;

                sortingGrid.Visibility = Visibility.Visible;
                userGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void RestorePass_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                User user = (User)usersGrid.SelectedItem;
                User myUser = db.Users.Find(user.ID);
                myUser.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("123456789")));

                db.SaveChanges();

                usersGrid.ItemsSource = null;
                db.Users.Load();
                usersGrid.ItemsSource = db.Users.Local.OrderBy(u => u.ID);

                sortingGrid.Visibility = Visibility.Visible;
                userGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
