using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ProductCard
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageSrc { get; set; }
        public string ImageData { get; set; }

        public ProductCard() { }

        public void update(ProductCard card)
        {
            this.name = card.name;
            //this.ImageSrc = card.ImageSrc;
        }

        public bool isValid()
        {
            return (name != null && name.Length > 3) && (this.imageSrc != null || this.ImageData != null);
        }
    }
}
