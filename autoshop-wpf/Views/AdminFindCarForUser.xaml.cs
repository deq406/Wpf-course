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
    /// Логика взаимодействия для AdminFindCarForUser.xaml
    /// </summary>
    public partial class AdminFindCarForUser : Page
    {
        public AdminFindCarForUser()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                db.Requirements.Load();
                requirementsGrid.ItemsSource = db.Requirements.Local.ToBindingList();
                db.Cars.Load();
                carsGrid.ItemsSource = db.Cars.Local.ToBindingList();
            }
        }

        private void carsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (carsGrid.SelectedItem != null)
            {
                Car selectedSnk = (Car)carsGrid.SelectedItem;
                carsGrid.DataContext = selectedSnk;
            }
        }

        private void require_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (requirementsGrid.SelectedItem != null)
            {
                Requirements selectedSnk = (Requirements)requirementsGrid.SelectedItem;
                requirementsGrid.DataContext = selectedSnk;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                if (carsGrid.SelectedItem != null && requirementsGrid.SelectedItem !=null)
                {
                    Car car = (Car)carsGrid.SelectedItem;
                    Car myCar = db.Cars.Find(car.ID);

                    Requirements requirements = (Requirements)requirementsGrid.SelectedItem;
                    Requirements userRequirements = db.Requirements.Find(requirements.ID);

                    db.Users.Load();
                    User user = db.Users.Where(u => u.ID == userRequirements.UserId).First();


                    if (user.Car.Contains(myCar))
                    {
                        MessageBox.Show("Car already in your wish list", "Autoshop", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        user.Car.Add(myCar);
                        db.SaveChanges();
                        MessageBox.Show("Car is added to your wish list", "Autoshop", MessageBoxButton.OK, MessageBoxImage.Information);
                    }


                }
            }
        }
    }
}
