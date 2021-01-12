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
            this.propertiesTable = new System.Windows.Forms.TableLayoutPanel();
            this.nodeGraph1 = new Hetwork.NodeGraph();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.primaryTable.SuspendLayout();
            this.propertiesTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // primaryTable
            // 
            this.primaryTable.ColumnCount = 2;
            this.primaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.625F));
            this.primaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.375F));
            this.primaryTable.Controls.Add(this.nodeGraph1, 0, 0);
            this.primaryTable.Controls.Add(this.propertiesTable, 1, 0);
            this.primaryTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primaryTable.Location = new System.Drawing.Point(0, 0);
            this.primaryTable.Name = "primaryTable";
            this.primaryTable.RowCount = 1;
            this.primaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.primaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.primaryTable.Size = new System.Drawing.Size(800, 450);
            this.primaryTable.TabIndex = 1;
            // 
            // propertiesTable
            // 
            this.propertiesTable.ColumnCount = 1;
            this.propertiesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.propertiesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.propertiesTable.Controls.Add(this.label1, 0, 0);
            this.propertiesTable.Controls.Add(this.richTextBox1, 0, 1);
            this.propertiesTable.Controls.Add(this.listView1, 0, 2);
            this.propertiesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesTable.Location = new System.Drawing.Point(680, 3);
            this.propertiesTable.Name = "propertiesTable";
            this.propertiesTable.RowCount = 3;
            this.propertiesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.828829F));
            this.propertiesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.72973F));
            this.propertiesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
            this.propertiesTable.Size = new System.Drawing.Size(117, 444);
            this.propertiesTable.TabIndex = 1;
            // 
            // nodeGraph1
            // 
            this.nodeGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeGraph1.BackColor = System.Drawing.Color.White;
            this.nodeGraph1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeGraph1.ForeColor = System.Drawing.Color.Black;
            this.nodeGraph1.Location = new System.Drawing.Point(3, 3);
            this.nodeGraph1.Name = "nodeGraph1";
            this.nodeGraph1.Size = new System.Drawing.Size(671, 444);
            this.nodeGraph1.TabIndex = 0;
            this.nodeGraph1.Load += new System.EventHandler(this.nodeGraph1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(111, 236);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 261);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(111, 180);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
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
            this.propertiesTable.ResumeLayout(false);
            this.propertiesTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NodeGraph nodeGraph1;
        private System.Windows.Forms.TableLayoutPanel primaryTable;
        private System.Windows.Forms.TableLayoutPanel propertiesTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListView listView1;
    }
}