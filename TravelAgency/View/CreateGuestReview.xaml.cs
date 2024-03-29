﻿using System;
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
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateGuestReview.xaml
    /// </summary>
    public partial class CreateGuestReview : Window
    {
        private UserRepository _userRepository;
        private GuestReviewRepository _guestReviewRepository;
        public int OwnerId { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public int CleanlinessGrade { get; set; }
        public int RespectGrade { get; set; }
        public string Comment { get; set; }
        public CreateGuestReview(User user,int guestId,GuestReviewRepository guestReviewRepository)
        {
            DataContext = this;
            _userRepository = new UserRepository();
            _guestReviewRepository = guestReviewRepository;

            OwnerId = user.Id;
            GuestId = guestId;
            GuestName = _userRepository.GetById(guestId).Username;

            Comment = String.Empty;

            InitializeComponent();
        }

        private void CheckedCleanliness(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            CleanlinessGrade = Convert.ToInt32(radioButton.Content);
        }

        private void CheckedRules(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            RespectGrade = Convert.ToInt32(radioButton.Content);
        }

        private void ButtonClickCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {
            if(Comment == null) 
            {
                Comment = String.Empty;
            }
            GuestReview guestReview = new(OwnerId, GuestId, CleanlinessGrade, RespectGrade, Comment);
            _guestReviewRepository.Save(guestReview);
            Close();
        }
    }
}
