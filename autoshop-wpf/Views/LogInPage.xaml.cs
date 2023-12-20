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
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            window.username.Text = "Autoshop";
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    loginLogin.ToolTip = null;
                    loginLogin.Background = null;

                    db.Users.Load();
                    User myUser = db.Users.Local.ToBindingList().Where(u => u.Login == loginLogin.Text).Select(s => s).First();
                    if (myUser.Password == Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(loginPass.Password))))
                    {
                        loginPass.ToolTip = null;
                        loginPass.Background = null;

                        MainWindow window = Window.GetWindow(this) as MainWindow;
                        window.setCurrentUser(myUser);

                        if (myUser.IsAdmin)
                        {
                            this.NavigationService.Navigate(new Uri("Views/AdminOrUserPage.xaml", UriKind.Relative));
                        }
                        else
                        {
                            this.NavigationService.Navigate(new Uri("Views/UserPage.xaml", UriKind.Relative));
                        }
                    }
                    else
                    {
                        ToolTip tooltip;
                        if (myUser.Password == Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("123456789"))))
                        {
                            tooltip = new ToolTip { Content = "Your restored password - 123456789" };
                        }
                        else
                        {
                            tooltip = new ToolTip { Content = "Invalid password" };
                        }
                        loginPass.ToolTip = tooltip;
                        loginPass.Background = Brushes.DarkOrange;
                    }
                }
                catch
                {
                    ToolTip tooltip = new ToolTip { Content = "This user does not exist" };
                    loginLogin.ToolTip = tooltip;
                    loginLogin.Background = Brushes.DarkOrange;

                    loginPass.ToolTip = null;
                    loginPass.Background = null;
                }
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/RegisterPage.xaml", UriKind.Relative));
        }
    }
}
