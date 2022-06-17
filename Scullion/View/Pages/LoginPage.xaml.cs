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
using System.Windows.Threading;

namespace Scullion.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        public DispatcherTimer dt;
        
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.Context.User
                .FirstOrDefault(p => p.UserLogin == TBoxLogin.Text && p.UserPassword == PBoxPassword.Password);

            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                NavigationService.Navigate(new ProductPage());
            }
            else
            {
                dt = new DispatcherTimer();
                BtnLogin.IsEnabled = false;
                dt.Interval = new TimeSpan(0, 0, 10);
                dt.Tick += new EventHandler(dispatcherTimer);
                dt.Start();
                MessageBox.Show("Пользователь с такими данными не найден", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dispatcherTimer(object sender, EventArgs e)
        {
            BtnLogin.IsEnabled = true;
            dt.Stop();
        }
    }
}
