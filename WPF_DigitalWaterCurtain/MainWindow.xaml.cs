using Microsoft.Win32;
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

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using static MaterialDesignThemes.Wpf.Theme;
using System.Windows.Markup;


namespace WPF_DigitalWaterCurtain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> imagePaths = new List<string>();
        private int currentIndex = 0;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "vIh7Mem9O5129VMl5kcNPPwzZUk7S3kVHVqgPR8M",
            BasePath= "https://water-curtain-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public MainWindow()
        {
            InitializeComponent();

            client = new FireSharp.FirebaseClient(config);
            if (client != null )
            {
                MessageBox.Show("Connection is established");
            }
            Insert_Click();
        }

        private async void Insert_Click()
        {
            var data = new Data
            {
                Id = "abc"
            };

            SetResponse response = await client.SetTaskAsync("Information/" + "a", data);

            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data Inserted");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                imagePaths.Clear();
                imagePaths.AddRange(dialog.FileNames);
                currentIndex = 0;
                DisplayImage();
                LoadThumbnails();
                UpdateButtons();
            }
        }

        private void DisplayImage()
        {
            if (imagePaths.Count > 0)
            {
                var imagePath = imagePaths[currentIndex];
                MainImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private void LoadThumbnails()
        {
            ThumbnailPanel.Children.Clear();
            for (int i = 0; i < imagePaths.Count; i++)
            {
                var border = new Border
                {
                    BorderThickness = new Thickness(i == currentIndex ? 3 : 0),
                    BorderBrush = Brushes.Red,
                    Margin = new Thickness(5),
                    Child = new Image
                    {
                        Source = new BitmapImage(new Uri(imagePaths[i])),
                        Width = 100,
                        Height = 100,
                        Tag = i // Gắn chỉ số để xác định ảnh
                    }
                };

                border.MouseLeftButtonUp += Thumbnail_Click;
                ThumbnailPanel.Children.Add(border);
            }
        }

        private void Thumbnail_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is Image image && image.Tag is int index)
            {
                currentIndex = index;
                DisplayImage();
                UpdateThumbnails();
                UpdateButtons();
            }
        }

        private void UpdateThumbnails()
        {
            for (int i = 0; i < ThumbnailPanel.Children.Count; i++)
            {
                if (ThumbnailPanel.Children[i] is Border border)
                {
                    border.BorderThickness = new Thickness(i == currentIndex ? 3 : 0);
                }
            }
        }


        private void Thumbnail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image thumbnail && thumbnail.Tag is int index)
            {
                currentIndex = index;
                DisplayImage();
                UpdateButtons();
            }
        }

        private void UpdateButtons()
        {
            PrevButton.IsEnabled = currentIndex > 0;
            NextButton.IsEnabled = currentIndex < imagePaths.Count - 1;
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayImage();
                ScrollToThumbnail();
                UpdateThumbnails();
                UpdateButtons();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < imagePaths.Count - 1)
            {
                currentIndex++;
                DisplayImage();
                ScrollToThumbnail();
                UpdateThumbnails();
                UpdateButtons();
            }
        }
        private void ScrollToThumbnail()
        {
            if (ThumbnailPanel.Children.Count > currentIndex)
            {
                var thumbnail = ThumbnailPanel.Children[currentIndex] as FrameworkElement;
                if (thumbnail != null)
                {
                    thumbnail.BringIntoView(new Rect(0, 0, thumbnail.RenderSize.Width, thumbnail.RenderSize.Height));
                }
            }
        }

    }
}
