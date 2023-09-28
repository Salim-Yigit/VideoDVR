using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Drawing;
using Accord.Video;
using SharpCompress.Writers;
using Accord.Video.FFMPEG;

namespace VideoToJson
{
    public partial class Form1 : Form
    {
        Watch watch;
        private int processMinute = 5;
        private int startTime; 
        private int futureTime;
        private int endTime;
        private string outputDirectory;
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
            string year = DateTime.Now.Year.ToString("00"); 
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
            
            string rtspUrl = "rtsp://admin:admin@10.3.26.18/profile?token=media_profile1&SessionTimeout=60";
            int currentMinute = 0;
            PrintResult("Program Başladı.");
            while(currentMinute < processMinute)
            {
                FolderCreate("C:\\Users\\yigit\\OneDrive\\Masaüstü\\RTSP"); 
                outputDirectory = UpdateOutputDirectory();

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\EskiHali\\VideoSaveToDatabase\\VideoToJson\\bin\\Debug\\ffmpeg.exe";

                    process.StartInfo.Arguments = $"-i {rtspUrl} -vf fps=30 {outputDirectory}\\frame_%d.jpg";


                    process.StartInfo.UseShellExecute = false;

                    process.StartInfo.RedirectStandardOutput = true;

                    process.StartInfo.CreateNoWindow = true;    

                    process.Start();

                    while (!File.Exists(Path.Combine(outputDirectory, "frame_1.jpg")))
                    {

                    }
                    watch.Start();
                    JpegToJson.ImagetoJson(outputDirectory, 1801,currentMinute);
                    PrintResult(watch.StopResult());
                    process.Kill();
                    process.Close();
                }
                currentMinute++;
            }
            PrintResult("Program Sonlandı!!!!!!!!!!!!!!!");    
            
        } 


        private List<string> DecodeReturnImages()
        {
            string connectionString = "mongodb://localhost:27017"; // MongoDB sunucu bağlantı adresi
            string databaseName = "LiveVideo";
            string collectionName = "Frames";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);

            List<string> imagesPaths = new List<string>();

            int start = this.startTime * 30;
            int finish = this.endTime * 30;
            int future = this.futureTime * 30;
            int gecici = this.futureTime - this.startTime;

            if (this.futureTime > 0)
            {
                while (gecici > 0)
                {
                    Thread.Sleep(1000);
                    ProgressBarFill();
                    gecici--;
                }
                for (int i = start; i < future; i++)
                {
                    var filter = new BsonDocument("Index", new BsonInt32(i));
                    var data = collection.Find(filter).ToList();
                    foreach (var path in data)
                    {
                        string tmp = path["FileNamePath"].AsString;
                        imagesPaths.Add(tmp);
                    }

                }
            }else
            {   
                for (int i = start; i < finish; i++)
                {
                    var filter = new BsonDocument("Index", new BsonInt32(i));
                    var data = collection.Find(filter).ToList();
                    foreach (var path in data)
                    {
                        string tmp = path["FileNamePath"].AsString;
                        imagesPaths.Add(tmp);
                    }

                }
            }

            
            return imagesPaths; 
        }
        private void CreateVideo()
        {
            List<string> imagesPaths = DecodeReturnImages();
            string outputFile = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\video.mp4";
            int frameRate = 30;
            int width = 1280;
            int height = 720;

            Accord.Video.FFMPEG.VideoFileWriter videoFileWriter = new Accord.Video.FFMPEG.VideoFileWriter();

            videoFileWriter.Open(outputFile, width, height,frameRate, VideoCodec.MPEG4);

            if(futureTime > 0)
            {
                foreach (string path in imagesPaths)
                {
                    // Resmi yükleyin.
                    Image image = Image.FromFile(path);
                    videoFileWriter.WriteVideoFrame((Bitmap)image);
                }
                videoFileWriter.Close();
                PrintResult("Video Kayıt İşlemi Bitti");
                LoadingVideoLabel.Text = "Video Kayıt İşlemi Bitti.";
            }else
            {
                foreach (string path in imagesPaths)
                {
                    // Resmi yükleyin.
                    Image image = Image.FromFile(path);
                    videoFileWriter.WriteVideoFrame((Bitmap)image);
                    ProgressBarFill();
                }
                videoFileWriter.Close();
                PrintResult("Video Kayıt İşlemi Bitti");
                LoadingVideoLabel.Text = "Video Kayıt İşlemi Bitti.";
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
                
            }

            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcess(Convert.ToInt32(mo["ProcessID"])); 
                }
            }
        }
        public void PrintResult(Form frm, string asdf) { frm.Text = asdf; }

        private void SaveVideoButton_Click(object sender, EventArgs e)
        {
            this.videoTimer.Start();
            CreateVideo();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            string kullaniciVerisi = PrevTimeText.Text;
            int prevTime;

            if (int.TryParse(kullaniciVerisi, out prevTime))
            {
                
                this.startTime = prevTime;
                LabelControl1.Text = prevTime.ToString();
            }
            
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            string kullaniciVerisi = EndInfoText.Text;
            int endTime;
            if (int.TryParse(kullaniciVerisi, out endTime))
            {
                this.endTime = endTime;
                LabelControl2.Text = endTime.ToString();
            }
            VideoProgressBar.Visible = true;
            LoadingVideoLabel.Visible = true;
            LoadingVideoLabel.Text = "Video Kaydediliyor...";
        }

        private void FutureButton_Click(object sender, EventArgs e)
        {
            string kullaniciVerisi = FutureText.Text;
            int futureTime;
            if (int.TryParse(kullaniciVerisi, out futureTime))
            {
                this.futureTime = futureTime;
                LabelControl2.Text = futureTime.ToString();
            }
            VideoProgressBar.Visible = true;
            LoadingVideoLabel.Visible = true;
            LoadingVideoLabel.Text = "Video Kaydediliyor...";
        }

        private void ProgressBarFill()
        {   
            if(this.futureTime > 0 )
            {
                int artisMiktari = 100 / (futureTime - startTime);
                this.VideoProgressBar.Increment(artisMiktari);
            }else
            {
                int artisMiktari = endTime - startTime;
                this.VideoProgressBar.Increment(artisMiktari);
            }
            
        }

      
    }
}
