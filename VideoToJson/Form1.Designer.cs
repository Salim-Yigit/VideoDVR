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
            this.VideoProgressBar = new System.Windows.Forms.ProgressBar();
            this.SaveVideoButton = new System.Windows.Forms.Button();
            this.StartInfoLabel = new System.Windows.Forms.Label();
            this.FutureInfoLabel = new System.Windows.Forms.Label();
            this.PrevTimeText = new System.Windows.Forms.TextBox();
            this.FutureInfoText = new System.Windows.Forms.TextBox();
            this.PrevTimeButton = new System.Windows.Forms.Button();
            this.FutureInfoButton = new System.Windows.Forms.Button();
            this.LoadingVideoLabel = new System.Windows.Forms.Label();
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
            this.SaveVideoButton.Location = new System.Drawing.Point(46, 287);
            this.SaveVideoButton.Name = "SaveVideoButton";
            this.SaveVideoButton.Size = new System.Drawing.Size(129, 52);
            this.SaveVideoButton.TabIndex = 1;
            this.SaveVideoButton.Text = "Video Kaydet";
            this.SaveVideoButton.UseVisualStyleBackColor = true;
            // 
            // StartInfoLabel
            // 
            this.StartInfoLabel.AutoSize = true;
            this.StartInfoLabel.Location = new System.Drawing.Point(43, 45);
            this.StartInfoLabel.Name = "StartInfoLabel";
            this.StartInfoLabel.Size = new System.Drawing.Size(440, 48);
            this.StartInfoLabel.TabIndex = 2;
            this.StartInfoLabel.Text = "Video kaydetme işlemini şu andan kaç saniye önce başlatmak istersiniz?\r\n(Saniye o" +
    "larak giriniz.)\r\n\r\n";
            this.StartInfoLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // FutureInfoLabel
            // 
            this.FutureInfoLabel.AutoSize = true;
            this.FutureInfoLabel.Location = new System.Drawing.Point(43, 176);
            this.FutureInfoLabel.Name = "FutureInfoLabel";
            this.FutureInfoLabel.Size = new System.Drawing.Size(462, 32);
            this.FutureInfoLabel.TabIndex = 3;
            this.FutureInfoLabel.Text = "Video kaydetme işlemini şu andan kaç saniye sonra sonlandırmak istersiniz?\r\n(Sani" +
    "ye olarak giriniz.)";
            this.FutureInfoLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // PrevTimeText
            // 
            this.PrevTimeText.Location = new System.Drawing.Point(46, 96);
            this.PrevTimeText.Name = "PrevTimeText";
            this.PrevTimeText.Size = new System.Drawing.Size(108, 22);
            this.PrevTimeText.TabIndex = 4;
            // 
            // FutureInfoText
            // 
            this.FutureInfoText.Location = new System.Drawing.Point(46, 223);
            this.FutureInfoText.Name = "FutureInfoText";
            this.FutureInfoText.Size = new System.Drawing.Size(108, 22);
            this.FutureInfoText.TabIndex = 5;
            // 
            // PrevTimeButton
            // 
            this.PrevTimeButton.Location = new System.Drawing.Point(173, 96);
            this.PrevTimeButton.Name = "PrevTimeButton";
            this.PrevTimeButton.Size = new System.Drawing.Size(85, 43);
            this.PrevTimeButton.TabIndex = 6;
            this.PrevTimeButton.Text = "Zamanı \r\nKaydet\r\n";
            this.PrevTimeButton.UseVisualStyleBackColor = true;
            this.PrevTimeButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // FutureInfoButton
            // 
            this.FutureInfoButton.Location = new System.Drawing.Point(173, 223);
            this.FutureInfoButton.Name = "FutureInfoButton";
            this.FutureInfoButton.Size = new System.Drawing.Size(85, 43);
            this.FutureInfoButton.TabIndex = 7;
            this.FutureInfoButton.Text = "Zamanı \r\nKaydet";
            this.FutureInfoButton.UseVisualStyleBackColor = true;
            // 
            // LoadingVideoLabel
            // 
            this.LoadingVideoLabel.AutoSize = true;
            this.LoadingVideoLabel.Location = new System.Drawing.Point(374, 341);
            this.LoadingVideoLabel.Name = "LoadingVideoLabel";
            this.LoadingVideoLabel.Size = new System.Drawing.Size(121, 16);
            this.LoadingVideoLabel.TabIndex = 8;
            this.LoadingVideoLabel.Text = "Video Kaydediliyor";
            this.LoadingVideoLabel.Click += new System.EventHandler(this.label3_Click);
            this.LoadingVideoLabel.Visible = false; 
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoadingVideoLabel);
            this.Controls.Add(this.FutureInfoButton);
            this.Controls.Add(this.PrevTimeButton);
            this.Controls.Add(this.FutureInfoText);
            this.Controls.Add(this.PrevTimeText);
            this.Controls.Add(this.FutureInfoLabel);
            this.Controls.Add(this.StartInfoLabel);
            this.Controls.Add(this.SaveVideoButton);
            this.Controls.Add(this.VideoProgressBar);
            this.Name = "Form1";
            this.Text = "Form1";
            //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar VideoProgressBar;
        private System.Windows.Forms.Button SaveVideoButton;
        private System.Windows.Forms.Label StartInfoLabel;
        private System.Windows.Forms.Label FutureInfoLabel;
        private System.Windows.Forms.TextBox PrevTimeText;
        private System.Windows.Forms.TextBox FutureInfoText;
        private System.Windows.Forms.Button PrevTimeButton;
        private System.Windows.Forms.Button FutureInfoButton;
        private System.Windows.Forms.Label LoadingVideoLabel;
    }
}

