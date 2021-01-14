namespace Hetwork
{
    partial class EditorForm
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
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.titleTextbox = new System.Windows.Forms.TextBox();
            this.buttonTable = new System.Windows.Forms.TableLayoutPanel();
            this.applyBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.contentBox = new System.Windows.Forms.RichTextBox();
            this.table.SuspendLayout();
            this.buttonTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.ColumnCount = 1;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Controls.Add(this.titleTextbox, 0, 0);
            this.table.Controls.Add(this.buttonTable, 0, 2);
            this.table.Controls.Add(this.contentBox, 0, 1);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 3;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.995975F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.2955F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.708779F));
            this.table.Size = new System.Drawing.Size(383, 467);
            this.table.TabIndex = 0;
            // 
            // titleTextbox
            // 
            this.titleTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleTextbox.Location = new System.Drawing.Point(3, 3);
            this.titleTextbox.Name = "titleTextbox";
            this.titleTextbox.Size = new System.Drawing.Size(377, 13);
            this.titleTextbox.TabIndex = 0;
            // 
            // buttonTable
            // 
            this.buttonTable.ColumnCount = 2;
            this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonTable.Controls.Add(this.applyBtn, 0, 0);
            this.buttonTable.Controls.Add(this.cancelBtn, 1, 0);
            this.buttonTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTable.Location = new System.Drawing.Point(3, 433);
            this.buttonTable.Name = "buttonTable";
            this.buttonTable.RowCount = 1;
            this.buttonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.buttonTable.Size = new System.Drawing.Size(377, 31);
            this.buttonTable.TabIndex = 1;
            // 
            // applyBtn
            // 
            this.applyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.applyBtn.Location = new System.Drawing.Point(3, 3);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(75, 23);
            this.applyBtn.TabIndex = 0;
            this.applyBtn.Text = "Apply";
            this.applyBtn.UseVisualStyleBackColor = true;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Location = new System.Drawing.Point(191, 3);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // contentBox
            // 
            this.contentBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contentBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentBox.Location = new System.Drawing.Point(3, 31);
            this.contentBox.Name = "contentBox";
            this.contentBox.Size = new System.Drawing.Size(377, 396);
            this.contentBox.TabIndex = 2;
            this.contentBox.Text = "";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 467);
            this.Controls.Add(this.table);
            this.Name = "EditorForm";
            this.Text = "Editor";
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            this.buttonTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.TextBox titleTextbox;
        private System.Windows.Forms.TableLayoutPanel buttonTable;
        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.RichTextBox contentBox;
    }
}