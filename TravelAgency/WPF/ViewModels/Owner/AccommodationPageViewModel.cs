using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class AccommodationPageViewModel 
    {
        public User LoggedInUser { get; private set; }
        private AccommodationsPage _accommodationsPage;
        private WrapPanel _accommodationsPanel;
        private AccommodationService _accommodationService;
        public AccommodationPageViewModel(User user,AccommodationsPage accommodationPage)
        {
            LoggedInUser = user;
            _accommodationsPage = accommodationPage;
            _accommodationService = new();
            
        }
        public void FillAccommodationsPanel()
        {
            _accommodationsPanel = _accommodationsPage.AccommodationsPanel;
            foreach(Accommodation accommodation in _accommodationService.FindUsersAccommodations(LoggedInUser.Id))
            {
                AddAccommodation(accommodation.Name, "/Resources/Images/UnknownPhoto.png");
            }
            
        }

        private void AddAccommodation(string name, string imagePath)
        {
            StackPanel stackPanel = new StackPanel();

            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            border.Margin = new Thickness(5);

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            image.Width = 250;
            image.Height = 150;
            image.Margin = new Thickness(0, 25, 0, 15);

            TextBlock nameTextBlock = new TextBlock();
            nameTextBlock.Text = name;
            nameTextBlock.TextAlignment = TextAlignment.Center;
            nameTextBlock.TextWrapping = TextWrapping.Wrap;
            nameTextBlock.FontSize = 25;

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(nameTextBlock);

            border.Child = stackPanel;
            _accommodationsPanel.Children.Add(border);
        }
    }
    
}
