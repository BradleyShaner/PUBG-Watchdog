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

        int notResponding = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Timer t = new Timer();
            t.Interval = 7000;

            t.Tick += delegate
            {
                button1.Enabled = true;
                t.Stop();
            };

            t.Start();

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
            
        }

        private void @delegate(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        public static bool IsProcessRespondingg(string process)
        {
            Process[] proc = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(process));

            if (!proc[0].Responding)
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

                if (IsProcessRespondingg("TslGame.exe"))
                {
                    pubgStatus.Text = "PUBG: Running";
                    notResponding = 0;
                } else
                {
                    pubgStatus.Text = "PUBG: Not Responding; restarting..";
                    notResponding++;

                    if (notResponding >= 10)
                    {
                        button1.PerformClick();
                        notResponding = 0;
                    }
                }

            } else
            {
                pubgStatus.Text = "PUBG: Not Running";
            }
        }
    }
}
