using System.Windows.Forms;

namespace LabelClear
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
        /// <param name="disposing">true if managed resources should be disposed; otherwise,</param>
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
            menuStrip1 = new MenuStrip();
            fileStripMenuItem = new ToolStripMenuItem();
            openPathStripMenuItem = new ToolStripMenuItem();
            pathTreeView = new TreeView();
            pictureBox = new PictureBox();
            splitContainer1 = new SplitContainer();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1274, 39);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileStripMenuItem
            // 
            fileStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openPathStripMenuItem });
            fileStripMenuItem.Name = "fileStripMenuItem";
            fileStripMenuItem.Size = new Size(82, 35);
            fileStripMenuItem.Text = "文件";
            // 
            // openPathStripMenuItem
            // 
            openPathStripMenuItem.Name = "openPathStripMenuItem";
            openPathStripMenuItem.Size = new Size(195, 44);
            openPathStripMenuItem.Text = "打开";
            openPathStripMenuItem.Click += openPathStripMenuItem_Click;
            // 
            // pathTreeView
            // 
            pathTreeView.BackColor = SystemColors.Window;
            pathTreeView.Dock = DockStyle.Fill;
            pathTreeView.Location = new Point(0, 0);
            pathTreeView.Name = "pathTreeView";
            pathTreeView.Size = new Size(270, 690);
            pathTreeView.TabIndex = 1;
            pathTreeView.AfterSelect += pathTreeView_AfterSelect;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.ActiveCaption;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(1000, 690);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 3;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 39);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pathTreeView);
            splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBox);
            splitContainer1.Panel2MinSize = 100;
            splitContainer1.Size = new Size(1274, 690);
            splitContainer1.SplitterDistance = 270;
            splitContainer1.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1274, 729);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileStripMenuItem;
        private ToolStripMenuItem openPathStripMenuItem;
        private TreeView pathTreeView;
        private PictureBox pictureBox;
        private SplitContainer splitContainer1;
    }
}
