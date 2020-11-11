namespace Drag_and_Drop_Process_Queue
{
    partial class ProcessQueue
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox_left = new System.Windows.Forms.ListBox();
            this.listBox_right = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 423);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox_left);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox_right);
            this.splitContainer1.Size = new System.Drawing.Size(800, 423);
            this.splitContainer1.SplitterDistance = 419;
            this.splitContainer1.TabIndex = 0;
            // 
            // listBox_left
            // 
            this.listBox_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_left.FormattingEnabled = true;
            this.listBox_left.Location = new System.Drawing.Point(0, 0);
            this.listBox_left.Name = "listBox_left";
            this.listBox_left.Size = new System.Drawing.Size(419, 423);
            this.listBox_left.TabIndex = 0;
            this.listBox_left.DragDrop += new System.Windows.Forms.DragEventHandler(this.generic_DragDrop);
            this.listBox_left.DragEnter += new System.Windows.Forms.DragEventHandler(this.generic_DragEnter);
            this.listBox_left.DragOver += new System.Windows.Forms.DragEventHandler(this.generic_DragOver);
            this.listBox_left.DragLeave += new System.EventHandler(this.generic_DragLeave);
            this.listBox_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.generic_MouseDown);
            // 
            // listBox_right
            // 
            this.listBox_right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_right.FormattingEnabled = true;
            this.listBox_right.Location = new System.Drawing.Point(0, 0);
            this.listBox_right.Name = "listBox_right";
            this.listBox_right.Size = new System.Drawing.Size(377, 423);
            this.listBox_right.TabIndex = 0;
            this.listBox_right.DragDrop += new System.Windows.Forms.DragEventHandler(this.generic_DragDrop);
            this.listBox_right.DragEnter += new System.Windows.Forms.DragEventHandler(this.generic_DragEnter);
            this.listBox_right.DragOver += new System.Windows.Forms.DragEventHandler(this.generic_DragOver);
            this.listBox_right.DragLeave += new System.EventHandler(this.generic_DragLeave);
            this.listBox_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.generic_MouseDown);
            // 
            // ProcessQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "ProcessQueue";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox_left;
        private System.Windows.Forms.ListBox listBox_right;
    }
}

