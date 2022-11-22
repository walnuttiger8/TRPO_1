using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TRPO_1.Extensions;

namespace TRPO_1.Models
{
    public class MoneyUnit
    {
        public int Quantity { get; set; }
        public Bitmap Image { get; set; }
        public ImageSource Source => Image.ToImageSource();


        public MoneyUnit Clone()
        {
            return (MoneyUnit) MemberwiseClone();
        }
    }
}
