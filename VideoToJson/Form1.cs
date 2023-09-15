using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace VideoToJson
{
    public partial class Form1 : Form
    {
        Watch watch;
        private int processMinute = 5;
        

        public Form1()
        {
            InitializeComponent();
            watch = new Watch();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread th = new Thread(Start);
            th.Start();
        } 

        private void FolderCreate(string mainPath)
        {
            DateTime now = DateTime.Now;
            string yearFolderPath = Path.Combine(mainPath, now.Year.ToString());
            if (!Directory.Exists(yearFolderPath))
            {
                Directory.CreateDirectory(yearFolderPath);
            }
            string monthFolderPath = Path.Combine(yearFolderPath, now.Month.ToString("00"));
            if (!Directory.Exists(monthFolderPath))
            {
                Directory.CreateDirectory(monthFolderPath);
            }
            string dayFolderPath = Path.Combine(monthFolderPath, now.Day.ToString("00"));
            if (!Directory.Exists(dayFolderPath))
            {
                Directory.CreateDirectory(dayFolderPath);
            }
            string hourFolderPath = Path.Combine(dayFolderPath, now.Hour.ToString("00"));
            if (!Directory.Exists(hourFolderPath))
            {
                Directory.CreateDirectory(hourFolderPath);
            }
            string minuteFolderPath = Path.Combine(hourFolderPath, now.Minute.ToString("00"));
            if (!Directory.Exists(minuteFolderPath))
            {
                Directory.CreateDirectory(minuteFolderPath);
            }
        }

        public static string UpdateOutputDirectory()
        {
            string year = DateTime.Now.Year.ToString("00"); // Replace with your desired directory
            string month = DateTime.Now.Month.ToString("00");
            string day = DateTime.Now.Day.ToString("00");
            string hour = DateTime.Now.Hour.ToString("00");
            string minute = DateTime.Now.Minute.ToString("00");

            string outputDirectory = Path.Combine("C:\\Users\\yigit\\OneDrive\\Masaüstü\\RTSP", year);
            outputDirectory = Path.Combine(outputDirectory, month);
            outputDirectory = Path.Combine(outputDirectory, day);
            outputDirectory = Path.Combine(outputDirectory, hour);
            outputDirectory = Path.Combine(outputDirectory, minute);
            return outputDirectory;
        }
        public void Start()
        {
            // RTSP URL of the live video stream
            string rtspUrl = "rtsp://admin:admin@10.3.26.18/profile?token=media_profile1&SessionTimeout=60"; 
            
            string outputDirectory;
            int currentMinute = 0;
            while(currentMinute < processMinute)
            {
                FolderCreate("C:\\Users\\yigit\\OneDrive\\Masaüstü\\RTSP"); 
                outputDirectory = UpdateOutputDirectory();

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\EskiHali\\VideoSaveToDatabase\\VideoToJson\\bin\\Debug\\ffmpeg.exe";

                    process.StartInfo.Arguments = $"-i {rtspUrl} -vf fps=15 {outputDirectory}\\frame_%d.jpg";


                    process.StartInfo.UseShellExecute = false;

                    process.StartInfo.RedirectStandardOutput = true;

                    process.StartInfo.CreateNoWindow = true;    

                    process.Start();

                    while (!File.Exists(Path.Combine(outputDirectory, "frame_1.jpg")))
                    {

                    }
                    watch.Start();
                    JpegToJson.ImagetoJson(outputDirectory, 901,currentMinute);
                    PrintResult(watch.StopResult());
                    process.Kill();
                    process.Close();
                }
                currentMinute++;
            }
            PrintResult("Program Sonlandı!!!!!!!!!!!!!!!");    
            
        }

        private void PrintResult(string result)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => PrintResult(this, result)));
            else
                PrintResult(this, result);
        }
        public void PrintResult(Form frm, string asdf) { frm.Text = asdf; }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
