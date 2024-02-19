
namespace ProjetoZeta1
{
    partial class Communication_Manager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Communication_Manager));
            this.ZetaTabPage = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnReadFiles = new System.Windows.Forms.Button();
            this.txt1_FilePathSet = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_SEND1 = new System.Windows.Forms.Button();
            this.btnCreateFiles = new System.Windows.Forms.Button();
            this.ZetaPage = new System.Windows.Forms.TabControl();
            this.ZetaTabPage.SuspendLayout();
            this.ZetaPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ZetaTabPage
            // 
            this.ZetaTabPage.Controls.Add(this.button1);
            this.ZetaTabPage.Controls.Add(this.btnReadFiles);
            this.ZetaTabPage.Controls.Add(this.txt1_FilePathSet);
            this.ZetaTabPage.Controls.Add(this.btn_close);
            this.ZetaTabPage.Controls.Add(this.btn_SEND1);
            this.ZetaTabPage.Controls.Add(this.btnCreateFiles);
            this.ZetaTabPage.Cursor = System.Windows.Forms.Cursors.Default;
            this.ZetaTabPage.Location = new System.Drawing.Point(4, 22);
            this.ZetaTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.ZetaTabPage.Name = "ZetaTabPage";
            this.ZetaTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.ZetaTabPage.Size = new System.Drawing.Size(510, 251);
            this.ZetaTabPage.TabIndex = 0;
            this.ZetaTabPage.Text = "Zeta";
            this.ZetaTabPage.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(266, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Print";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnReadFiles
            // 
            this.btnReadFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadFiles.AutoSize = true;
            this.btnReadFiles.Location = new System.Drawing.Point(347, 196);
            this.btnReadFiles.Margin = new System.Windows.Forms.Padding(2);
            this.btnReadFiles.Name = "btnReadFiles";
            this.btnReadFiles.Size = new System.Drawing.Size(159, 23);
            this.btnReadFiles.TabIndex = 0;
            this.btnReadFiles.Text = "Read Machine Files";
            this.btnReadFiles.UseVisualStyleBackColor = true;
            this.btnReadFiles.Click += new System.EventHandler(this.btnReadFiles_Click);
            // 
            // txt1_FilePathSet
            // 
            this.txt1_FilePathSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt1_FilePathSet.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.txt1_FilePathSet.Location = new System.Drawing.Point(4, 4);
            this.txt1_FilePathSet.Margin = new System.Windows.Forms.Padding(2);
            this.txt1_FilePathSet.Name = "txt1_FilePathSet";
            this.txt1_FilePathSet.Size = new System.Drawing.Size(502, 20);
            this.txt1_FilePathSet.TabIndex = 3;
            this.txt1_FilePathSet.Text = "Enter File Path";
            this.txt1_FilePathSet.UseWaitCursor = true;
            this.txt1_FilePathSet.TextChanged += new System.EventHandler(this.txt1_FilePathSet_TextChanged);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_close.Location = new System.Drawing.Point(5, 221);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 6;
            this.btn_close.Text = "Quit";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_SEND1
            // 
            this.btn_SEND1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SEND1.Location = new System.Drawing.Point(347, 221);
            this.btn_SEND1.Name = "btn_SEND1";
            this.btn_SEND1.Size = new System.Drawing.Size(50, 23);
            this.btn_SEND1.TabIndex = 5;
            this.btn_SEND1.Text = "SEND";
            this.btn_SEND1.UseVisualStyleBackColor = true;
            this.btn_SEND1.Click += new System.EventHandler(this.btn_SEND1_Click);
            // 
            // btnCreateFiles
            // 
            this.btnCreateFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateFiles.AutoSize = true;
            this.btnCreateFiles.Location = new System.Drawing.Point(402, 221);
            this.btnCreateFiles.Margin = new System.Windows.Forms.Padding(2);
            this.btnCreateFiles.Name = "btnCreateFiles";
            this.btnCreateFiles.Size = new System.Drawing.Size(104, 23);
            this.btnCreateFiles.TabIndex = 4;
            this.btnCreateFiles.Text = "Create Files";
            this.btnCreateFiles.UseVisualStyleBackColor = true;
            this.btnCreateFiles.Click += new System.EventHandler(this.btnCreateFiles_Click);
            // 
            // ZetaPage
            // 
            this.ZetaPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZetaPage.Controls.Add(this.ZetaTabPage);
            this.ZetaPage.Location = new System.Drawing.Point(7, 9);
            this.ZetaPage.Margin = new System.Windows.Forms.Padding(2);
            this.ZetaPage.Name = "ZetaPage";
            this.ZetaPage.SelectedIndex = 0;
            this.ZetaPage.Size = new System.Drawing.Size(518, 277);
            this.ZetaPage.TabIndex = 6;
            this.ZetaPage.Tag = "";
            // 
            // Communication_Manager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 292);
            this.Controls.Add(this.ZetaPage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Communication_Manager";
            this.Text = "Communication Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ZetaTabPage.ResumeLayout(false);
            this.ZetaTabPage.PerformLayout();
            this.ZetaPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage ZetaTabPage;
        private System.Windows.Forms.Button btnReadFiles;
        private System.Windows.Forms.TextBox txt1_FilePathSet;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_SEND1;
        private System.Windows.Forms.Button btnCreateFiles;
        private System.Windows.Forms.TabControl ZetaPage;
        private System.Windows.Forms.Button button1;
    }
}

