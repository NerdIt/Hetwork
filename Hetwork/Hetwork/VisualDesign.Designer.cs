
namespace Hetwork
{
    partial class VisualDesign
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
            this.nodeMenu1 = new Hetwork.NodeMenu();
            this.SuspendLayout();
            // 
            // nodeMenu1
            // 
            this.nodeMenu1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeMenu1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeMenu1.Location = new System.Drawing.Point(604, 12);
            this.nodeMenu1.Name = "nodeMenu1";
            this.nodeMenu1.Size = new System.Drawing.Size(184, 426);
            this.nodeMenu1.TabIndex = 0;
            // 
            // VisualDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nodeMenu1);
            this.Name = "VisualDesign";
            this.Text = "VisualDesign";
            this.ResumeLayout(false);

        }

        #endregion

        private NodeMenu nodeMenu1;
    }
}