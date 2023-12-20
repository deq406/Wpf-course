using Autoshop.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            User user = window.getCurrentUser();
            ID.Text = "Profile #" + user.ID;
            profileGrid.DataContext = user;
            if (user.IsAdmin)
            {
                Login.Foreground = Brushes.Green;
            }
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User user = window.getCurrentUser();
                if (checkTextBoxes())
                {
                    User curUser = db.Users.Find(user.ID);
                    curUser.FIO = FIO.Text;
                    curUser.Mail = Mail.Text;
                    curUser.Phone = Phone.Text;
                    db.SaveChanges();
                    window.setCurrentUser(curUser);
                }
            }
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User user = window.getCurrentUser();
                if (oldPassMatch())
                {
                    if (checkPassBoxes())
                    {
                        if (passMatch())
                        {
                            User curUser = db.Users.Find(user.ID);
                            curUser.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(newPass.Password)));
                            db.SaveChanges();
                            currentPass.Password = null;
                            newPass.Password = null;
                            confPass.Password = null;
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

        public bool checkTextBoxes()
        {
            bool check = true;
            clearTextBoxes();
            Regex fioCheck = new Regex(@"^[A-Z][a-z]*(\s[A-Z][a-z]*)+${5,30}");
            Regex mailCheck = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex phoneCheck = new Regex(@"\(?\d{2,5}\)?-? *\d{3}-? *-?\d{4}");
            //(xxx)xxxxxxx
            //(xxx) xxxxxxx
            //(xxx)xxx - xxxx
            //(xxx) xxx - xxxx
            //xxxxxxxxxx
            //xxx - xxx - xxxxx

            if (FIO.Text == "")
            {
                SetError(FIO, "Required field");
                check = false;
            }
            else if (!fioCheck.IsMatch(FIO.Text))
            {
                SetError(FIO, "Invalid input format");
                check = false;
            }

            if (Mail.Text == "")
            {
                SetError(Mail, "Required field");
                check = false;
            }
            else if (!mailCheck.IsMatch(Mail.Text))
            {
                SetError(Mail, "Invalid input format");
                check = false;
            }

            if (Phone.Text == "")
            {
                SetError(Phone, "Required field");
                check = false;
            }
            else if (!phoneCheck.IsMatch(Phone.Text))
            {
                SetError(Phone, "Invalid input format");
                check = false;
            }

            return check;
        }

        public void clearTextBoxes()
        {
            SetValid(FIO);
            SetValid(Mail);
            SetValid(Phone);
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

        public bool checkPassBoxes()
        {
            bool check = true;
            clearPassBoxes();
            Regex passCheck = new Regex(@"[A-Za-z0-9]{6,20}");

            if (newPass.Password == "")
            {
                SetPassError(newPass, "Required field");
                check = false;
            }
            else if (!passCheck.IsMatch(newPass.Password))
            {
                SetPassError(newPass, "Invalid input format");
                check = false;
            }
            else SetPassValid(newPass);

            return check;
        }

        public bool oldPassMatch()
        {
            bool check = true;
            MainWindow window = Window.GetWindow(this) as MainWindow;
            User user = window.getCurrentUser();

            if (currentPass.Password == "")
            {
                SetPassError(currentPass, "Required field");
                check = false;
            }
            else if (Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(currentPass.Password))) != user.Password)
            {
                SetPassError(currentPass, "Password mismatch");
                check = false;
            }
            else SetPassValid(currentPass);

            return check;
        }

        public bool passMatch()
        {
            bool check = true;

            if (confPass.Password == "")
            {
                SetPassError(confPass, "Required field");
                check = false;
            }
            else if (newPass.Password != confPass.Password)
            {
                SetPassError(confPass, "Password mismatch");
                check = false;
            }
            else SetPassValid(confPass);

            return check;
        }

        public void clearPassBoxes()
        {
            SetPassValid(currentPass);
            SetPassValid(newPass);
            SetPassValid(confPass);
        }
    }
}
