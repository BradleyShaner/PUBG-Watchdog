using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PubgKillAndRun
{
    public partial class Form1 : Form
    {
        private int notResponding = 0;
        private int respondingDelay = 10;

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
                if (LaunchProcess("steam://run/578080", "") <= 0)
                {
                    MessageBox.Show("Unable to launch PUBG through Steam. You sure you have Steam installed properly?");
                }
            }
            else
            {
                MessageBox.Show("PUBG is still running :( try again.", "Pubg Watchdog");
            }
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

        public static bool IsProcessResponding(string process)
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
                if (IsProcessResponding("TslGame.exe"))
                {
                    pubgStatus.Text = "PUBG: Running";
                    notResponding = 0;
                }
                else
                {
                    pubgStatus.Text = "PUBG: Not Responding.. " + notResponding + "/" + respondingDelay;
                    notResponding++;

                    if (notResponding >= respondingDelay)
                    {
                        button1.PerformClick();
                        notResponding = 0;
                    }
                }
            }
            else
            {
                pubgStatus.Text = "PUBG: Not Running";
            }
        }

        private void killPUBGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillProcess("TslGame.exe");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simple kill-and-restart PUBG in C#. MIT license." + Environment.NewLine + "<3 LostSoulfly", "Pubg Watchdog");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}