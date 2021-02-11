namespace NodeIt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeForm));
            this.primaryTable = new System.Windows.Forms.TableLayoutPanel();
            this.mainGraph = new NodeIt.NodeGraph();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.nodeMenu1 = new NodeIt.NodeMenu();
            this.nodeFormMenu = new System.Windows.Forms.ToolStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.ts0 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts3 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpBtn = new System.Windows.Forms.ToolStripButton();
            this.primaryTable.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.nodeFormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // primaryTable
            // 
            this.primaryTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryTable.ColumnCount = 2;
            this.primaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.625F));
            this.primaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.375F));
            this.primaryTable.Controls.Add(this.mainGraph, 0, 0);
            this.primaryTable.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.primaryTable.Location = new System.Drawing.Point(0, 28);
            this.primaryTable.Name = "primaryTable";
            this.primaryTable.RowCount = 1;
            this.primaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.primaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 422F));
            this.primaryTable.Size = new System.Drawing.Size(800, 422);
            this.primaryTable.TabIndex = 1;
            this.primaryTable.Paint += new System.Windows.Forms.PaintEventHandler(this.primaryTable_Paint);
            // 
            // mainGraph
            // 
            this.mainGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGraph.BackColor = System.Drawing.Color.Transparent;
            this.mainGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainGraph.ForeColor = System.Drawing.Color.Black;
            this.mainGraph.Location = new System.Drawing.Point(3, 3);
            this.mainGraph.Name = "mainGraph";
            this.mainGraph.Size = new System.Drawing.Size(671, 416);
            this.mainGraph.TabIndex = 0;
            this.mainGraph.NodeSelected += new System.EventHandler(this.nodeGraph1_NodeSelected);
            this.mainGraph.NodeEdited += new System.EventHandler(this.mainGraph_NodeEdited);
            this.mainGraph.Load += new System.EventHandler(this.nodeGraph1_Load);
            this.mainGraph.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainGraph_KeyDown);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(117, 416);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // nodeMenu1
            // 
            this.nodeMenu1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeMenu1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeMenu1.Location = new System.Drawing.Point(3, 3);
            this.nodeMenu1.Name = "nodeMenu1";
            this.nodeMenu1.Size = new System.Drawing.Size(111, 410);
            this.nodeMenu1.TabIndex = 0;
            this.nodeMenu1.ControlUpdated += new System.EventHandler(this.nodeMenu1_ControlUpdated);
            this.nodeMenu1.MenuKeyDown += new System.Windows.Forms.KeyEventHandler(this.nodeMenu1_MenuKeyDown);
            // 
            // nodeFormMenu
            // 
            this.nodeFormMenu.BackColor = System.Drawing.Color.White;
            this.nodeFormMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.helpBtn});
            this.nodeFormMenu.Location = new System.Drawing.Point(0, 0);
            this.nodeFormMenu.Name = "nodeFormMenu";
            this.nodeFormMenu.Size = new System.Drawing.Size(800, 25);
            this.nodeFormMenu.TabIndex = 2;
            this.nodeFormMenu.Text = "windowForm";
            // 
            // FileMenu
            // 
            this.FileMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts0,
            this.ts1,
            this.ts2,
            this.ts3});
            this.FileMenu.Image = ((System.Drawing.Image)(resources.GetObject("FileMenu.Image")));
            this.FileMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(38, 22);
            this.FileMenu.Text = "File";
            this.FileMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.FileMenu_DropDownItemClicked);
            // 
            // ts0
            // 
            this.ts0.Name = "ts0";
            this.ts0.Size = new System.Drawing.Size(164, 22);
            this.ts0.Text = "New (CTRL + N)";
            // 
            // ts1
            // 
            this.ts1.Name = "ts1";
            this.ts1.Size = new System.Drawing.Size(164, 22);
            this.ts1.Text = "Open (CTRL + O)";
            // 
            // ts2
            // 
            this.ts2.Name = "ts2";
            this.ts2.Size = new System.Drawing.Size(164, 22);
            this.ts2.Text = "Save (CTRL + S)";
            // 
            // ts3
            // 
            this.ts3.Name = "ts3";
            this.ts3.Size = new System.Drawing.Size(164, 22);
            this.ts3.Text = "Close";
            // 
            // helpBtn
            // 
            this.helpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.helpBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpBtn.Image")));
            this.helpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(36, 22);
            this.helpBtn.Text = "Help";
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // NodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nodeFormMenu);
            this.Controls.Add(this.primaryTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NodeForm";
            this.Tag = "MainForm";
            this.Text = "NodeIt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NodeForm_FormClosing);
            this.primaryTable.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.nodeFormMenu.ResumeLayout(false);
            this.nodeFormMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel primaryTable;
        private NodeGraph mainGraph;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private NodeMenu nodeMenu1;
        private System.Windows.Forms.ToolStrip nodeFormMenu;
        private System.Windows.Forms.ToolStripDropDownButton FileMenu;
        private System.Windows.Forms.ToolStripMenuItem ts0;
        private System.Windows.Forms.ToolStripMenuItem ts1;
        private System.Windows.Forms.ToolStripMenuItem ts2;
        private System.Windows.Forms.ToolStripMenuItem ts3;
        private System.Windows.Forms.ToolStripButton helpBtn;
    }
}