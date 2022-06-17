using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scullion.Model
{
    public partial class OrderProduct
    {
        public string OrderTotalCost
        {
            get
            {
                return $"Сумма: {TotalCost}руб.";
            }
        }
        public string OrderTotalDiscount
        {
            get
            {
                return $"Скидка: {TotalDiscount}%";
            }
        }
        public string BackColorOrder
        {
            get
            {
                return "#20b2aa";
            }
        }
    }
}
