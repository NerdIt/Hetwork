namespace NodeIt
{
    partial class NodeGraph
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 0;
            this.toolTip1.BackColor = System.Drawing.Color.White;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 500;
            // 
            // NodeGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Name = "NodeGraph";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintControl);
            this.Enter += new System.EventHandler(this.NodeGraph_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NodeGraph_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NodeGraph_KeyPress);
            this.Leave += new System.EventHandler(this.NodeGraph_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NodeGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NodeGraph_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NodeGraph_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
    }
}
