﻿
namespace MagnetLinkConvertForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_magnetpath = new System.Windows.Forms.Button();
            this.btn_torrentpath = new System.Windows.Forms.Button();
            this.btn_downloadpath = new System.Windows.Forms.Button();
            this.txtbox_magnetpath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbox_torrentpath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbox_downloadpath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_magnetpath
            // 
            this.btn_magnetpath.Location = new System.Drawing.Point(432, 45);
            this.btn_magnetpath.Name = "btn_magnetpath";
            this.btn_magnetpath.Size = new System.Drawing.Size(75, 23);
            this.btn_magnetpath.TabIndex = 0;
            this.btn_magnetpath.Text = "Browse";
            this.btn_magnetpath.UseVisualStyleBackColor = true;
            this.btn_magnetpath.Click += new System.EventHandler(this.magnetlink_Path_Click);
            // 
            // btn_torrentpath
            // 
            this.btn_torrentpath.Location = new System.Drawing.Point(432, 102);
            this.btn_torrentpath.Name = "btn_torrentpath";
            this.btn_torrentpath.Size = new System.Drawing.Size(75, 23);
            this.btn_torrentpath.TabIndex = 1;
            this.btn_torrentpath.Text = "Browse";
            this.btn_torrentpath.UseVisualStyleBackColor = true;
            this.btn_torrentpath.Click += new System.EventHandler(this.torrentFile_Path_Click);
            // 
            // btn_downloadpath
            // 
            this.btn_downloadpath.Location = new System.Drawing.Point(432, 161);
            this.btn_downloadpath.Name = "btn_downloadpath";
            this.btn_downloadpath.Size = new System.Drawing.Size(75, 23);
            this.btn_downloadpath.TabIndex = 2;
            this.btn_downloadpath.Text = "Browse";
            this.btn_downloadpath.UseVisualStyleBackColor = true;
            this.btn_downloadpath.Click += new System.EventHandler(this.download_Path_Click);
            // 
            // txtbox_magnetpath
            // 
            this.txtbox_magnetpath.Location = new System.Drawing.Point(28, 44);
            this.txtbox_magnetpath.Name = "txtbox_magnetpath";
            this.txtbox_magnetpath.Size = new System.Drawing.Size(398, 23);
            this.txtbox_magnetpath.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Magnet link watch folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Converted torrent save path";
            // 
            // txtbox_torrentpath
            // 
            this.txtbox_torrentpath.Location = new System.Drawing.Point(28, 103);
            this.txtbox_torrentpath.Name = "txtbox_torrentpath";
            this.txtbox_torrentpath.Size = new System.Drawing.Size(398, 23);
            this.txtbox_torrentpath.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Download path for running torrent";
            // 
            // txtbox_downloadpath
            // 
            this.txtbox_downloadpath.Location = new System.Drawing.Point(28, 161);
            this.txtbox_downloadpath.Name = "txtbox_downloadpath";
            this.txtbox_downloadpath.Size = new System.Drawing.Size(398, 23);
            this.txtbox_downloadpath.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 344);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbox_downloadpath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbox_torrentpath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbox_magnetpath);
            this.Controls.Add(this.btn_downloadpath);
            this.Controls.Add(this.btn_torrentpath);
            this.Controls.Add(this.btn_magnetpath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_magnetpath;
        private System.Windows.Forms.Button btn_torrentpath;
        private System.Windows.Forms.Button btn_downloadpath;
        private System.Windows.Forms.TextBox txtbox_magnetpath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbox_torrentpath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbox_downloadpath;
    }
}
