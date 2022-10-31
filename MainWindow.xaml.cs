using System.Windows;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager userManager;
        TravelManager travelManager;
        public MainWindow()
        {
            InitializeComponent();
            userManager = new();
            travelManager = new();
            Admin admin = new(travelManager, userManager);
            admin.IUser("admin", "password", Countries.Sweden);
            userManager.AddUser(admin);
            User user = new();
            user.IUser("Gandalf", "password", Countries.Sweden);
            userManager.AddUser(user);
            User user2 = new();
            user.IUser("qwe", "asd", Countries.Sweden);
            userManager.AddUser(user);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);
            registerWindow.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (userManager.SignInUser(tbUsername.Text, tbPassword.Password))
            {
                tbPassword.Clear();
                TravelsWindow travelsWindow = new(userManager, travelManager);
                travelsWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrecto");
                tbPassword.Clear();
            }
        }
    }
}
