using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Reg_log_Kor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BD1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            log_1.Text = log_1.Tag.ToString();
        }

        public int z = 0;
        public int kol_v = 0;

        private void Bt_Reg1(object sender, RoutedEventArgs e)
        {
            Reg reg = new Reg();
            reg.Show();
            this.Hide();
        }


        public void Rem_txt (object sender, EventArgs e)
        {
            log_1 = (TextBox)sender;
            if (log_1.Text == log_1.Tag.ToString())
            {
                log_1.Text = "";
            }
        }

        public void Add_txt(object sender, EventArgs e)
        {
            log_1 = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(log_1.Text))
            {
                log_1.Text = log_1.Tag.ToString();
            }
        }

        private void Bt_show_hide(object sender, RoutedEventArgs e)
        {
            if (z == 0)
            {
                Pass_text.Text = pass_log.Password;
                Pass_text.Visibility = Visibility.Visible;
                pass_log.Visibility = Visibility.Hidden;
                z++;
            }
            else
            {
                pass_log.Password = Pass_text.Text;
                pass_log.Visibility = Visibility.Visible;
                Pass_text.Visibility= Visibility.Hidden;
                z = 0;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void Vhod_Btn(object sender, RoutedEventArgs e)
        {

            if (kol_v==2)
            {
                log_1.IsEnabled = false;
                pass_log.IsEnabled = false;
                Pass_text.IsEnabled = false;
                Osh.Text = "ввод заблокирован на 15 сек";
                await Task.Delay(15000);
                log_1.IsEnabled = true;
                pass_log.IsEnabled = true;
                Pass_text.IsEnabled = true;
                Osh.Text = "";
                kol_v = 0;
                return;
            }

            if (z==1)
            {
                pass_log.Password = Pass_text.Text;
            }

            var login = log_1.Text;

            var passsword = pass_log.Password;

            var context = new AppDbContext();

            var user = context.Users.FirstOrDefault(x => x.Login == login || x.Email == login && x.Password == passsword);

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(passsword) || login == "Введите логин")
            {
                Osh.Text = "Логин и пароль обязательны";
                log_1.BorderBrush = Brushes.Red;
                log_1.BorderThickness = new Thickness(2);
                pass_log.BorderBrush = Brushes.Red;
                pass_log.BorderThickness = new Thickness(2);
                Pass_text.BorderBrush = Brushes.Red;
                Pass_text.BorderThickness = new Thickness(2);
                return;
            }


            if (user is null)
            {
                Osh.Text = "Неверный логин";
                log_1.BorderBrush = Brushes.Red;
                log_1.BorderThickness = new Thickness(2);
                pass_log.BorderBrush = null;
                Pass_text.BorderBrush = null;
                kol_v++;
                return;

            }

            if (user.Password != passsword)
            {
                Osh.Text = "Неверный пароль";
                pass_log.BorderBrush = Brushes.Red;
                pass_log.BorderThickness = new Thickness(2);
                Pass_text.BorderBrush = Brushes.Red;
                Pass_text.BorderThickness = new Thickness(2);
                log_1.BorderBrush = null;
                kol_v++;
                return;
            }
            else
            {
                pass_log.BorderBrush = null;
                log_1.BorderBrush = null;
                Pass_text.BorderBrush= null;
                Osh.Text = "";

                Login logg = new Login(login);

                logg.Show();
                this.Hide();
            }
        }


    }
}