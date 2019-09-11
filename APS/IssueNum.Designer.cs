namespace APS
{
    partial class IssueNum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueNum));
            this.sNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.bNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNumUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sNumUpDown
            // 
            this.sNumUpDown.Location = new System.Drawing.Point(97, 52);
            this.sNumUpDown.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.sNumUpDown.Name = "sNumUpDown";
            this.sNumUpDown.Size = new System.Drawing.Size(62, 20);
            this.sNumUpDown.TabIndex = 4;
            this.sNumUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sNumUpDown.ValueChanged += new System.EventHandler(this.sNumUpDown_ValueChanged);
            // 
            // bNumUpDown
            // 
            this.bNumUpDown.Enabled = false;
            this.bNumUpDown.Location = new System.Drawing.Point(97, 90);
            this.bNumUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.bNumUpDown.Name = "bNumUpDown";
            this.bNumUpDown.Size = new System.Drawing.Size(62, 20);
            this.bNumUpDown.TabIndex = 5;
            this.bNumUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Укажите номер выпуска";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Image = global::APS.Properties.Resources.Stop_2;
            this.btnCancel.Location = new System.Drawing.Point(124, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 45);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отменить";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Image = global::APS.Properties.Resources.Floppy_Disk_Blue;
            this.btnSave.Location = new System.Drawing.Point(12, 130);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 45);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // IssueNum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 187);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bNumUpDown);
            this.Controls.Add(this.sNumUpDown);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IssueNum";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Недостаточно данных";
            this.Load += new System.EventHandler(this.IssueNum_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNumUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown sNumUpDown;
        private System.Windows.Forms.NumericUpDown bNumUpDown;
        private System.Windows.Forms.Label label3;
    }
}