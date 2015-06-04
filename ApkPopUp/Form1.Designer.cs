namespace ApkPopUp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnInstall = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ComBxDevices = new System.Windows.Forms.ComboBox();
            this.apkPanel1 = new ApkPopUp.ApkPanel();
            this.SuspendLayout();
            // 
            // BtnInstall
            // 
            this.BtnInstall.Location = new System.Drawing.Point(12, 218);
            this.BtnInstall.Name = "BtnInstall";
            this.BtnInstall.Size = new System.Drawing.Size(121, 23);
            this.BtnInstall.TabIndex = 1;
            this.BtnInstall.Text = "Install";
            this.BtnInstall.UseVisualStyleBackColor = true;
            this.BtnInstall.Click += new System.EventHandler(this.BtnInstall_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(198, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(624, 257);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // ComBxDevices
            // 
            this.ComBxDevices.FormattingEnabled = true;
            this.ComBxDevices.Location = new System.Drawing.Point(12, 247);
            this.ComBxDevices.Name = "ComBxDevices";
            this.ComBxDevices.Size = new System.Drawing.Size(121, 21);
            this.ComBxDevices.TabIndex = 1;
            this.ComBxDevices.SelectedIndexChanged += new System.EventHandler(this.ComBxDevices_SelectedIndexChanged);
            this.ComBxDevices.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ComBxDevices_MouseClick);
            // 
            // apkPanel1
            // 
            this.apkPanel1.apk = null;
            this.apkPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.apkPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.apkPanel1.Location = new System.Drawing.Point(12, 12);
            this.apkPanel1.Name = "apkPanel1";
            this.apkPanel1.Size = new System.Drawing.Size(180, 200);
            this.apkPanel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 281);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.ComBxDevices);
            this.Controls.Add(this.BtnInstall);
            this.Controls.Add(this.apkPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ApkPanel apkPanel1;
        private System.Windows.Forms.Button BtnInstall;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox ComBxDevices;
    }
}

