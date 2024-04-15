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
using System.Windows.Shapes;

namespace Reg_log_Kor
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        public Reg()
        {
            InitializeComponent();
        }

        private List<char> specsum = new List<char>
        {
            '+','!','-','=',',','*','/','.',';','"','<', '>', '?', '~', '#', '$', '_'
        };

        private List<char> maill = new List<char>
        {
            '@'
        };
        


        private void Bt_back(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Reg_Btn(object sender, RoutedEventArgs e)
        {
            var login = LogBox.Text;

            var pass = Pass_reg1.Password;

            var pass1 = Pass_reg2.Password;

            var mail = Email_Box.Text;

            var context = new AppDbContext();

            var user_exists = context.Users.FirstOrDefault(x => x.Login == login);
            if (user_exists is not null)
            {
                Osh_reg.Text = ("Такой пользователь уже зарегестрирован");
                return;
            }
            if ((LogBox.Text).Length>2 & (LogBox.Text).Length < 16)
            {
                LogBox.BorderBrush = null;
                if ((Pass_reg1.Password).Length > 5 & (Pass_reg1.Password).Length < 16)
                {
                    Pass_reg1.BorderBrush = null;
                    Pass_reg2.BorderBrush = null;
                    if (pass.Any(specsum.Contains))
                    {
                        Pass_reg1.BorderBrush = null;
                        Pass_reg2.BorderBrush = null;
                        if (Pass_reg1.Password == Pass_reg2.Password)
                        {
                            if ((Email_Box.Text).Length > 5)
                            {
                                if (mail.Any(maill.Contains))
                                {
                                    var user = new User { Login = login, Password = pass, Email = mail };
                                    context.Users.Add(user);
                                    context.SaveChanges();
                                    MainWindow main = new MainWindow();
                                    main.Show();
                                    this.Hide();
                                    Email_Box.BorderBrush = null;
                                }
                                else
                                {
                                    Osh_reg.Text = ("неверный email");
                                    Email_Box.BorderBrush = Brushes.Red;
                                    Email_Box.BorderThickness = new Thickness(2);
                                }
                            }
                            else
                            {
                                Osh_reg.Text = ("неверный email");
                                Email_Box.BorderBrush=Brushes.Red;
                                Email_Box.BorderThickness= new Thickness(2);
                            }
                        }
                        else
                        {
                            Osh_reg.Text = ("Пароли должны совпадать");
                            Pass_reg1.BorderBrush = Brushes.Red;
                            Pass_reg1.BorderThickness = new Thickness(2);
                            Pass_reg2.BorderBrush = Brushes.Red;
                            Pass_reg2.BorderThickness = new Thickness(2);
                        }
                    }
                    else
                    {
                        Osh_reg.Text = ("Введите специальные символы");
                        Pass_reg1.BorderBrush = Brushes.Red;
                        Pass_reg1.BorderThickness = new Thickness(2);
                        Pass_reg2.BorderBrush = Brushes.Red;
                        Pass_reg2.BorderThickness = new Thickness(2);
                    }
                }
                else
                {
                    Osh_reg.Text = ("Пароль должен состоять минимум из 6 символов и не более 16 символов");
                    Pass_reg1.BorderBrush = Brushes.Red;
                    Pass_reg1.BorderThickness = new Thickness(2);
                    Pass_reg2.BorderBrush = Brushes.Red;
                    Pass_reg2.BorderThickness = new Thickness(2);
                }
            }
            else
            {
                Osh_reg.Text = ("Логин должен состоять минимум из 3 букв и не более 16");
                LogBox.BorderBrush = Brushes.Red;
                LogBox.BorderThickness = new Thickness(2);

            }
            
        }
    }
}
