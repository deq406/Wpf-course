using Microsoft.Win32;
using Autoshop.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Autoshop.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateRequirement.xaml
    /// </summary>
    public partial class CreateRequirement : Page
    {
        public CreateRequirement()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User user = window.getCurrentUser();
                db.Requirements.Load();
                sneakersGrid.ItemsSource = db.Requirements.Local.ToBindingList().Where(r => r.UserId == user.ID).Select(s => s);
            }
        }
        private void ImageSetter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (op.ShowDialog() == true && sneakersGrid.SelectedItem != null)
            {
                Requirements selectedSnk = (Requirements)sneakerGrid.DataContext;
                selectedSnk.Image = File.ReadAllBytes(op.FileName);
                sneakerGrid.DataContext = null;
                sneakerGrid.DataContext = selectedSnk;
            }
        }

        private void sneakersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sneakersGrid.SelectedItem != null)
            {
                Requirements selectedSnk = (Requirements)sneakersGrid.SelectedItem;
                sneakerGrid.DataContext = selectedSnk;
            }
        }

        private void NewSneaker_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User user = window.getCurrentUser();
                if (op.ShowDialog() == true)
                {
                    Requirements sneaker = new Requirements();
                    sneaker.Image = File.ReadAllBytes(op.FileName);

                    sneaker.Model = op.FileName.Substring(op.FileName.Length - 10);
                    sneaker.Brand = op.FileName.Substring(op.FileName.Length - 10);
                    sneaker.Price = 0;
                    sneaker.UserId = user.ID;

                    db.Requirements.Add(sneaker);
                    db.SaveChanges();
                    sneakersGrid.ItemsSource = null;
                    db.Requirements.Load();
                    sneakersGrid.ItemsSource = db.Requirements.Local.ToBindingList().Where(r => r.UserId == user.ID).Select(s => s);
                    sneakerGrid.DataContext = null;
                }
            }
        }

        private void SaveSneaker_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User user = window.getCurrentUser();

                if (sneakersGrid.SelectedItem != null && checkTextBoxes())
                {
                    Requirements car = (Requirements)sneakerGrid.DataContext;
                    Requirements myCar = db.Requirements.Find(car.ID);

                    myCar.Brand = car.Brand;
                    myCar.Model = car.Model;
                    myCar.Year = car.Year;
                    myCar.Mileage = car.Mileage;
                    myCar.EngineType = car.EngineType;
                    myCar.Price = car.Price;
                    myCar.Image = car.Image;

                    db.SaveChanges();
                    sneakersGrid.ItemsSource = null;
                    db.Requirements.Load();
                    sneakersGrid.ItemsSource = db.Requirements.Local.ToBindingList().Where(r => r.UserId == user.ID).Select(s => s);
                    sneakerGrid.DataContext = null;
                }
            }
        }

        private void DelSneaker_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                MainWindow window = Window.GetWindow(this) as MainWindow;
                User user = window.getCurrentUser();
                if (sneakersGrid.SelectedItem != null)
                {
                    Requirements sneaker = (Requirements)sneakerGrid.DataContext;
                    Requirements mySneaker = db.Requirements.Find(sneaker.ID);

                    db.Requirements.Remove(mySneaker);
                    db.SaveChanges();

                    sneakersGrid.ItemsSource = null;
                    db.Requirements.Load();
                    sneakersGrid.ItemsSource = db.Requirements.Local.ToBindingList().Where(r => r.UserId == user.ID).Select(s => s);

                    sneakerGrid.DataContext = null;
                }
            }
        }

        public void SetError(TextBox textBox, string errorName)
        {
            ToolTip tooltip = new ToolTip { Content = errorName };
            textBox.ToolTip = tooltip;
            textBox.Background = Brushes.Yellow;
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
            Regex nameCheck = new Regex(@"[A-Za-z][A-Za-z0-9._]{3,20}");

            if (Brand.Text == "")
            {
                SetError(Brand, "Required field");
                check = false;
            }
            else if (!nameCheck.IsMatch(Brand.Text))
            {
                SetError(Brand, "Invalid input format");
                check = false;
            }

            if (Model.Text == "")
            {
                SetError(Model, "Required field");
                check = false;
            }
            else if (!nameCheck.IsMatch(Model.Text))
            {
                SetError(Model, "Invalid input format");
                check = false;
            }

            if (Price.Text == "")
            {
                SetError(Price, "Required field");
                check = false;
            }

            if (Year.Text == "")
            {
                SetError(Year, "Required field");
                check = false;
            }

            return check;
        }

        public void clearTextBoxes()
        {
            SetValid(Brand);
            SetValid(Model);
            SetValid(Price);
        }
    }
}
