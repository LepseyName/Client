using Client.Logic;
using Client.Model;
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

namespace Client.View
{
    /// <summary>
    /// Логика взаимодействия для MyPage.xaml
    /// </summary>
    public partial class MyPage : Window
    {
        private int offsetLoad = 0;

        private int RAMKA_WIDTH = 120;
        private int RAMKA_HEIGHT = 150;
        private int COUNT_WIDTH = 5;

        
        public MyPage()
        {
            this.updateUrl();
            InitializeComponent();
        }

        private void updateUrl()
        {
            MainWindow url = new MainWindow();
            url.ShowDialog();
        }

        private Image getImageWithSourse(string pathOrUrl)
        {
            //var uriSource = new Uri(@"/WpfApplication1;component/Images/Untitled.png", UriKind.RelativeOrAbsolute Relative);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.None;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            
            bitmapImage.UriSource = new Uri(pathOrUrl);
            bitmapImage.EndInit();
            Image photo = new Image();
            photo.Source = bitmapImage;
            return photo;
        }

        private Button getButtonWithImage(string imagePath)
        {
            Button btn = new Button();
            Image picture = getImageWithSourse(imagePath);
            btn.Content = picture;
            return btn;
        }


        private void addButtonToPanel(string image, int id, string tag, double width, ref StackPanel panel)
        {
            Button btn = getButtonWithImage(image);
            btn.Width = width;
            btn.Tag = new ButtonTag() { ID = id, name = tag };
            panel.Children.Add(btn);
        }


        private void showOneCard(ProductCard card, double left, double top)
        {
            StackPanel ramka = new StackPanel();
            ramka.Width = RAMKA_WIDTH;
            ramka.Height = RAMKA_HEIGHT;
            ramka.HorizontalAlignment = HorizontalAlignment.Left;
            ramka.VerticalAlignment = VerticalAlignment.Top;

            Image photo = getImageWithSourse(Requests.getRootUrl() + card.imageSrc);
            photo.Height = 6 * ramka.Height / 10;

            Label name = new Label();
            name.Content = card.name;

            StackPanel buttons = new StackPanel();
            buttons.Width = ramka.Width;
            buttons.Orientation = Orientation.Horizontal;

            addButtonToPanel(Environment.CurrentDirectory + "/View/del.png",card.id, "del", buttons.Width*3/10, ref buttons);
            addButtonToPanel(Environment.CurrentDirectory + "/View/upd.png", card.id, "upd", buttons.Width * 3 / 10, ref buttons);
            addButtonToPanel(Environment.CurrentDirectory + "/View/see.png", card.id, "see", buttons.Width * 3 / 10, ref buttons);

            buttons.AddHandler(Button.ClickEvent, new RoutedEventHandler(Cards_Button_Click));

            ramka.Children.Add(photo);
            ramka.Children.Add(name);
            ramka.Children.Add(buttons);
            ramka.Margin = new Thickness(left, top, 0,0);
            this.Plane.Children.Add(ramka);
        }
        
        private void showEditor(ProductCard card, string action)
        {
            Editor window = new Editor(card, action);
            window.ShowDialog();
            this.update(50, 0);
        }

        private void update(int count, int? offset)
        {
            if (offset == null) offset = this.offsetLoad;
            offset = (int)offset;
            string sort = this.sortBox.Text=="Name"?"name":"id";

            this.Plane.Children.Clear();
            
            ProductCard[] array = Requests.getCard(count, (int)offset, sort);
            this.offsetLoad = (int)offset + count;
            
            int width = 12 * RAMKA_WIDTH / 11;
            int height = 12 * RAMKA_HEIGHT / 11;
            for (int i = 0; i < array.Length; i++)
                showOneCard(array[i], 10 + (i % COUNT_WIDTH) * width, 10 + (i / COUNT_WIDTH) * height);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.update(50, 0);
        }

        private void Cards_Button_Click(object sender, RoutedEventArgs e)
        {    
            if( e.OriginalSource.GetType() == typeof(Button) && ((Button)e.OriginalSource).Tag.GetType() == typeof(ButtonTag))
            {
                var tag = (ButtonTag)((Button)e.OriginalSource).Tag;
                switch (tag.name)
                {
                    case "del" : {
                            Requests.deleteCardById(tag.ID);
                            this.update(50, 0);
                            break;
                        }
                    case "see":
                        {
                            this.showEditor(Requests.getCardById(tag.ID), "see");
                            break;
                        }
                    case "upd":
                        {
                            this.showEditor(Requests.getCardById(tag.ID), "upd");
                            break;
                        }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.showEditor(null, "new");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
