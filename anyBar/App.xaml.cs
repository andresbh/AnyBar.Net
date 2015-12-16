using System.Windows;

namespace anyBar
{
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow();
            wnd.Init(GetPort(e.Args, 1738));
            wnd.Show();
        }

        private int GetPort(string[] args, int @default)
        {
            int i;
            if (args.Length == 1)
                if (int.TryParse(args[0], out i))
                    return i;
            return @default;
        }
    }
}