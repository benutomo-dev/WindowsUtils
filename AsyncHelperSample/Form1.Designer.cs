﻿namespace AsyncHelperSample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.normalDoDelayActionButton = new System.Windows.Forms.Button();
            this.modalDoDelayActionButton = new System.Windows.Forms.Button();
            this.statusLabel1 = new System.Windows.Forms.Label();
            this.showDialogButton = new System.Windows.Forms.Button();
            this.showFormButton = new System.Windows.Forms.Button();
            this.showChildFormButton = new System.Windows.Forms.Button();
            this.showModalTaskDialogButton = new System.Windows.Forms.Button();
            this.showModelessTaskDialogButton = new System.Windows.Forms.Button();
            this.exclusiveDelayActionButton = new System.Windows.Forms.Button();
            this.statusLabel2 = new System.Windows.Forms.Label();
            this.exclusiveDelayAction2Button = new System.Windows.Forms.Button();
            this.exclusiveDelayAction1Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // normalDoDelayActionButton
            // 
            this.normalDoDelayActionButton.Location = new System.Drawing.Point(5, 3);
            this.normalDoDelayActionButton.Name = "normalDoDelayActionButton";
            this.normalDoDelayActionButton.Size = new System.Drawing.Size(180, 23);
            this.normalDoDelayActionButton.TabIndex = 0;
            this.normalDoDelayActionButton.Text = "NormalDoDelayAction";
            this.normalDoDelayActionButton.UseVisualStyleBackColor = true;
            this.normalDoDelayActionButton.Click += new System.EventHandler(this.normalDoDelayActionButton_Click);
            // 
            // modalDoDelayActionButton
            // 
            this.modalDoDelayActionButton.Location = new System.Drawing.Point(5, 32);
            this.modalDoDelayActionButton.Name = "modalDoDelayActionButton";
            this.modalDoDelayActionButton.Size = new System.Drawing.Size(180, 23);
            this.modalDoDelayActionButton.TabIndex = 1;
            this.modalDoDelayActionButton.Text = "ModalDoDelayAction";
            this.modalDoDelayActionButton.UseVisualStyleBackColor = true;
            this.modalDoDelayActionButton.Click += new System.EventHandler(this.modalDoDelayActionButton_Click);
            // 
            // statusLabel1
            // 
            this.statusLabel1.AutoSize = true;
            this.statusLabel1.Location = new System.Drawing.Point(211, 14);
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(35, 12);
            this.statusLabel1.TabIndex = 2;
            this.statusLabel1.Text = "label1";
            // 
            // showDialogButton
            // 
            this.showDialogButton.Location = new System.Drawing.Point(5, 158);
            this.showDialogButton.Name = "showDialogButton";
            this.showDialogButton.Size = new System.Drawing.Size(179, 23);
            this.showDialogButton.TabIndex = 3;
            this.showDialogButton.Text = "ShowDialog";
            this.showDialogButton.UseVisualStyleBackColor = true;
            this.showDialogButton.Click += new System.EventHandler(this.showDialogButton_Click);
            // 
            // showFormButton
            // 
            this.showFormButton.Location = new System.Drawing.Point(5, 188);
            this.showFormButton.Name = "showFormButton";
            this.showFormButton.Size = new System.Drawing.Size(179, 23);
            this.showFormButton.TabIndex = 4;
            this.showFormButton.Text = "ShowForm";
            this.showFormButton.UseVisualStyleBackColor = true;
            this.showFormButton.Click += new System.EventHandler(this.showFormButton_Click);
            // 
            // showChildFormButton
            // 
            this.showChildFormButton.Location = new System.Drawing.Point(5, 217);
            this.showChildFormButton.Name = "showChildFormButton";
            this.showChildFormButton.Size = new System.Drawing.Size(179, 23);
            this.showChildFormButton.TabIndex = 5;
            this.showChildFormButton.Text = "ShowChildForm";
            this.showChildFormButton.UseVisualStyleBackColor = true;
            this.showChildFormButton.Click += new System.EventHandler(this.showChildFormButton_Click);
            // 
            // showModalTaskDialogButton
            // 
            this.showModalTaskDialogButton.Location = new System.Drawing.Point(5, 247);
            this.showModalTaskDialogButton.Name = "showModalTaskDialogButton";
            this.showModalTaskDialogButton.Size = new System.Drawing.Size(179, 23);
            this.showModalTaskDialogButton.TabIndex = 6;
            this.showModalTaskDialogButton.Text = "ShowModalTaskDialog";
            this.showModalTaskDialogButton.UseVisualStyleBackColor = true;
            this.showModalTaskDialogButton.Click += new System.EventHandler(this.showModalTaskDialogButton_Click);
            // 
            // showModelessTaskDialogButton
            // 
            this.showModelessTaskDialogButton.Location = new System.Drawing.Point(5, 276);
            this.showModelessTaskDialogButton.Name = "showModelessTaskDialogButton";
            this.showModelessTaskDialogButton.Size = new System.Drawing.Size(179, 23);
            this.showModelessTaskDialogButton.TabIndex = 7;
            this.showModelessTaskDialogButton.Text = "ShowModelesslTaskDialog";
            this.showModelessTaskDialogButton.UseVisualStyleBackColor = true;
            this.showModelessTaskDialogButton.Click += new System.EventHandler(this.showModelessTaskDialogButton_Click);
            // 
            // exclusiveDelayActionButton
            // 
            this.exclusiveDelayActionButton.Location = new System.Drawing.Point(5, 61);
            this.exclusiveDelayActionButton.Name = "exclusiveDelayActionButton";
            this.exclusiveDelayActionButton.Size = new System.Drawing.Size(180, 23);
            this.exclusiveDelayActionButton.TabIndex = 9;
            this.exclusiveDelayActionButton.Text = "ExclusiveDelayAction";
            this.exclusiveDelayActionButton.UseVisualStyleBackColor = true;
            this.exclusiveDelayActionButton.Click += new System.EventHandler(this.exclusiveDelayActionButton_Click);
            // 
            // statusLabel2
            // 
            this.statusLabel2.AutoSize = true;
            this.statusLabel2.Location = new System.Drawing.Point(211, 43);
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(35, 12);
            this.statusLabel2.TabIndex = 10;
            this.statusLabel2.Text = "label1";
            // 
            // exclusiveDelayAction2Button
            // 
            this.exclusiveDelayAction2Button.Location = new System.Drawing.Point(5, 119);
            this.exclusiveDelayAction2Button.Name = "exclusiveDelayAction2Button";
            this.exclusiveDelayAction2Button.Size = new System.Drawing.Size(180, 23);
            this.exclusiveDelayAction2Button.TabIndex = 11;
            this.exclusiveDelayAction2Button.Text = "ExclusiveDelayAction2";
            this.exclusiveDelayAction2Button.UseVisualStyleBackColor = true;
            this.exclusiveDelayAction2Button.Click += new System.EventHandler(this.exclusiveDelayAction2Button_Click);
            // 
            // exclusiveDelayAction1Button
            // 
            this.exclusiveDelayAction1Button.Location = new System.Drawing.Point(5, 90);
            this.exclusiveDelayAction1Button.Name = "exclusiveDelayAction1Button";
            this.exclusiveDelayAction1Button.Size = new System.Drawing.Size(180, 23);
            this.exclusiveDelayAction1Button.TabIndex = 12;
            this.exclusiveDelayAction1Button.Text = "ExclusiveDelayAction1";
            this.exclusiveDelayAction1Button.UseVisualStyleBackColor = true;
            this.exclusiveDelayAction1Button.Click += new System.EventHandler(this.exclusiveDelayAction1Button_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.normalDoDelayActionButton);
            this.panel1.Controls.Add(this.statusLabel2);
            this.panel1.Controls.Add(this.exclusiveDelayAction1Button);
            this.panel1.Controls.Add(this.statusLabel1);
            this.panel1.Controls.Add(this.modalDoDelayActionButton);
            this.panel1.Controls.Add(this.exclusiveDelayAction2Button);
            this.panel1.Controls.Add(this.showDialogButton);
            this.panel1.Controls.Add(this.showFormButton);
            this.panel1.Controls.Add(this.exclusiveDelayActionButton);
            this.panel1.Controls.Add(this.showChildFormButton);
            this.panel1.Controls.Add(this.showModelessTaskDialogButton);
            this.panel1.Controls.Add(this.showModalTaskDialogButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 348);
            this.panel1.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 348);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button normalDoDelayActionButton;
        private System.Windows.Forms.Button modalDoDelayActionButton;
        private System.Windows.Forms.Label statusLabel1;
        private System.Windows.Forms.Button showDialogButton;
        private System.Windows.Forms.Button showFormButton;
        private System.Windows.Forms.Button showChildFormButton;
        private System.Windows.Forms.Button showModalTaskDialogButton;
        private System.Windows.Forms.Button showModelessTaskDialogButton;
        private System.Windows.Forms.Button exclusiveDelayActionButton;
        private System.Windows.Forms.Label statusLabel2;
        private System.Windows.Forms.Button exclusiveDelayAction2Button;
        private System.Windows.Forms.Button exclusiveDelayAction1Button;
        private System.Windows.Forms.Panel panel1;
    }
}

