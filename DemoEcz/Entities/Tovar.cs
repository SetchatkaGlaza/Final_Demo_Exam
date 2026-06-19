
namespace DemoEcz.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tovar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tovar()
        {
            this.Zakazs = new HashSet<Zakaz>();
        }
    
        public string art { get; set; }
        public string name { get; set; }
        public string ed_izm { get; set; }
        public int cena { get; set; }
        public string postaw { get; set; }
        public string proizw { get; set; }
        public Nullable<int> kat { get; set; }
        public Nullable<int> skidka { get; set; }
        public Nullable<int> ostatok { get; set; }
        public string opisanie { get; set; }
        public string foto { get; set; }
        public string FotoItog
        {
            get
            {
                if (string.IsNullOrWhiteSpace(foto))
                {
                    return "/Assets/picture.png";
                }
                return $"/Assets/{foto}";

            }
        }
        // попытка
        public int NovCena
        {
            get
            {
                // Если скидка не указана
                if (skidka == null || skidka == 0)
                    return cena;

                // Рассчитываем цену со скидкой
                decimal NewPrice = cena * (100 - skidka.Value) / 100;
                return (int)Math.Round(NewPrice);
            }
        }
        // фон
        public string Zvet
        {
            get
            {
                if (skidka >= 15)
                    return "#2E8B57";

                if (ostatok == 0)
                    return "Aqua";

                return "#7FFF00";
            }
        }

        //Strikethrough
        public string Strikethrough
        {
            get
            {
                if (skidka >= 1)
                    return "Strikethrough";
                return "None";
            }
        }
        // read
        public string TextColor
        {
            get
            {
                if (skidka >= 1)
                    return "red";
                return "black";
            }
        }

        // read
        public string Visibility
        {
            get
            {
                if (skidka >= 1)
                    return "Visible";
                return "Hidden";
            }
        }

        public virtual Kat Kat1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zakaz> Zakazs { get; set; }
    }
}
