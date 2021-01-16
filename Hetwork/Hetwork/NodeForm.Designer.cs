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
            this.propertiesTable = new System.Windows.Forms.TableLayoutPanel();
            this.nodeTitleLabel = new System.Windows.Forms.Label();
            this.contentDisplayPanel = new System.Windows.Forms.Panel();
            this.primaryTable.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.propertiesTable.SuspendLayout();
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
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.82051F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.17949F));
            this.tableLayoutPanel1.Controls.Add(this.propertiesTable, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(680, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(117, 444);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // propertiesTable
            // 
            this.propertiesTable.ColumnCount = 1;
            this.propertiesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.propertiesTable.Controls.Add(this.nodeTitleLabel, 0, 0);
            this.propertiesTable.Controls.Add(this.contentDisplayPanel, 0, 1);
            this.propertiesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesTable.Location = new System.Drawing.Point(17, 3);
            this.propertiesTable.Name = "propertiesTable";
            this.propertiesTable.RowCount = 2;
            this.propertiesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.538462F));
            this.propertiesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.46153F));
            this.propertiesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.propertiesTable.Size = new System.Drawing.Size(97, 438);
            this.propertiesTable.TabIndex = 2;
            // 
            // nodeTitleLabel
            // 
            this.nodeTitleLabel.AutoSize = true;
            this.nodeTitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.nodeTitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeTitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeTitleLabel.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.nodeTitleLabel.Location = new System.Drawing.Point(3, 0);
            this.nodeTitleLabel.Name = "nodeTitleLabel";
            this.nodeTitleLabel.Size = new System.Drawing.Size(91, 28);
            this.nodeTitleLabel.TabIndex = 0;
            this.nodeTitleLabel.Text = "<Name>";
            this.nodeTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nodeTitleLabel.TextChanged += new System.EventHandler(this.nodeTitleLabel_TextChanged);
            // 
            // contentDisplayPanel
            // 
            this.contentDisplayPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.contentDisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contentDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentDisplayPanel.Location = new System.Drawing.Point(3, 31);
            this.contentDisplayPanel.Name = "contentDisplayPanel";
            this.contentDisplayPanel.Size = new System.Drawing.Size(91, 404);
            this.contentDisplayPanel.TabIndex = 3;
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
            this.propertiesTable.ResumeLayout(false);
            this.propertiesTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel primaryTable;
        private NodeGraph mainGraph;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel propertiesTable;
        private System.Windows.Forms.Label nodeTitleLabel;
        private System.Windows.Forms.Panel contentDisplayPanel;
    }
}