using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgKillAndRun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (IsProcessRunning("TslGame.exe"))
            {
                KillProcess("TslGame.exe");
            }

            System.Threading.Thread.Sleep(1000);

            if (!IsProcessRunning("TslGame.exe"))
            {
                LaunchProcess("steam://run/578080", "");
            } else
            {
                MessageBox.Show("PUBG is still running :( try again.");
            }
            button1.Enabled = true;
            
        }

        public static int LaunchProcess(string exe, string args)
        {
            Process p = System.Diagnostics.Process.Start(exe, args);

            return p.Id;

        }

        public static bool IsProcessRunning(string process)
        {
            Process[] proc = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(process));

            if (proc.Length == 0)
                return false;
            
            return true;
        }

        public static bool KillProcess(string proc)
        {
            bool cantKillProcess = false;

            //Debug("KillProcess entered: " + proc);

            try
            {
                foreach (Process p in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(proc)))
                {
                    if (!p.HasExited) p.Kill();

                    p.WaitForExit(2000);    //wait a max of 2 seconds for the process to terminate

                    if (!p.HasExited)
                        cantKillProcess = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
            return !cantKillProcess;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsProcessRunning("TslGame.exe"))
            {
                pubgStatus.Text = "PUBG: Running";
            } else
            {
                pubgStatus.Text = "PUBG: Not Running";
            }
        }
    }
}
