namespace Hetwork
{
    partial class CheckListPro
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CheckListPro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Name = "CheckListPro";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Paint_Object);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CheckListPro_MouseDown);
            this.MouseLeave += new System.EventHandler(this.CheckListPro_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CheckListPro_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckListPro_MouseUp);
            this.Resize += new System.EventHandler(this.CheckListPro_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
