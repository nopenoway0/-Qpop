using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qpop
{
    public partial class Program_Selection : Form
    {
        public delegate void Button_Add_Enabler(string button_name);
        public int num_buttons = 2;
        private TextBox tstBox = new TextBox();
        private TableLayoutPanel tpane = new TableLayoutPanel();
        [STAThread]
        public static void Main()
        {
            Program_Selection app = new Program_Selection();
            Application.EnableVisualStyles();
            Application.Run(app);
        }
        public Program_Selection()
        {
            InitializeComponent();
            Thread main_runner = new Thread(delegate ()
            {
                Program.Start(this);
            });
            main_runner.IsBackground = true;
            main_runner.Start();
            this.Dock = DockStyle.Fill;
            //Set up Tpane to fill window
            tpane.VerticalScroll.Enabled = true;
            tpane.AutoSize = true;
            //End set up
            this.Controls.Add(tpane);
        }

        private void Program_Selection_Load(object sender, EventArgs e)
        {

        }

        public void Add_Button(string button_name)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new Button_Add_Enabler(Add_Button), new object[] { button_name });
                return;
            }
            else{ Button b1 = new Button();
                b1.Text = button_name;
                b1.AutoSize = true;
                tpane.Controls.Add(b1); }
        }
    }
}
