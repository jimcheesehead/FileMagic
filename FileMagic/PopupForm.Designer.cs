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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // PopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 155);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}