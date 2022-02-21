using Client.Logic;
using Client.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Client.View
{
    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private ProductCard card;
        private string action;
        private string photoSrc = null;
        private bool isChangePhoto = false;

        public Editor(ProductCard card, string action)
        {
            InitializeComponent();
            this.card = card;
            this.action = action;
            if (action == "see") this.saveButton.Visibility = Visibility.Hidden;
            if (action == "upd" || action =="see")
            {
                if (card == null) throw new Exception("Card is null!");
                setImage(Requests.getRootUrl() + card.imageSrc);

                this.nameText.Text = card.name;
            }
            else this.card = null;
            
        }

        private string getImageData()
        {
            if(this.photoSrc == null)
            {
                MessageBox.Show("Photo can not select!");
                return null;
            }
            using (FileStream stream = File.OpenRead(this.photoSrc))
            {
                byte[] array = new byte[stream.Length];
                stream.Read(array, 0, array.Length);
                return Convert.ToBase64String(array);
            }
        }

        private void setImage(string url)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(url);
            bitmapImage.EndInit();
            this.image.Source = bitmapImage;
            this.photoSrc = url;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Photo(jpg)|*.jpg";
            if ((bool)file.ShowDialog())
            {
                setImage(file.FileName);
                this.isChangePhoto = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(this.card == null)
            {
                this.card = new ProductCard() { ImageData = getImageData(), name = this.nameText.Text };
                if (this.card.isValid())
                {
                    try{
                        Requests.createCard(this.card);
                        this.Close();
                    }
                    catch (Exception) { 
                        MessageBox.Show("internet exception!");
                        this.updateUrl();
                    }
                }   
                else
                    MessageBox.Show("invalide data!");
                this.card = null;
            }
            else
            {
                this.card.name = this.nameText.Text;
                if (isChangePhoto)
                    this.card.ImageData = getImageData();

                if (this.card.isValid())
                {
                    try
                    {
                        Requests.updateCard(this.card);
                        this.Close();
                    }
                    catch (Exception) { 
                        MessageBox.Show("internet exception!");
                        this.updateUrl();
                    }
                    
                }   
                else
                    MessageBox.Show("invalide data!");
            }
        }

        private void updateUrl()
        {
            MainWindow url = new MainWindow();
            url.ShowDialog();
        }
    }
}
