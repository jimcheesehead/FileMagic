namespace FileMagic
{
    partial class PopupForm
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
            this.btnPopupOK = new System.Windows.Forms.Button();
            this.btnPopupSkip = new System.Windows.Forms.Button();
            this.btnPopupCancel = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNewTargetDir = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnPopupOK
            // 
            this.btnPopupOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPopupOK.Location = new System.Drawing.Point(12, 107);
            this.btnPopupOK.Name = "btnPopupOK";
            this.btnPopupOK.Size = new System.Drawing.Size(75, 23);
            this.btnPopupOK.TabIndex = 0;
            this.btnPopupOK.Text = "OK";
            this.btnPopupOK.UseVisualStyleBackColor = true;
            this.btnPopupOK.Click += new System.EventHandler(this.btnPopupOK_Click);
            // 
            // btnPopupSkip
            // 
            this.btnPopupSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnPopupSkip.Location = new System.Drawing.Point(107, 107);
            this.btnPopupSkip.Name = "btnPopupSkip";
            this.btnPopupSkip.Size = new System.Drawing.Size(75, 23);
            this.btnPopupSkip.TabIndex = 1;
            this.btnPopupSkip.Text = "Skip";
            this.btnPopupSkip.UseVisualStyleBackColor = true;
            this.btnPopupSkip.Click += new System.EventHandler(this.btnPopupSkip_Click);
            // 
            // btnPopupCancel
            // 
            this.btnPopupCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPopupCancel.Location = new System.Drawing.Point(200, 107);
            this.btnPopupCancel.Name = "btnPopupCancel";
            this.btnPopupCancel.Size = new System.Drawing.Size(75, 23);
            this.btnPopupCancel.TabIndex = 2;
            this.btnPopupCancel.Text = "Cancel";
            this.btnPopupCancel.UseVisualStyleBackColor = true;
            this.btnPopupCancel.Click += new System.EventHandler(this.btnPopupCancel_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(12, 9);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(35, 13);
            this.lblFile.TabIndex = 3;
            this.lblFile.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Current Target:";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(104, 38);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(35, 13);
            this.lblTarget.TabIndex = 5;
            this.lblTarget.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "New Target";
            // 
            // txtNewTargetDir
            // 
            this.txtNewTargetDir.Location = new System.Drawing.Point(107, 60);
            this.txtNewTargetDir.Name = "txtNewTargetDir";
            this.txtNewTargetDir.Size = new System.Drawing.Size(738, 20);
            this.txtNewTargetDir.TabIndex = 7;
            this.txtNewTargetDir.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // PopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 155);
            this.Controls.Add(this.txtNewTargetDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.btnPopupCancel);
            this.Controls.Add(this.btnPopupSkip);
            this.Controls.Add(this.btnPopupOK);
            this.Name = "PopupForm";
            this.Text = "Create BAD shortcuts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPopupOK;
        private System.Windows.Forms.Button btnPopupSkip;
        private System.Windows.Forms.Button btnPopupCancel;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNewTargetDir;
    }
}