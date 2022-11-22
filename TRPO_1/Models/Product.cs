using System.Drawing;
using System.Windows.Media;
using TRPO_1.Extensions;

namespace TRPO_1.Models
{
    public class Product
    {
        public string Name { get; set; }
        public float Price { get; set; }

        public float Volume { get; set; } = 0.33f;
        
        public Bitmap Image { get; set; }

        public ImageSource ImageSource => Image.ToImageSource();

        public Product Clone()
        {
            return (Product)MemberwiseClone();
        }
    }
}
