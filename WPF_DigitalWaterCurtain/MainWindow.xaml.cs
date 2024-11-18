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
using System.IO;


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
            AuthSecret = "1zjb4u8SFhRvUKVioAO2HaQFRujXzDMjE7Ir46PZ",
            BasePath = "https://digitalwatercurtain-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public MainWindow()
        {
            InitializeComponent();

            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                MessageBox.Show("Connection is established");
            }
            //Insert_Click();
            //LoadFromFireBase();
        }

        private async void Insert_Click()
        {
            var data = new Data
            {
                Id = "abc"
            };

            SetResponse response = await client.SetTaskAsync("Information/" + "b", data);

            //Data result = response.ResultAs<Data>();

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

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThumbnailPanel.Children.Count == 0)
            {
                MessageBox.Show("No images available to send.");
                return;
            }

            try
            {
                // Step 1: Send the number of images to the NumOfImg path
                int numOfImages = ThumbnailPanel.Children.Count;
                var num = new Count
                {
                    count = numOfImages.ToString()
                };
                await client.SetTaskAsync("NumOfImg", num);

                // Step 2: Delete existing images from Firebase
                await client.DeleteTaskAsync("Image/");

                // Step 3: Upload new images as a single JSON object
                Dictionary<string, string> imagesData = new Dictionary<string, string>();

                for (int i = 0; i < ThumbnailPanel.Children.Count; i++)
                {
                    if (ThumbnailPanel.Children[i] is Border border && border.Child is Image image)
                    {
                        // Convert the image source to a Bitmap for encoding
                        BitmapSource bitmapSource = image.Source as BitmapSource;
                        if (bitmapSource != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                BitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                                encoder.Save(ms);

                                byte[] byteArray = ms.ToArray();
                                string output = Convert.ToBase64String(byteArray);

                                // Add the image data to the dictionary
                                string imageName = $"Img{i + 1}";
                                imagesData[imageName] = output;
                            }
                        }
                    }
                }

                // Send the entire dictionary as a JSON object to Firebase
                await client.SetTaskAsync("Image", imagesData);

                MessageBox.Show("All images inserted as a single JSON object");


                //MessageBox.Show("All images inserted");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void LoadFromFireBase()
        {
            try
            {
                // Get the number of images stored in Firebase
                FirebaseResponse numResponse = await client.GetTaskAsync("NumOfImg");
                var numData = numResponse.ResultAs<Count>();
                int numOfImages = int.Parse(numData.count);

                // Clear any existing thumbnails
                ThumbnailPanel.Children.Clear();
                imagePaths.Clear(); // Clear local image paths list

                // Load images from Firebase
                for (int i = 0; i < numOfImages; i++)
                {
                    string imageName = $"Img{i + 1}";
                    FirebaseResponse response = await client.GetTaskAsync($"Image/{imageName}");
                    var imageData = response.ResultAs<Image_Modal>();

                    if (imageData != null && !string.IsNullOrEmpty(imageData.Img))
                    {
                        // Convert Base64 string back to BitmapImage
                        byte[] imageBytes = Convert.FromBase64String(imageData.Img);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = ms;
                            bitmap.EndInit();
                            bitmap.Freeze();

                            // Add the image to the thumbnails panel
                            var border = new Border
                            {
                                BorderThickness = new Thickness(0),
                                BorderBrush = Brushes.Red,
                                Margin = new Thickness(5),
                                Child = new Image
                                {
                                    Source = bitmap,
                                    Width = 100,
                                    Height = 100,
                                    Tag = i
                                }
                            };

                            border.MouseLeftButtonUp += Thumbnail_Click;
                            ThumbnailPanel.Children.Add(border);
                        }
                    }
                }

                MessageBox.Show("Images loaded from Firebase");
                currentIndex = 0;
                UpdateButtons();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }
        }

    }
}
