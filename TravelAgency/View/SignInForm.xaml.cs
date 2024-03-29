﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password && user.Role == Roles.VLASNIK)
                {
                    ShowAccommodations showAccommodations = new ShowAccommodations(user);
                    showAccommodations.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.VODIC)
                {
                    ShowToursWindow showToursWindow = new ShowToursWindow(user);
                    showToursWindow.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.GOST1)
                {

                    Close();
                }
                else if (user.Password == txtPassword.Password && user.Role == Roles.GOST2)
                {
                    ShowAndSearchTours showAndSearchTours = new ShowAndSearchTours(user);
                    showAndSearchTours.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }
    }
}
