using System;
using System.Windows.Forms;

namespace VideoToJson
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.VideoProgressBar = new System.Windows.Forms.ProgressBar();
            this.SaveVideoButton = new System.Windows.Forms.Button();
            this.StartInfoLabel = new System.Windows.Forms.Label();
            this.EndInfoLabel = new System.Windows.Forms.Label();
            this.PrevTimeText = new System.Windows.Forms.TextBox();
            this.EndInfoText = new System.Windows.Forms.TextBox();
            this.PrevTimeButton = new System.Windows.Forms.Button();
            this.EndInfoButton = new System.Windows.Forms.Button();
            this.LoadingVideoLabel = new System.Windows.Forms.Label();
            this.LabelControl1 = new System.Windows.Forms.Label();
            this.LabelControl2 = new System.Windows.Forms.Label();
            this.videoTimer = new System.Windows.Forms.Timer(this.components);
            this.FutureLabel = new System.Windows.Forms.Label();
            this.FutureButton = new System.Windows.Forms.Button();
            this.FutureText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // VideoProgressBar
            // 
            this.VideoProgressBar.Location = new System.Drawing.Point(201, 360);
            this.VideoProgressBar.Name = "VideoProgressBar";
            this.VideoProgressBar.Size = new System.Drawing.Size(458, 44);
            this.VideoProgressBar.TabIndex = 0;
            this.VideoProgressBar.Visible = false;
            // 
            // SaveVideoButton
            // 
            this.SaveVideoButton.Location = new System.Drawing.Point(46, 284);
            this.SaveVideoButton.Name = "SaveVideoButton";
            this.SaveVideoButton.Size = new System.Drawing.Size(129, 52);
            this.SaveVideoButton.TabIndex = 1;
            this.SaveVideoButton.Text = "Video Kaydet";
            this.SaveVideoButton.UseVisualStyleBackColor = true;
            this.SaveVideoButton.Click += new System.EventHandler(this.SaveVideoButton_Click);
            // 
            // StartInfoLabel
            // 
            this.StartInfoLabel.AutoSize = true;
            this.StartInfoLabel.Location = new System.Drawing.Point(43, 45);
            this.StartInfoLabel.Name = "StartInfoLabel";
            this.StartInfoLabel.Size = new System.Drawing.Size(220, 32);
            this.StartInfoLabel.TabIndex = 2;
            this.StartInfoLabel.Text = "Videonun başlangıç saniyesini girin.\r\n\r\n";
            // 
            // EndInfoLabel
            // 
            this.EndInfoLabel.AutoSize = true;
            this.EndInfoLabel.Location = new System.Drawing.Point(43, 176);
            this.EndInfoLabel.Name = "EndInfoLabel";
            this.EndInfoLabel.Size = new System.Drawing.Size(185, 16);
            this.EndInfoLabel.TabIndex = 3;
            this.EndInfoLabel.Text = "Videonun bitiş saniyesini girin.";
            // 
            // PrevTimeText
            // 
            this.PrevTimeText.Location = new System.Drawing.Point(46, 96);
            this.PrevTimeText.Name = "PrevTimeText";
            this.PrevTimeText.Size = new System.Drawing.Size(108, 22);
            this.PrevTimeText.TabIndex = 4;
            // 
            // EndInfoText
            // 
            this.EndInfoText.Location = new System.Drawing.Point(46, 223);
            this.EndInfoText.Name = "EndInfoText";
            this.EndInfoText.Size = new System.Drawing.Size(108, 22);
            this.EndInfoText.TabIndex = 5;
            // 
            // PrevTimeButton
            // 
            this.PrevTimeButton.Location = new System.Drawing.Point(173, 96);
            this.PrevTimeButton.Name = "PrevTimeButton";
            this.PrevTimeButton.Size = new System.Drawing.Size(85, 43);
            this.PrevTimeButton.TabIndex = 6;
            this.PrevTimeButton.Text = "Zamanı \r\nKaydet\r\n";
            this.PrevTimeButton.UseVisualStyleBackColor = true;
            this.PrevTimeButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EndInfoButton
            // 
            this.EndInfoButton.Location = new System.Drawing.Point(173, 223);
            this.EndInfoButton.Name = "EndInfoButton";
            this.EndInfoButton.Size = new System.Drawing.Size(85, 43);
            this.EndInfoButton.TabIndex = 7;
            this.EndInfoButton.Text = "Zamanı \r\nKaydet";
            this.EndInfoButton.UseVisualStyleBackColor = true;
            this.EndInfoButton.Click += new System.EventHandler(this.EndButton_Click);
            // 
            // LoadingVideoLabel
            // 
            this.LoadingVideoLabel.AutoSize = true;
            this.LoadingVideoLabel.Location = new System.Drawing.Point(374, 341);
            this.LoadingVideoLabel.Name = "LoadingVideoLabel";
            this.LoadingVideoLabel.Size = new System.Drawing.Size(121, 16);
            this.LoadingVideoLabel.TabIndex = 8;
            this.LoadingVideoLabel.Text = "Video Kaydediliyor";
            this.LoadingVideoLabel.Visible = false;
            // 
            // LabelControl1
            // 
            this.LabelControl1.AutoSize = true;
            this.LabelControl1.Location = new System.Drawing.Point(303, 93);
            this.LabelControl1.Name = "LabelControl1";
            this.LabelControl1.Size = new System.Drawing.Size(0, 16);
            this.LabelControl1.TabIndex = 9;
            // 
            // LabelControl2
            // 
            this.LabelControl2.AutoSize = true;
            this.LabelControl2.Location = new System.Drawing.Point(303, 229);
            this.LabelControl2.Name = "LabelControl2";
            this.LabelControl2.Size = new System.Drawing.Size(0, 16);
            this.LabelControl2.TabIndex = 10;
            // 
            // FutureLabel
            // 
            this.FutureLabel.AutoSize = true;
            this.FutureLabel.Location = new System.Drawing.Point(439, 45);
            this.FutureLabel.Name = "FutureLabel";
            this.FutureLabel.Size = new System.Drawing.Size(266, 16);
            this.FutureLabel.TabIndex = 11;
            this.FutureLabel.Text = "Eğer isterseniz ileriki zaman için saniye girin";
            // 
            // FutureButton
            // 
            this.FutureButton.Location = new System.Drawing.Point(598, 89);
            this.FutureButton.Name = "FutureButton";
            this.FutureButton.Size = new System.Drawing.Size(107, 50);
            this.FutureButton.TabIndex = 12;
            this.FutureButton.Text = "Zamanı\r\n Kaydet";
            this.FutureButton.UseVisualStyleBackColor = true;
            this.FutureButton.Click += new System.EventHandler(this.FutureButton_Click);
            // 
            // FutureText
            // 
            this.FutureText.Location = new System.Drawing.Point(442, 87);
            this.FutureText.Name = "FutureText";
            this.FutureText.Size = new System.Drawing.Size(124, 22);
            this.FutureText.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FutureText);
            this.Controls.Add(this.FutureButton);
            this.Controls.Add(this.FutureLabel);
            this.Controls.Add(this.LabelControl2);
            this.Controls.Add(this.LabelControl1);
            this.Controls.Add(this.LoadingVideoLabel);
            this.Controls.Add(this.EndInfoButton);
            this.Controls.Add(this.PrevTimeButton);
            this.Controls.Add(this.EndInfoText);
            this.Controls.Add(this.PrevTimeText);
            this.Controls.Add(this.EndInfoLabel);
            this.Controls.Add(this.StartInfoLabel);
            this.Controls.Add(this.SaveVideoButton);
            this.Controls.Add(this.VideoProgressBar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar VideoProgressBar;
        private System.Windows.Forms.Button SaveVideoButton;
        private System.Windows.Forms.Label StartInfoLabel;
        private System.Windows.Forms.Label EndInfoLabel;
        private System.Windows.Forms.TextBox PrevTimeText;
        private System.Windows.Forms.TextBox EndInfoText;
        private System.Windows.Forms.Button PrevTimeButton;
        private System.Windows.Forms.Button EndInfoButton;
        private System.Windows.Forms.Label LoadingVideoLabel;
        private Label LabelControl1;
        private Label LabelControl2;
        private Timer videoTimer;
        private Label FutureLabel;
        private Button FutureButton;
        private TextBox FutureText;
    }
}

