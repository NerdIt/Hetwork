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
            this.checkListPro1 = new Hetwork.CheckListPro(this);
            this.SuspendLayout();
            // 
            // checkListPro1
            // 
            this.checkListPro1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkListPro1.AutoScroll = true;
            this.checkListPro1.BackColor = System.Drawing.Color.Gainsboro;
            this.checkListPro1.BorderColor = System.Drawing.Color.Black;
            this.checkListPro1.BorderWidth = 0.5F;
            this.checkListPro1.CheckColor = System.Drawing.Color.White;
            this.checkListPro1.ElementColor = System.Drawing.Color.Gainsboro;
            this.checkListPro1.ElementDistance = 3;
            this.checkListPro1.HorizontalPadding = 3;
            this.checkListPro1.HoverCheckColor = System.Drawing.Color.LightGray;
            this.checkListPro1.HoverColor = System.Drawing.Color.LightGray;
            this.checkListPro1.IsCheckedColor = System.Drawing.Color.Gray;
            this.checkListPro1.Location = new System.Drawing.Point(12, 12);
            this.checkListPro1.Name = "checkListPro1";
            this.checkListPro1.ScrollSensitivity = 5;
            this.checkListPro1.SelectedColor = System.Drawing.Color.DarkGray;
            this.checkListPro1.Size = new System.Drawing.Size(150, 426);
            this.checkListPro1.TabIndex = 0;
            this.checkListPro1.TextColor = System.Drawing.Color.Black;
            this.checkListPro1.TextFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkListPro1.UseItemBorders = false;
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