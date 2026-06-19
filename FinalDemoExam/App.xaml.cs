using FinalDemoExam.Entity;
using System.Windows;
namespace FinalDemoExam
{
    public partial class App : Application
    {
        public static FinalDemoExamKorepinVDEntities DB = new FinalDemoExamKorepinVDEntities();
        public static int UserId;
        public static string UserFullName;
        public static string UserRole;
    }
}