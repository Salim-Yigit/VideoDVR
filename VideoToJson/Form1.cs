using System;
using System.Drawing;
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
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Drawing;
using MongoDB.Bson.IO;

namespace VideoToJson
{
    public partial class Form1 : Form
    {
        Watch watch;
        private int processMinute = 5;
        private int prevTime; 
        private int futureTime;
        Thread th;

        public Form1()
        {
            InitializeComponent();
            watch = new Watch();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            th = new Thread(StartSavingVideo);
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
        public void StartSavingVideo()
        {
            // RTSP URL of the live video stream
            string rtspUrl = "rtsp://admin:admin@10.3.26.18/profile?token=media_profile1&SessionTimeout=60";
            string outputDirectory;
            int currentMinute = 0;
            PrintResult("Program Başladı.");
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

        private void CreateVideo()
        {   
            DateTime getPrev,getNext;
            DateTime now = DateTime.Now;

            getPrev = now.AddSeconds(-prevTime);
            getNext = now.AddSeconds(futureTime);

            int indexStart = 900 - (this.prevTime*15);
            int indexFinish = this.futureTime * 15;

            string connectionString = "mongodb://localhost:27017"; // MongoDB sunucu bağlantı adresi

            IMongoClient mongoClient = new MongoClient(connectionString);
            IMongoDatabase database = mongoClient.GetDatabase("LiveVideo");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Frames");

            for (int i = indexStart;i<901;i++)
            {
                var filter = new BsonDocument("Index", new BsonInt32(i));
                var data = collection.Find(filter).ToList();
                foreach (var j in data)
                {
                    string base64Stirng = j["Image"].AsString;
                    byte[] byteArray = Convert.FromBase64String(base64Stirng); 

                    using(MemoryStream memoryStream = new MemoryStream(byteArray))
                    {
                        Image image = Image.FromStream(memoryStream);

                        image.Save("C:\\Users\\yigit\\OneDrive\\Masaüstü\\VideoResimleri\\" + "frame" + j["Index"].ToString() + ".jpg");
                    }
                    
                    
                }
            }

            for (int i = 1; i < indexFinish; i++)
            {
                var filter = new BsonDocument("Index", new BsonInt32(i));
                var data = collection.Find(filter).ToList();
                foreach (var j in data)
                {
                    string base64Stirng = j["Image"].AsString;
                    byte[] byteArray = Convert.FromBase64String(base64Stirng);

                    using (MemoryStream memoryStream = new MemoryStream(byteArray))
                    {
                        Image image = Image.FromStream(memoryStream);

                        image.Save("C:\\Users\\yigit\\OneDrive\\Masaüstü\\VideoResimleri\\" + "frame" + j["Index"].ToString()+".jpg");
                    }


                }
            }
        }

        private void PrintResult(string result)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => PrintResult(this, result)));
            else
                PrintResult(this, result);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetProcessesByName("ffmpeg")[0].Kill();
            Environment.Exit(0);
        }
        public void KillProcess(int pid)
        {
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
        public void PrintResult(Form frm, string asdf) { frm.Text = asdf; }

        private void SaveVideoButton_Click(object sender, EventArgs e)
        {
            // SaveVideoButton'a tıklanınca LoadingVideoLabel ve LoadingVideoProcessBar görünür yapılır.
            LoadingVideoLabel.Visible = true;
            VideoProgressBar.Visible = true;
            CreateVideo();
            // Resimleri videoya çevirecek kod lazım.
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            string kullaniciVerisi = PrevTimeText.Text;
            int prevTime;

            if (int.TryParse(kullaniciVerisi, out prevTime))
            {
                
                this.prevTime = prevTime;
                LabelControl1.Text = prevTime.ToString();
            }
           
        }

        private void FutureButton_Click(object sender, EventArgs e)
        {
            
            string kullaniciVerisi = FutureInfoText.Text;
            int futureTime;
            if (int.TryParse(kullaniciVerisi, out futureTime))
            {
                this.futureTime = futureTime;
                LabelControl2.Text = futureTime.ToString();
            }
           
        }
        private void LabelControl2_Click(object sender, EventArgs e)
        {

        }
    }
}
