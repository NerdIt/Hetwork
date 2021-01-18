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
            this.primaryTable = new System.Windows.Forms.TableLayoutPanel();
            this.mainGraph = new Hetwork.NodeGraph();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.nodeMenu1 = new Hetwork.NodeMenu();
            this.primaryTable.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // primaryTable
            // 
            this.primaryTable.ColumnCount = 2;
            this.primaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.625F));
            this.primaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.375F));
            this.primaryTable.Controls.Add(this.mainGraph, 0, 0);
            this.primaryTable.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.primaryTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primaryTable.Location = new System.Drawing.Point(0, 0);
            this.primaryTable.Name = "primaryTable";
            this.primaryTable.RowCount = 1;
            this.primaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.primaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.primaryTable.Size = new System.Drawing.Size(800, 450);
            this.primaryTable.TabIndex = 1;
            this.primaryTable.Paint += new System.Windows.Forms.PaintEventHandler(this.primaryTable_Paint);
            // 
            // mainGraph
            // 
            this.mainGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGraph.BackColor = System.Drawing.Color.White;
            this.mainGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainGraph.ForeColor = System.Drawing.Color.Black;
            this.mainGraph.Location = new System.Drawing.Point(3, 3);
            this.mainGraph.Name = "mainGraph";
            this.mainGraph.Size = new System.Drawing.Size(671, 444);
            this.mainGraph.TabIndex = 0;
            this.mainGraph.NodeSelected += new System.EventHandler(this.nodeGraph1_NodeSelected);
            this.mainGraph.NodeEdited += new System.EventHandler(this.mainGraph_NodeEdited);
            this.mainGraph.Load += new System.EventHandler(this.nodeGraph1_Load);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.17949F));
            this.tableLayoutPanel1.Controls.Add(this.nodeMenu1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(680, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(117, 444);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // nodeMenu1
            // 
            this.nodeMenu1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeMenu1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeMenu1.Location = new System.Drawing.Point(3, 3);
            this.nodeMenu1.Name = "nodeMenu1";
            this.nodeMenu1.Size = new System.Drawing.Size(111, 438);
            this.nodeMenu1.TabIndex = 0;
            // 
            // NodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.primaryTable);
            this.Name = "NodeForm";
            this.Text = "NodeForm";
            this.primaryTable.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel primaryTable;
        private NodeGraph mainGraph;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private NodeMenu nodeMenu1;
    }
}