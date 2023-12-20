using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Autoshop.Models;
using Autoshop.Views;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Autoshop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LogInPage());

            ToggleBaseColour(false);

            try
            {
                using (UserContext db = new UserContext())
                {
                    var users = db.Users;
                    User myUser = users.Where(u => u.Login == "admin").Select(s => s).First();
                }
            }
            catch
            {
                using (UserContext db = new UserContext())
                {
                    User user1 = new User { Login = "admin" };
                    user1.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("admin")));
                    user1.IsAdmin = true;

                    db.Users.Add(user1);
                    db.SaveChanges();
                }
            }

            try
            {
                using (UserContext db = new UserContext())
                {
                    var users = db.Users;
                    User myUser = users.Where(u => u.Login == "user").Select(s => s).First();
                }
            }
            catch
            {
                using (UserContext db = new UserContext())
                {
                    User user2 = new User { Login = "user" };
                    user2.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("user")));

                    db.Users.Add(user2);
                    db.SaveChanges();
                }
            }
        }

        User currentUser = new User();

        public void setCurrentUser(User user)
        {
            currentUser = user;
        }

        public User getCurrentUser()
        {
            return currentUser;
        }

        Car currentSneaker = new Car();

        public void setCurrentSneaker(Car sneaker)
        {
            currentSneaker = sneaker;
        }

        public Car getCurrentSneaker()
        {
            return currentSneaker;
        }

        public bool isThemeDark = true;

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonCommands buttonCommands = new ButtonCommands();
            buttonCommands.MinimizeWindow(Window.GetWindow(this) as MainWindow);
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonCommands buttonCommands = new ButtonCommands();
            buttonCommands.MaximizeWindow(Window.GetWindow(this) as MainWindow, RestoreButton, MaximizeButton);
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonCommands buttonCommands = new ButtonCommands();
            buttonCommands.RestoreWindow(Window.GetWindow(this) as MainWindow, RestoreButton, MaximizeButton);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonCommands buttonCommands = new ButtonCommands();
            buttonCommands.CloseWindow(Window.GetWindow(this) as MainWindow);
        }

        private void ThemeChanger_Click(object sender, RoutedEventArgs e)
        {
            ToggleBaseColour(isThemeDark);
            isThemeDark = !isThemeDark;
        }

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private void ToggleBaseColour(bool isThemeDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            /* IBaseTheme baseTheme = isThemeDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();*/
            IBaseTheme baseTheme = new MaterialDesignDarkTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonCommands buttonCommands = new ButtonCommands();
            buttonCommands.DragMoveWindow(Window.GetWindow(this) as MainWindow, e);
        }
    }
}
