using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Autoshop.Models
{
    class ButtonCommands
    {
        public void CloseWindow(MainWindow window)
        {
            window.Close();
        }

        public void MinimizeWindow(MainWindow window)
        {
            window.WindowState = WindowState.Minimized;
        }

        public void MaximizeWindow(MainWindow window, Button RestoreButton, Button MaximizeButton)
        {
            window.WindowState = WindowState.Maximized;
            RestoreButton.Visibility = Visibility.Visible;
            MaximizeButton.Visibility = Visibility.Collapsed;
        }

        public void RestoreWindow(MainWindow window, Button RestoreButton, Button MaximizeButton)
        {
            window.WindowState = WindowState.Normal;
            RestoreButton.Visibility = Visibility.Collapsed;
            MaximizeButton.Visibility = Visibility.Visible;
        }

        public void DragMoveWindow(MainWindow window, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) window.DragMove();
        }

        public void isWindowMiximized(MainWindow window, Button RestoreButton, Button MaximizeButton)
        {
            WindowState state = window.WindowState;
            if (state == WindowState.Maximized)
            {
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
