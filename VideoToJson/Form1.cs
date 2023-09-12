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
        private const int MaxQueueSize = 1000;
        private readonly Queue<byte[]> frameQueue = new Queue<byte[]>();
        private readonly object queueLock = new object();
        private bool isProcessing = false; 
        private bool isRunning = true;

        public Form1()
        {
            InitializeComponent();
            watch = new Watch();
        }
        private void EnqueueFrame(byte[] frameData)
        {
            lock (queueLock)
            {
                frameQueue.Enqueue(frameData);
                while (frameQueue.Count > MaxQueueSize)
                {
                    frameQueue.Dequeue(); // FIFO: Eski kareleri sil
                }
            }
        }

        private byte[] DequeueFrame()
        {
            lock (queueLock)
            {
                if (frameQueue.Count > 0)
                {
                    return frameQueue.Dequeue();
                }
                return null;
            }
        }

        private void DeleteOldestImageFromDisk(string outputDirectory,int maxSize)
        {

            // Output dizini içindeki tüm resim dosyalarını al
            string[] imageFiles = Directory.GetFiles(outputDirectory);

            if(imageFiles.Length >= maxSize) 
            { 
                int silinmeSayisi = imageFiles.Length - maxSize;
                for(int i = 0; i < silinmeSayisi; i++)
                {
                    File.Delete(imageFiles[i]);
                   
                }
            }
        }

        public void WritePathsToTxt(string path)
        {
            using (StreamWriter sw = new StreamWriter("C:\\Users\\yigit\\OneDrive\\Masaüstü\\jpeg_paths.txt", true))
            {
                sw.WriteLine(path);
            }
        }

        public void SilTxtIcerik(string dosyaYolu)
        {
            // Dosyanın içeriğini sıfırla
            File.WriteAllText(dosyaYolu, string.Empty);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread th = new Thread(Start);
            th.Start();
        } 

        private void FolderCreate(string mainPath)
        {
            // Mevcut zamanı al.
            DateTime now = DateTime.Now;

            // Yıl klasörünü oluştur.
            string yearFolderPath = Path.Combine(mainPath, now.Year.ToString());
            if (!Directory.Exists(yearFolderPath))
            {
                Directory.CreateDirectory(yearFolderPath);
            }

            // Ay klasörünü oluştur.
            string monthFolderPath = Path.Combine(yearFolderPath, now.Month.ToString("00"));
            if (!Directory.Exists(monthFolderPath))
            {
                Directory.CreateDirectory(monthFolderPath);
            }

            // Gün klasörünü oluştur.
            string dayFolderPath = Path.Combine(monthFolderPath, now.Day.ToString("00"));
            if (!Directory.Exists(dayFolderPath))
            {
                Directory.CreateDirectory(dayFolderPath);
            }

            // Saat klasörünü oluştur.
            string hourFolderPath = Path.Combine(dayFolderPath, now.Hour.ToString("00"));
            if (!Directory.Exists(hourFolderPath))
            {
                Directory.CreateDirectory(hourFolderPath);
            }

            // Dakika klasörünü oluştur.
            string minuteFolderPath = Path.Combine(hourFolderPath, now.Minute.ToString("00"));
            if (!Directory.Exists(minuteFolderPath))
            {
                Directory.CreateDirectory(minuteFolderPath);
            }

            // İşlem başarılı.
            SomeMethod("İşlem Başarılı");
        }

        private string UpdateOutputDirectory()
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
            FolderCreate("C:\\Users\\yigit\\OneDrive\\Masaüstü\\RTSP");
            string outputDirectory = UpdateOutputDirectory();


            // Create a process to run ffmpeg
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "ffmpeg";

                process.StartInfo.Arguments = $"-i {rtspUrl} -vf fps=30 {outputDirectory}\\{DateTime.Now.ToString("yyyy_dd_MM_HH_mm_ss")}_frame%d.jpg";

                

                process.StartInfo.UseShellExecute = false;

                process.StartInfo.RedirectStandardOutput = true;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (stopwatch.Elapsed.TotalSeconds < 60 )
                {
                    process.Start();
               
                    SomeMethod(process.ProcessName);

                     //watch.Start();
                     //JpegToJson.ImagetoJson("C:\\Users\\yigit\\OneDrive\\Masaüstü\\jpeg_paths.txt");
                     //SomeMethod(watch.StopResult());
                   
                    JpegToJson.ImagetoJson("C:\\Users\\yigit\\OneDrive\\Masaüstü\\jpeg_paths.txt");
                stopwatch.Stop();
                process.WaitForExit();
                process.Close();
                outputDirectory = UpdateOutputDirectory();
            }        
        }
                }

            }


        }
        private void SomeMethod(string result)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => SomeMethods(this, result)));
            else
                SomeMethods(this, result);
        }
        public void SomeMethods(Form frm, string asdf) { frm.Text = asdf; }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetProcessesByName("ffmpeg")[0].Kill();
            Environment.Exit(0);
        }

        public void KillProcess(int pid) {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited) proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }

            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcess(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }
        }
        public static List<string> yollariDosyayaYaz()
        {
            int imageCount = 0;
            List<string> imagePaths = new List<string>();
            
            imageCount++;
            string imagePath = Path.Combine("C:\\Users\\yigit\\OneDrive\\Masaüstü\\jpeg_paths.txt", $"frame_{imageCount:D4}.jpg");

            // Dosyayı yazmadan önce kapat
            

                
            imagePaths.Add(imagePath);
           
            return imagePaths;

        }
    }
}
