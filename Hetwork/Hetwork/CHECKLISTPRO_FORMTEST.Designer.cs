namespace Hetwork
{
    partial class CHECKLISTPRO_FORMTEST
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
            this.checkListPro1 = new Hetwork.CheckListPro();
            this.SuspendLayout();
            // 
            // checkListPro1
            // 
            this.checkListPro1.AutoScroll = true;
            this.checkListPro1.BackColor = System.Drawing.Color.Gainsboro;
            this.checkListPro1.Location = new System.Drawing.Point(12, 12);
            this.checkListPro1.Name = "checkListPro1";
            this.checkListPro1.Size = new System.Drawing.Size(150, 59);
            this.checkListPro1.TabIndex = 0;
            // 
            // CHECKLISTPRO_FORMTEST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkListPro1);
            this.Name = "CHECKLISTPRO_FORMTEST";
            this.Text = "CHECKLISTPRO_FORMTEST";
            this.ResumeLayout(false);

        }

        #endregion

        private CheckListPro checkListPro1;
    }
}