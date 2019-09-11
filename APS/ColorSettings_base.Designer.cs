namespace APS
{
    partial class ColorSettings_base
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSettings_base));
            this.lblEdition = new System.Windows.Forms.Label();
            this.cmbIssue = new System.Windows.Forms.ComboBox();
            this.lblIssue = new System.Windows.Forms.Label();
            this.cmbEdition = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.colorsIcon = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPages = new System.Windows.Forms.Label();
            this.cmbPages = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txbName = new System.Windows.Forms.TextBox();
            this.lblScheme = new System.Windows.Forms.Label();
            this.cmbScheme = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoText = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEdition
            // 
            this.lblEdition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblEdition.AutoSize = true;
            this.lblEdition.Location = new System.Drawing.Point(178, 51);
            this.lblEdition.Name = "lblEdition";
            this.lblEdition.Size = new System.Drawing.Size(45, 13);
            this.lblEdition.TabIndex = 3;
            this.lblEdition.Text = "Выпуск";
            // 
            // cmbIssue
            // 
            this.cmbIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIssue.FormattingEnabled = true;
            this.cmbIssue.Location = new System.Drawing.Point(229, 48);
            this.cmbIssue.Name = "cmbIssue";
            this.cmbIssue.Size = new System.Drawing.Size(107, 21);
            this.cmbIssue.TabIndex = 4;
            // 
            // lblIssue
            // 
            this.lblIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIssue.AutoSize = true;
            this.lblIssue.Location = new System.Drawing.Point(12, 51);
            this.lblIssue.Name = "lblIssue";
            this.lblIssue.Size = new System.Drawing.Size(51, 13);
            this.lblIssue.TabIndex = 3;
            this.lblIssue.Text = "Издание";
            // 
            // cmbEdition
            // 
            this.cmbEdition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbEdition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEdition.FormattingEnabled = true;
            this.cmbEdition.Location = new System.Drawing.Point(69, 48);
            this.cmbEdition.Name = "cmbEdition";
            this.cmbEdition.Size = new System.Drawing.Size(103, 21);
            this.cmbEdition.TabIndex = 5;
            this.cmbEdition.SelectedIndexChanged += new System.EventHandler(this.cmbEdition_SelectedIndexChanged);
            this.cmbEdition.DropDownClosed += new System.EventHandler(this.cmbEdition_DropDownClosed);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::APS.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(349, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 45);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Сохранить";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Image = global::APS.Properties.Resources.Back_2;
            this.btnClose.Location = new System.Drawing.Point(349, 91);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 44);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Вернуться";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // colorsIcon
            // 
            this.colorsIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("colorsIcon.ImageStream")));
            this.colorsIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.colorsIcon.Images.SetKeyName(0, "bw");
            this.colorsIcon.Images.SetKeyName(1, "color");
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(15, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 149);
            this.panel1.TabIndex = 10;
            // 
            // lblPages
            // 
            this.lblPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPages.AutoSize = true;
            this.lblPages.Location = new System.Drawing.Point(12, 85);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(105, 13);
            this.lblPages.TabIndex = 11;
            this.lblPages.Text = "Количестово полос";
            // 
            // cmbPages
            // 
            this.cmbPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPages.FormattingEnabled = true;
            this.cmbPages.Location = new System.Drawing.Point(123, 82);
            this.cmbPages.Name = "cmbPages";
            this.cmbPages.Size = new System.Drawing.Size(49, 21);
            this.cmbPages.TabIndex = 12;
            this.cmbPages.SelectedIndexChanged += new System.EventHandler(this.pCount_SelectedIndexChanged);
            this.cmbPages.DropDownClosed += new System.EventHandler(this.cmbPages_DropDownClosed);
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 117);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(57, 13);
            this.lblName.TabIndex = 13;
            this.lblName.Text = "Название";
            // 
            // txbName
            // 
            this.txbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txbName.Location = new System.Drawing.Point(75, 114);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(261, 20);
            this.txbName.TabIndex = 14;
            // 
            // lblScheme
            // 
            this.lblScheme.AutoSize = true;
            this.lblScheme.Location = new System.Drawing.Point(12, 15);
            this.lblScheme.Name = "lblScheme";
            this.lblScheme.Size = new System.Drawing.Size(86, 13);
            this.lblScheme.TabIndex = 15;
            this.lblScheme.Text = "Готовые схемы";
            this.lblScheme.Visible = false;
            // 
            // cmbScheme
            // 
            this.cmbScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScheme.FormattingEnabled = true;
            this.cmbScheme.Location = new System.Drawing.Point(104, 12);
            this.cmbScheme.Name = "cmbScheme";
            this.cmbScheme.Size = new System.Drawing.Size(232, 21);
            this.cmbScheme.TabIndex = 16;
            this.cmbScheme.Visible = false;
            this.cmbScheme.SelectedIndexChanged += new System.EventHandler(this.cmbScheme_SelectedIndexChanged);
            this.cmbScheme.DropDownClosed += new System.EventHandler(this.cmbScheme_DropDownClosed);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 305);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(481, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoText
            // 
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(49, 17);
            this.infoText.Text = "infoText";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ColorSettings_base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 327);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmbScheme);
            this.Controls.Add(this.lblScheme);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cmbPages);
            this.Controls.Add(this.lblPages);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmbEdition);
            this.Controls.Add(this.cmbIssue);
            this.Controls.Add(this.lblIssue);
            this.Controls.Add(this.lblEdition);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorSettings_base";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ColorSettings";
            this.Load += new System.EventHandler(this.ColorSettings_Load);
            this.SizeChanged += new System.EventHandler(this.ColorSettings_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList colorsIcon;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Label lblEdition;
        internal System.Windows.Forms.ComboBox cmbIssue;
        internal System.Windows.Forms.Label lblIssue;
        internal System.Windows.Forms.ComboBox cmbEdition;
        internal System.Windows.Forms.Label lblPages;
        internal System.Windows.Forms.ComboBox cmbPages;
        internal System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.TextBox txbName;
        internal System.Windows.Forms.Label lblScheme;
        internal System.Windows.Forms.ComboBox cmbScheme;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoText;
        private System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.Panel panel1;
    }
}