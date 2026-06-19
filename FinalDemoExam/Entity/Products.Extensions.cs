using System;
using System.IO;

namespace FinalDemoExam.Entity
{
    public partial class Products
    {
        public string DisplayPhotoPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(photo_path))
                {
                    if (File.Exists(photo_path))
                    {
                        return photo_path;
                    }

                    return $"/Image/{Path.GetFileName(photo_path)}";
                }

                return "/Image/picture.png";
            }
        }

        public decimal FinalPrice => Math.Round(price * (100 - discount) / 100, 2);

        public string PriceText => $"{price:0.00} руб.";

        public string FinalPriceText => $"{FinalPrice:0.00} руб.";

        public string DiscountText => $"{discount:0.##}%";

        public string StockText => $"{stock_quantity} {Units?.name}";

        public bool HasDiscount => discount > 0;

        public string RowBackground
        {
            get
            {
                if (stock_quantity == 0) return "LightGray";
                if (discount > 25) return "#23E1EF";
                return "White";
            }
        }
    }
}
