using GUI;
namespace Main{
    static class Program{
        public static void Main(){
            ApplicationConfiguration.Initialize();
            Application.Run(new Forms());
        }
    }
}