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

        private void DeleteOldestImageFromDisk(string outputDirectory)
        {

            // Output dizini içindeki tüm resim dosyalarını al
            string[] imageFiles = Directory.GetFiles(outputDirectory);

            string[] sortedNames = new string[imageFiles.Length];
            //Array.Copy(imageFiles, sortedNames, imageFiles.Length);

            // Kopya diziyi sırala
            Array.Sort(imageFiles, (a, b) => File.GetCreationTime(a).CompareTo(File.GetCreationTime(b)));

            if (sortedNames.Length > 1000)
            {
                //Console.WriteLine(imageFiles[0]);
                string oldestImagePath = sortedNames[0];

                // İlk resmi sil
                if (File.Exists(oldestImagePath))
                {
                    // Dosyayı kapat
                    using (FileStream fileStream = new FileStream(oldestImagePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        fileStream.Close();
                    }

                    File.Delete(oldestImagePath);
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
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread th = new Thread(Start);
            th.Start();
        }

        public void Start()
        {
            // RTSP URL of the live video stream
            string rtspUrl = "rtsp://admin:admin@10.3.26.18/profile?token=media_profile1&SessionTimeout=60";

            // Output directory for saving JPEG frames
            string outputDirectory = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\yeni"; // Replace with your desired directory

            Directory.CreateDirectory(outputDirectory);
            

            // Create a process to run ffmpeg
            using (Process process = new Process())
            {
                // Set the command to run ffmpeg

                process.StartInfo.FileName = "ffmpeg";

                process.StartInfo.Arguments = $"-report -i {rtspUrl} -vf fps=15 {outputDirectory}\\{DateTime.Now.ToString("yyyy_dd_MM_HH_mm_ss")}_frame%d.jpg";

                process.StartInfo.UseShellExecute = false;

                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.CreateNoWindow = true;

               
                process.Start();

                SomeMethod("FFMPEG Exe çalışmaya başladı");

                while (isProcessing)
                {
                    byte[] frameData = DequeueFrame();


                    //StreamWriter sw = new StreamWriter("C:\\Users\\yigit\\OneDrive\\Masaüstü\\jpeg_paths.txt", true, Encoding.UTF8);
                    
                    //SomeMethod(filePath);
                    watch.Start();
                    Thread.Sleep(1000);
                    JpegToJson.ImagetoJson("C:\\Users\\yigit\\OneDrive\\Masaüstü\\jpeg_paths.txt");
                    DeleteOldestImageFromDisk(outputDirectory);
                    SomeMethod(watch.StopResult());
                    //JpegToJson.deleteImagesFromDatabase();
                }

                process.WaitForExit();
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
