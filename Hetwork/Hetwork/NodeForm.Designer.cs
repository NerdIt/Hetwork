namespace Hetwork
{
    partial class NodeForm
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
            this.nodeGraph1 = new Hetwork.NodeGraph();
            this.SuspendLayout();
            // 
            // nodeGraph1
            // 
            this.nodeGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeGraph1.BackColor = System.Drawing.Color.White;
            this.nodeGraph1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeGraph1.ForeColor = System.Drawing.Color.Black;
            this.nodeGraph1.Location = new System.Drawing.Point(13, 13);
            this.nodeGraph1.Name = "nodeGraph1";
            this.nodeGraph1.Size = new System.Drawing.Size(556, 425);
            this.nodeGraph1.TabIndex = 0;
            this.nodeGraph1.Load += new System.EventHandler(this.nodeGraph1_Load);
            // 
            // NodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nodeGraph1);
            this.Name = "NodeForm";
            this.Text = "NodeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private NodeGraph nodeGraph1;
    }
}