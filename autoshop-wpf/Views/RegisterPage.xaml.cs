using Autoshop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            window.username.Text = "Autoshop";
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    db.Users.Load();
                    User myUser = db.Users.Local.ToBindingList().Where(u => u.Login == regLogin.Text).Select(s => s).First();
                    clearTextBoxes();
                    SetError(regLogin, "This user already exists");
                }
                catch
                {
                    SetValid(regLogin);
                    if (checkTextBoxes())
                    {
                        if (passMatch())
                        {
                            User user = new User();
                            user.Login = regLogin.Text;
                            user.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(regPass.Password)));
                            user.FIO = regFIO.Text;
                            user.Mail = regMail.Text;
                            user.Phone = regPhone.Text;

                            db.Users.Add(user);
                            db.SaveChanges();

                            MessageBox.Show("Registration OK", "Autoshop", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.NavigationService.Navigate(new Uri("Views/LogInPage.xaml", UriKind.Relative));
                        }
                    }
                }
            }
        }

        public void SetError(TextBox textBox, string errorName)
        {
            ToolTip tooltip = new ToolTip { Content = errorName };
            textBox.ToolTip = tooltip;
            textBox.Background = Brushes.DarkOrange;
        }

        public void SetValid(TextBox textBox)
        {
            textBox.ToolTip = null;
            textBox.Background = null;
        }

        public void SetPassError(PasswordBox passwordBox, string errorName)
        {
            ToolTip tooltip = new ToolTip { Content = errorName };
            passwordBox.ToolTip = tooltip;
            passwordBox.Background = Brushes.DarkOrange;
        }

        public void SetPassValid(PasswordBox passwordBox)
        {
            passwordBox.ToolTip = null;
            passwordBox.Background = null;
        }

        public bool checkTextBoxes()
        {
            bool check = true;
            clearTextBoxes();
            Regex loginCheck = new Regex(@"[A-Za-z][A-Za-z0-9._]{4,20}");
            Regex fioCheck = new Regex(@"^[A-Z][a-z]*(\s[A-Z][a-z]*)+${5,30}");
            Regex mailCheck = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex phoneCheck = new Regex(@"\(?\d{2,5}\)?-? *\d{3}-? *-?\d{4}");
            Regex passCheck = new Regex(@"[A-Za-z0-9]{6,20}");
            //(xxx)xxxxxxx
            //(xxx) xxxxxxx
            //(xxx)xxx - xxxx
            //(xxx) xxx - xxxx
            //xxxxxxxxxx
            //xxx - xxx - xxxxx

            if (regLogin.Text == "")
            {
                SetError(regLogin, "Required field");
                check = false;
            }
            else if (!loginCheck.IsMatch(regLogin.Text))
            {
                SetError(regLogin, "Invalid input format");
                check = false;
            }

            if (regFIO.Text == "")
            {
                SetError(regFIO, "Required field");
                check = false;
            }
            else if (!fioCheck.IsMatch(regFIO.Text))
            {
                SetError(regFIO, "Invalid input format");
                check = false;
            }

            if (regMail.Text == "")
            {
                SetError(regMail, "Required field");
                check = false;
            }
            else if (!mailCheck.IsMatch(regMail.Text))
            {
                SetError(regMail, "Invalid input format");
                check = false;
            }

            if (regPhone.Text == "")
            {
                SetError(regPhone, "Required field");
                check = false;
            }
            else if (!phoneCheck.IsMatch(regPhone.Text))
            {
                SetError(regPhone, "Invalid input format");
                check = false;
            }

            if (regPass.Password == "")
            {
                SetPassError(regPass, "Required field");
                check = false;
            }
            else if (!passCheck.IsMatch(regPass.Password))
            {
                SetPassError(regPass, "Invalid input format");
                check = false;
            }
            else SetPassValid(regPass);

            return check;
        }

        public void clearTextBoxes()
        {
            SetValid(regLogin);
            SetValid(regFIO);
            SetValid(regMail);
            SetValid(regPhone);
        }

        public bool passMatch()
        {
            bool check = true;

            if (regPassConf.Password == "")
            {
                SetPassError(regPassConf, "Required field");
                check = false;
            }
            else if (regPass.Password != regPassConf.Password)
            {
                SetPassError(regPassConf, "Password mismatch");
                check = false;
            }
            else SetPassValid(regPassConf);

            return check;
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/LogInPage.xaml", UriKind.Relative));
        }
    }
}
