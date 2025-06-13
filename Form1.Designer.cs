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
            FlowLayoutPanel flowLayoutPanel1;
            btnPrev = new Button();
            btnNext = new Button();
            btnSave = new Button();
            cancelButton = new Button();
            menuStrip1 = new MenuStrip();
            fileStripMenuItem = new ToolStripMenuItem();
            openPathStripMenuItem = new ToolStripMenuItem();
            pathTreeView = new TreeView();
            pictureBox = new PictureBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            jsonPreviewBox = new RichTextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Info;
            flowLayoutPanel1.Controls.Add(btnPrev);
            flowLayoutPanel1.Controls.Add(btnNext);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Controls.Add(cancelButton);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(623, 56);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(3, 3);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(150, 46);
            btnPrev.TabIndex = 0;
            btnPrev.Text = "上一张";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(159, 3);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(150, 46);
            btnNext.TabIndex = 1;
            btnNext.Text = "下一张";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(315, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 46);
            btnSave.TabIndex = 2;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(471, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(149, 46);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "取消编辑";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Visible = false;
            cancelButton.Click += cancelButton_Click;
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
            pathTreeView.Size = new Size(345, 690);
            pathTreeView.TabIndex = 1;
            pathTreeView.AfterSelect += pathTreeView_AfterSelect;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.ActiveCaption;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(0, 56);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(623, 634);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 3;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
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
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel2MinSize = 100;
            splitContainer1.Size = new Size(972, 690);
            splitContainer1.SplitterDistance = 345;
            splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 39);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(jsonPreviewBox);
            splitContainer2.Size = new Size(1274, 690);
            splitContainer2.SplitterDistance = 972;
            splitContainer2.TabIndex = 5;
            // 
            // jsonPreviewBox
            // 
            jsonPreviewBox.Dock = DockStyle.Fill;
            jsonPreviewBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            jsonPreviewBox.Location = new Point(0, 0);
            jsonPreviewBox.Name = "jsonPreviewBox";
            jsonPreviewBox.ReadOnly = true;
            jsonPreviewBox.Size = new Size(298, 690);
            jsonPreviewBox.TabIndex = 0;
            jsonPreviewBox.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HighlightText;
            ClientSize = new Size(1274, 729);
            Controls.Add(splitContainer2);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            flowLayoutPanel1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
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
        private SplitContainer splitContainer2;
        private RichTextBox jsonPreviewBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnPrev;
        private Button btnNext;
        private Button btnSave;
        private Button cancelButton;
    }
}
