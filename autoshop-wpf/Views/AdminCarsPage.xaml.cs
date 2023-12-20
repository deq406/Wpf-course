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
    /// Логика взаимодействия для AdminSneakersPage.xaml
    /// </summary>
    public partial class AdminCarsPage : Page
    {
        public AdminCarsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
 
            using (UserContext db = new UserContext())
            {
                db.Cars.Load();
               
                carsGrid.ItemsSource = db.Cars.Local.ToBindingList();
            }
        }

        private void ImageSetter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (op.ShowDialog() == true && carsGrid.SelectedItem != null)
            {
                Car selectedCar = (Car)carGrid.DataContext;
                selectedCar.Image = File.ReadAllBytes(op.FileName);
                carGrid.DataContext = null;
                carGrid.DataContext = selectedCar;
            }
        }

        private void carsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (carsGrid.SelectedItem != null)
            {
                Car selectedCar = (Car)carsGrid.SelectedItem;
                carGrid.DataContext = selectedCar;
            }
        }

        private void NewCar_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (op.ShowDialog() == true)
                {
                    Car car = new Car();
                    car.Image = File.ReadAllBytes(op.FileName);

                    car.Model = op.FileName.Substring(op.FileName.Length - 10);
                    car.Brand = op.FileName.Substring(op.FileName.Length - 10);
                    car.Price = 0;

                    db.Cars.Add(car);
                    db.SaveChanges();
                    carsGrid.ItemsSource = null;
                    db.Cars.Load();
                    carsGrid.ItemsSource = db.Cars.Local.ToBindingList().OrderBy(u => u.ID);
                    carGrid.DataContext = null;
                }
            }
        }

        private void SaveCar_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                if (carsGrid.SelectedItem != null && checkTextBoxes())
                {
                    Car car = (Car)carGrid.DataContext;
                    Car myCar = db.Cars.Find(car.ID);

                    myCar.Brand = car.Brand;
                    myCar.Model = car.Model;
                    myCar.Year = car.Year;
                    myCar.Mileage = car.Mileage;
                    myCar.EngineType = car.EngineType;
                    myCar.Price = car.Price;
                    myCar.Image = car.Image;

                    db.SaveChanges();
                    carsGrid.ItemsSource = null;
                    db.Cars.Load();
                    carsGrid.ItemsSource = db.Cars.Local.ToBindingList().OrderBy(u => u.ID);
                    carGrid.DataContext = null;
                }
            }
        }

        private void DelCar_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                if (carsGrid.SelectedItem != null)
                {
                    Car car = (Car)carGrid.DataContext;
                    Car myCar = db.Cars.Find(car.ID);

                    db.Cars.Remove(myCar);
                    db.SaveChanges();

                    carsGrid.ItemsSource = null;
                    db.Cars.Load();
                    carsGrid.ItemsSource = db.Cars.Local.ToBindingList().OrderBy(u => u.ID);

                    carGrid.DataContext = null;
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

            if(Year.Text == "")
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
