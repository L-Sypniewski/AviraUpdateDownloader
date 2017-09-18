using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
namespace AviraUpdater
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }
        private TaskbarManager windowsTaskbar = TaskbarManager.Instance;
        private void button1_Click(object sender, EventArgs e)
        {

            string systemFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);

            IJumpListTask notepadTask = new JumpListLink(System.IO.Path.Combine(systemFolder, "notepad.exe"), "Open Notepad")
            {
                IconReference = new IconReference(Path.Combine(systemFolder, "notepad.exe"), 0)
            };
            IJumpListTask calcTask = new JumpListLink(Path.Combine(systemFolder, "calc.exe"), "Open Calculator")
            {
                IconReference = new IconReference(Path.Combine(systemFolder, "calc.exe"), 0)
            };
            IJumpListTask paintTask = new JumpListLink(Path.Combine(systemFolder, "mspaint.exe"), "Open Paint")
            {
                IconReference = new IconReference(Path.Combine(systemFolder, "mspaint.exe"), 0)
            };

            // JumpList.AddUserTasks(notepadTask, calcTask, new JumpListSeparator(), paintTask);
            // JumpList.Refresh();



            if (button1.Text != "Zakończ")
            {
                button1.Enabled = false;

                Icon Ikona = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\avira_by_faton.ico");
                TaskbarManager.Instance.SetOverlayIcon(Ikona, "Ikona");

                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri("http://dl.antivir.de/down/vdf/ivdf_fusebundle_nt_en.zip"), System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ivdf_fusebundle_nt_en.zip");
            }
            else Application.Exit();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            const string AppName = "Avira Aktualizator";
            progressBar1.Value = e.ProgressPercentage;
            double KiloBytesReceived, TotalKiloBytesToReceive;
            KiloBytesReceived = e.BytesReceived / 1024;
            TotalKiloBytesToReceive = e.TotalBytesToReceive / 1024;
            this.Text = KiloBytesReceived.ToString() + "Kb" + " / " + TotalKiloBytesToReceive + "Kb" + " - " + AppName;
            windowsTaskbar.SetProgressValue(e.ProgressPercentage, 100);
            if (e.ProgressPercentage != 100)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }

        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            this.Activate();
            button1.Text = "Zakończ";

        }

        private void MainFrm_Activated(object sender, EventArgs e)
        {

            if (TaskbarManager.IsPlatformSupported == false)
            {
                MessageBox.Show("System nie obsługuje Paska Zadań z Windows 7 lub nowszych!", "Pełna fuckjonalność nie jest możliwa!", MessageBoxButtons.OK, MessageBoxIcon.Error); Application.Exit();
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            JumpList jump = JumpList.CreateJumpList();
            jump.AddUserTasks(new JumpListLink("C:\\Program Files (x86)\\CCleaner\\CCleaner.exe", "Uruchom CCleanera")
            {
                IconReference = new IconReference("C:\\Program Files (x86)\\CCleaner\\CCleaner.exe", 0)
                
            });
            jump.Refresh();

        }
    }
}

