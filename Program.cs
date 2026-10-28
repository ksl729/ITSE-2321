namespace Lab6_Payroll_KLeach
{

    // Program: Employee Payroll System
    // Name: Karly Leach

    //This interface that allows input of various employee types, instantiates the appropriate employee object for said employee
    //It can also display the employees and their weekly pay.
    
    internal class Program
    {
        [System.STAThread]
        static void Main(string[] args)
        {
            // Initialize WinForms application and run the main form
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new MainForm());
        }
    }
}
