using System.Windows;

namespace DemoEcz
{
    public partial class App : Application
    {
        public static Entities.BD1 ContextBD { get; set; } = new Entities.BD1();

        public static Entities.User CurentUser { get; set; }

        public static Entities.Tovar CurrentTovar { get; set; }
    }
}
