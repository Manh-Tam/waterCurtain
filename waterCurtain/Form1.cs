
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Drawing.Imaging;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace waterCurtain
{
    public partial class Form1 : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "vIh7Mem9O5129VMl5kcNPPwzZUk7S3kVHVqgPR8M",
            BasePath = "https://water-curtain-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client != null)
                MessageBox.Show("Connection established");
            else
                MessageBox.Show("Connection not established");
        }

        private async void Insert_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Id = textBox1.Text
            };

            SetResponse response = await client.SetTaskAsync("Information/" + textBox1.Text, data);

            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data Inserted");
        }

        private async void Retrieve_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Information/" + textBox1.Text);
            Data obj = response.ResultAs<Data>();
            MessageBox.Show(obj.Id);
        }

        private async void Update_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Id = textBox1.Text
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Information/" + textBox1.Text, data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Data updated at ID: " + result.Id);
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Information/" + textBox1.Text);
            MessageBox.Show(textBox1.Text + " deledted");
        }

        private async void DeleteAll_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Information");
            MessageBox.Show("all elements deledted");
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "Image Files(*.jpg) | *.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);
                imageBox.Image = img.GetThumbnailImage(350, 200, null, new IntPtr());
            }
        }

        private async void InsertImg_Click(object sender, EventArgs e)
        {
            //INSERT - PictureBox into ms => byte array => toBase64String
            MemoryStream ms = new MemoryStream();
            imageBox.Image.Save(ms, ImageFormat.Jpeg);

            byte[] a = ms.GetBuffer();

            string output = Convert.ToBase64String(a);

            var data = new Image_Modal
            {
                Img = output
            };

            SetResponse response = await client.SetTaskAsync("Image/", data);
            Image_Modal result = response.ResultAs<Image_Modal>();

            imageBox.Image = null;
            MessageBox.Show("Image inserted");
        }

        /*
        private async void RetrieveImg_Click(object sender, EventArgs e)
        {
            //Retrieve base64->byte array->ms->Bitmap
            FirebaseResponse response = await client.GetTaskAsync("Image/");
            Image_Modal image = response.ResultAs<Image_Modal>();
            byte[] b = Convert.FromBase64String(image.Img);
            
            MemoryStream ms = new MemoryStream();
            ms.Write(b, 0, Convert.ToInt32(b.Length));

            Bitmap bm = new Bitmap(ms, false);
            ms.Dispose();
            imageBox.Image = bm;


        }
        */
        private async void RetrieveImg_Click(object sender, EventArgs e)
        {
            // Retrieve base64->byte array->ms->Bitmap
            FirebaseResponse response = await client.GetTaskAsync("Image/");
            Image_Modal image = response.ResultAs<Image_Modal>();
            byte[] b = Convert.FromBase64String(image.Img);

            using (MemoryStream ms = new MemoryStream(b))
            {
                Bitmap bm = new Bitmap(ms);

                
                for (int y = 0; y < bm.Height; y++)
                {
                    for (int x = 0; x < bm.Width; x++)
                    {
                       
                        Color originalColor = bm.GetPixel(x, y);

                        
                        int grayScale = (int)(0.3 * originalColor.R + 0.59 * originalColor.G + 0.11 * originalColor.B);

                        
                        Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);

                        
                        bm.SetPixel(x, y, grayColor);
                    }
                }

                
                imageBox.Image = bm;
            }
        }

    }

}
