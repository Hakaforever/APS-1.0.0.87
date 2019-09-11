namespace APS
{
    partial class MultiTaskForms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiTaskForms));
            this.btnIcon = new System.Windows.Forms.Button();
            this.lblCombo = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbSelect = new System.Windows.Forms.ComboBox();
            this.chkSpread = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.lblColor = new System.Windows.Forms.Label();
            this.chkAtex = new System.Windows.Forms.CheckBox();
            this.cmbEdition = new System.Windows.Forms.ComboBox();
            this.lblEdition = new System.Windows.Forms.Label();
            this.setIcon = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnIcon
            // 
            this.btnIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIcon.Location = new System.Drawing.Point(339, 52);
            this.btnIcon.Name = "btnIcon";
            this.btnIcon.Size = new System.Drawing.Size(108, 57);
            this.btnIcon.TabIndex = 5;
            this.btnIcon.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnIcon.UseVisualStyleBackColor = true;
            this.btnIcon.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // lblCombo
            // 
            this.lblCombo.AutoSize = true;
            this.lblCombo.Location = new System.Drawing.Point(12, 31);
            this.lblCombo.Name = "lblCombo";
            this.lblCombo.Size = new System.Drawing.Size(94, 13);
            this.lblCombo.TabIndex = 3;
            this.lblCombo.Text = "Выбор из списка";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(12, 71);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(88, 13);
            this.lblCode.TabIndex = 4;
            this.lblCode.Text = "Код (3 символа)";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 111);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(57, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Название";
            // 
            // txtCode
            // 
            this.txtCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCode.Location = new System.Drawing.Point(112, 68);
            this.txtCode.MaxLength = 3;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(51, 20);
            this.txtCode.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(112, 108);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(209, 20);
            this.txtName.TabIndex = 3;
            // 
            // cmbSelect
            // 
            this.cmbSelect.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelect.FormattingEnabled = true;
            this.cmbSelect.Location = new System.Drawing.Point(112, 28);
            this.cmbSelect.Name = "cmbSelect";
            this.cmbSelect.Size = new System.Drawing.Size(209, 21);
            this.cmbSelect.TabIndex = 1;
            // 
            // chkSpread
            // 
            this.chkSpread.AutoSize = true;
            this.chkSpread.Location = new System.Drawing.Point(354, 123);
            this.chkSpread.Name = "chkSpread";
            this.chkSpread.Size = new System.Drawing.Size(74, 17);
            this.chkSpread.TabIndex = 9;
            this.chkSpread.Text = "Разворот";
            this.chkSpread.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSpread.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 182);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(593, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stripStatus
            // 
            this.stripStatus.Image = global::APS.Properties.Resources.Spell;
            this.stripStatus.Name = "stripStatus";
            this.stripStatus.Size = new System.Drawing.Size(78, 17);
            this.stripStatus.Text = "stripStatus";
            this.stripStatus.Visible = false;
            this.stripStatus.TextChanged += new System.EventHandler(this.stripStatus_TextChanged);
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(12, 151);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(77, 13);
            this.lblComment.TabIndex = 5;
            this.lblComment.Text = "Комментарий";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(112, 148);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(209, 20);
            this.txtComment.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::APS.Properties.Resources.Back_2;
            this.btnCancel.Location = new System.Drawing.Point(465, 114);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 54);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Завершить";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Image = global::APS.Properties.Resources.Save;
            this.btnOK.Location = new System.Drawing.Point(465, 28);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(115, 54);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "ОК";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.FullOpen = true;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(339, 30);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(92, 13);
            this.lblColor.TabIndex = 11;
            this.lblColor.Text = "Выбрать иконку:";
            // 
            // chkAtex
            // 
            this.chkAtex.AutoSize = true;
            this.chkAtex.Location = new System.Drawing.Point(354, 150);
            this.chkAtex.Name = "chkAtex";
            this.chkAtex.Size = new System.Drawing.Size(64, 17);
            this.chkAtex.TabIndex = 12;
            this.chkAtex.Text = "Из Atex";
            this.chkAtex.UseVisualStyleBackColor = true;
            // 
            // cmbEdition
            // 
            this.cmbEdition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEdition.FormattingEnabled = true;
            this.cmbEdition.Location = new System.Drawing.Point(245, 68);
            this.cmbEdition.Name = "cmbEdition";
            this.cmbEdition.Size = new System.Drawing.Size(76, 21);
            this.cmbEdition.TabIndex = 13;
            this.cmbEdition.DropDownClosed += new System.EventHandler(this.cmbEdition_DropDownClosed);
            // 
            // lblEdition
            // 
            this.lblEdition.AutoSize = true;
            this.lblEdition.Location = new System.Drawing.Point(179, 71);
            this.lblEdition.Name = "lblEdition";
            this.lblEdition.Size = new System.Drawing.Size(51, 13);
            this.lblEdition.TabIndex = 14;
            this.lblEdition.Text = "Издание";
            // 
            // setIcon
            // 
            this.setIcon.Filter = "*.png|*.*";
            // 
            // MultiTaskForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(593, 204);
            this.Controls.Add(this.lblEdition);
            this.Controls.Add(this.cmbEdition);
            this.Controls.Add(this.chkAtex);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.chkSpread);
            this.Controls.Add(this.cmbSelect);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.lblCombo);
            this.Controls.Add(this.btnIcon);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiTaskForms";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MultiTaskForms";
            this.Load += new System.EventHandler(this.MultiTaskForms_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnIcon;
        private System.Windows.Forms.Label lblCombo;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbSelect;
        private System.Windows.Forms.CheckBox chkSpread;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stripStatus;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.CheckBox chkAtex;
        private System.Windows.Forms.ComboBox cmbEdition;
        private System.Windows.Forms.Label lblEdition;
        private System.Windows.Forms.OpenFileDialog setIcon;
    }
}