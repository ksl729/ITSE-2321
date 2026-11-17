using System;
using System.Windows.Forms;

namespace LibraryObject_KLeach
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // This launches the main window
            Application.Run(new Form1());
        }
    }
}
