using FinalDemoExam.Entity;
using System.Windows;
namespace FinalDemoExam
{
    public partial class App : Application
    {
        public static FinalDemoExamKorepinVDEntities DB = new FinalDemoExamKorepinVDEntities();
        public static int UserRole = 1;
        public static int UserId = 0;
        public static string UserFullName = "Гость";
    }
}