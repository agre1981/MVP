using MVP.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MVP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ////////// MVP binding //////////////
            var view = new MVP.UI.View();
            var service = new MVP.UI.Service();
            var presenter = new Presenter(view, service);
            /////////////////////////////////////

            Application.Run(view);
        }
    }
}
