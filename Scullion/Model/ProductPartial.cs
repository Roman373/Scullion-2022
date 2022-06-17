using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scullion.Model
{
    public partial class Product
    {
        public string Images
        {
            get
            {
                if (ProductImage != null && ProductImage != "")
                    return ProductImage;
                else
                    return "/Images/picture.png";
            }
        }
        public string BackColor
        {
            get
            {
                if (ProductCurrentDiscount > 15)
                    return "#7fff00";
                return "";
            }
        }
        public string CurrentDiscount
        {
            get
            {
                return $"Скидка {ProductCurrentDiscount}%";
            }
        }
        public string AdminControlsVisibility
        {
            get
            {
                if (App.CurrentUser.UserRole == 1)
                    return "Visible";
                else
                    return "Collapsed";
            }
        }
    }
}
