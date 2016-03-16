namespace Swim_Algorithm
{
    partial class Swim_Algorithm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.KalmanR_textBox = new System.Windows.Forms.TextBox();
            this.KalmanR_label = new System.Windows.Forms.Label();
            this.KalmanQ_textBox = new System.Windows.Forms.TextBox();
            this.KalmanQ_label = new System.Windows.Forms.Label();
            this.KalmanP_textBox = new System.Windows.Forms.TextBox();
            this.KalmanP_label = new System.Windows.Forms.Label();
            this.KalmanH_textBox = new System.Windows.Forms.TextBox();
            this.KalmanH_label = new System.Windows.Forms.Label();
            this.KalmanA_textBox = new System.Windows.Forms.TextBox();
            this.Kalman_groupBox = new System.Windows.Forms.GroupBox();
            this.KalmanA_label = new System.Windows.Forms.Label();
            this.SVM_MA_checkBox = new System.Windows.Forms.CheckBox();
            this.SVM_checkBox = new System.Windows.Forms.CheckBox();
            this.MA_checkBox = new System.Windows.Forms.CheckBox();
            this.RawData_checkBox = new System.Windows.Forms.CheckBox();
            this.LeftHand_radioButton = new System.Windows.Forms.RadioButton();
            this.RightHand_radioButton = new System.Windows.Forms.RadioButton();
            this.OpenFileDialog_button = new System.Windows.Forms.Button();
            this.FilePath_groupBox = new System.Windows.Forms.GroupBox();
            this.FilePath_label = new System.Windows.Forms.Label();
            this.FilePath_textBox = new System.Windows.Forms.TextBox();
            this.Test_button = new System.Windows.Forms.Button();
            this.Kalman_groupBox.SuspendLayout();
            this.FilePath_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // KalmanR_textBox
            // 
            this.KalmanR_textBox.Location = new System.Drawing.Point(369, 15);
            this.KalmanR_textBox.Name = "KalmanR_textBox";
            this.KalmanR_textBox.Size = new System.Drawing.Size(49, 22);
            this.KalmanR_textBox.TabIndex = 9;
            this.KalmanR_textBox.Text = "5";
            // 
            // KalmanR_label
            // 
            this.KalmanR_label.AutoSize = true;
            this.KalmanR_label.Location = new System.Drawing.Point(344, 18);
            this.KalmanR_label.Name = "KalmanR_label";
            this.KalmanR_label.Size = new System.Drawing.Size(16, 12);
            this.KalmanR_label.TabIndex = 8;
            this.KalmanR_label.Text = "R:";
            // 
            // KalmanQ_textBox
            // 
            this.KalmanQ_textBox.Location = new System.Drawing.Point(289, 15);
            this.KalmanQ_textBox.Name = "KalmanQ_textBox";
            this.KalmanQ_textBox.Size = new System.Drawing.Size(49, 22);
            this.KalmanQ_textBox.TabIndex = 7;
            this.KalmanQ_textBox.Text = "0.1";
            // 
            // KalmanQ_label
            // 
            this.KalmanQ_label.AutoSize = true;
            this.KalmanQ_label.Location = new System.Drawing.Point(267, 18);
            this.KalmanQ_label.Name = "KalmanQ_label";
            this.KalmanQ_label.Size = new System.Drawing.Size(16, 12);
            this.KalmanQ_label.TabIndex = 6;
            this.KalmanQ_label.Text = "Q:";
            // 
            // KalmanP_textBox
            // 
            this.KalmanP_textBox.Location = new System.Drawing.Point(212, 15);
            this.KalmanP_textBox.Name = "KalmanP_textBox";
            this.KalmanP_textBox.Size = new System.Drawing.Size(49, 22);
            this.KalmanP_textBox.TabIndex = 5;
            this.KalmanP_textBox.Text = "0.1";
            // 
            // KalmanP_label
            // 
            this.KalmanP_label.AutoSize = true;
            this.KalmanP_label.Location = new System.Drawing.Point(192, 18);
            this.KalmanP_label.Name = "KalmanP_label";
            this.KalmanP_label.Size = new System.Drawing.Size(14, 12);
            this.KalmanP_label.TabIndex = 4;
            this.KalmanP_label.Text = "P:";
            // 
            // KalmanH_textBox
            // 
            this.KalmanH_textBox.Location = new System.Drawing.Point(137, 15);
            this.KalmanH_textBox.Name = "KalmanH_textBox";
            this.KalmanH_textBox.Size = new System.Drawing.Size(49, 22);
            this.KalmanH_textBox.TabIndex = 3;
            this.KalmanH_textBox.Text = "1";
            // 
            // KalmanH_label
            // 
            this.KalmanH_label.AutoSize = true;
            this.KalmanH_label.Location = new System.Drawing.Point(115, 18);
            this.KalmanH_label.Name = "KalmanH_label";
            this.KalmanH_label.Size = new System.Drawing.Size(16, 12);
            this.KalmanH_label.TabIndex = 2;
            this.KalmanH_label.Text = "H:";
            // 
            // KalmanA_textBox
            // 
            this.KalmanA_textBox.Location = new System.Drawing.Point(60, 15);
            this.KalmanA_textBox.Name = "KalmanA_textBox";
            this.KalmanA_textBox.Size = new System.Drawing.Size(49, 22);
            this.KalmanA_textBox.TabIndex = 1;
            this.KalmanA_textBox.Text = "1";
            // 
            // Kalman_groupBox
            // 
            this.Kalman_groupBox.Controls.Add(this.KalmanR_textBox);
            this.Kalman_groupBox.Controls.Add(this.KalmanR_label);
            this.Kalman_groupBox.Controls.Add(this.KalmanQ_textBox);
            this.Kalman_groupBox.Controls.Add(this.KalmanQ_label);
            this.Kalman_groupBox.Controls.Add(this.KalmanP_textBox);
            this.Kalman_groupBox.Controls.Add(this.KalmanP_label);
            this.Kalman_groupBox.Controls.Add(this.KalmanH_textBox);
            this.Kalman_groupBox.Controls.Add(this.KalmanH_label);
            this.Kalman_groupBox.Controls.Add(this.KalmanA_textBox);
            this.Kalman_groupBox.Controls.Add(this.KalmanA_label);
            this.Kalman_groupBox.Location = new System.Drawing.Point(12, 84);
            this.Kalman_groupBox.Name = "Kalman_groupBox";
            this.Kalman_groupBox.Size = new System.Drawing.Size(425, 45);
            this.Kalman_groupBox.TabIndex = 12;
            this.Kalman_groupBox.TabStop = false;
            this.Kalman_groupBox.Text = "Kalman Parameters";
            this.Kalman_groupBox.Visible = false;
            // 
            // KalmanA_label
            // 
            this.KalmanA_label.AutoSize = true;
            this.KalmanA_label.Location = new System.Drawing.Point(38, 18);
            this.KalmanA_label.Name = "KalmanA_label";
            this.KalmanA_label.Size = new System.Drawing.Size(16, 12);
            this.KalmanA_label.TabIndex = 0;
            this.KalmanA_label.Text = "A:";
            // 
            // SVM_MA_checkBox
            // 
            this.SVM_MA_checkBox.AutoSize = true;
            this.SVM_MA_checkBox.Location = new System.Drawing.Point(346, 43);
            this.SVM_MA_checkBox.Name = "SVM_MA_checkBox";
            this.SVM_MA_checkBox.Size = new System.Drawing.Size(72, 16);
            this.SVM_MA_checkBox.TabIndex = 9;
            this.SVM_MA_checkBox.Text = "SVM+MA";
            this.SVM_MA_checkBox.UseVisualStyleBackColor = true;
            // 
            // SVM_checkBox
            // 
            this.SVM_checkBox.AutoSize = true;
            this.SVM_checkBox.Location = new System.Drawing.Point(292, 43);
            this.SVM_checkBox.Name = "SVM_checkBox";
            this.SVM_checkBox.Size = new System.Drawing.Size(48, 16);
            this.SVM_checkBox.TabIndex = 8;
            this.SVM_checkBox.Text = "SVM";
            this.SVM_checkBox.UseVisualStyleBackColor = true;
            // 
            // MA_checkBox
            // 
            this.MA_checkBox.AutoSize = true;
            this.MA_checkBox.Location = new System.Drawing.Point(186, 43);
            this.MA_checkBox.Name = "MA_checkBox";
            this.MA_checkBox.Size = new System.Drawing.Size(100, 16);
            this.MA_checkBox.TabIndex = 7;
            this.MA_checkBox.Text = "MovingAverage";
            this.MA_checkBox.UseVisualStyleBackColor = true;
            // 
            // RawData_checkBox
            // 
            this.RawData_checkBox.AutoSize = true;
            this.RawData_checkBox.Location = new System.Drawing.Point(114, 43);
            this.RawData_checkBox.Name = "RawData_checkBox";
            this.RawData_checkBox.Size = new System.Drawing.Size(66, 16);
            this.RawData_checkBox.TabIndex = 6;
            this.RawData_checkBox.Text = "RawData";
            this.RawData_checkBox.UseVisualStyleBackColor = true;
            // 
            // LeftHand_radioButton
            // 
            this.LeftHand_radioButton.AutoSize = true;
            this.LeftHand_radioButton.Checked = true;
            this.LeftHand_radioButton.Location = new System.Drawing.Point(8, 42);
            this.LeftHand_radioButton.Name = "LeftHand_radioButton";
            this.LeftHand_radioButton.Size = new System.Drawing.Size(47, 16);
            this.LeftHand_radioButton.TabIndex = 5;
            this.LeftHand_radioButton.TabStop = true;
            this.LeftHand_radioButton.Text = "左手";
            this.LeftHand_radioButton.UseVisualStyleBackColor = true;
            // 
            // RightHand_radioButton
            // 
            this.RightHand_radioButton.AutoSize = true;
            this.RightHand_radioButton.Location = new System.Drawing.Point(61, 42);
            this.RightHand_radioButton.Name = "RightHand_radioButton";
            this.RightHand_radioButton.Size = new System.Drawing.Size(47, 16);
            this.RightHand_radioButton.TabIndex = 4;
            this.RightHand_radioButton.TabStop = true;
            this.RightHand_radioButton.Text = "右手";
            this.RightHand_radioButton.UseVisualStyleBackColor = true;
            // 
            // OpenFileDialog_button
            // 
            this.OpenFileDialog_button.Location = new System.Drawing.Point(346, 13);
            this.OpenFileDialog_button.Name = "OpenFileDialog_button";
            this.OpenFileDialog_button.Size = new System.Drawing.Size(69, 23);
            this.OpenFileDialog_button.TabIndex = 3;
            this.OpenFileDialog_button.Text = "Open File";
            this.OpenFileDialog_button.UseVisualStyleBackColor = true;
            this.OpenFileDialog_button.Click += new System.EventHandler(this.OpenFileDialog_button_Click);
            // 
            // FilePath_groupBox
            // 
            this.FilePath_groupBox.Controls.Add(this.SVM_MA_checkBox);
            this.FilePath_groupBox.Controls.Add(this.SVM_checkBox);
            this.FilePath_groupBox.Controls.Add(this.MA_checkBox);
            this.FilePath_groupBox.Controls.Add(this.RawData_checkBox);
            this.FilePath_groupBox.Controls.Add(this.LeftHand_radioButton);
            this.FilePath_groupBox.Controls.Add(this.RightHand_radioButton);
            this.FilePath_groupBox.Controls.Add(this.OpenFileDialog_button);
            this.FilePath_groupBox.Controls.Add(this.FilePath_label);
            this.FilePath_groupBox.Controls.Add(this.FilePath_textBox);
            this.FilePath_groupBox.Location = new System.Drawing.Point(12, 12);
            this.FilePath_groupBox.Name = "FilePath_groupBox";
            this.FilePath_groupBox.Size = new System.Drawing.Size(425, 66);
            this.FilePath_groupBox.TabIndex = 11;
            this.FilePath_groupBox.TabStop = false;
            this.FilePath_groupBox.Text = "File Path";
            // 
            // FilePath_label
            // 
            this.FilePath_label.AutoSize = true;
            this.FilePath_label.Location = new System.Drawing.Point(6, 18);
            this.FilePath_label.Name = "FilePath_label";
            this.FilePath_label.Size = new System.Drawing.Size(32, 12);
            this.FilePath_label.TabIndex = 2;
            this.FilePath_label.Text = "路徑:";
            // 
            // FilePath_textBox
            // 
            this.FilePath_textBox.Location = new System.Drawing.Point(45, 15);
            this.FilePath_textBox.Name = "FilePath_textBox";
            this.FilePath_textBox.Size = new System.Drawing.Size(293, 22);
            this.FilePath_textBox.TabIndex = 1;
            // 
            // Test_button
            // 
            this.Test_button.Location = new System.Drawing.Point(352, 87);
            this.Test_button.Name = "Test_button";
            this.Test_button.Size = new System.Drawing.Size(75, 23);
            this.Test_button.TabIndex = 13;
            this.Test_button.Text = "Test";
            this.Test_button.UseVisualStyleBackColor = true;
            this.Test_button.Click += new System.EventHandler(this.Test_button_Click);
            // 
            // Swim_Algorithm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 124);
            this.Controls.Add(this.Test_button);
            this.Controls.Add(this.Kalman_groupBox);
            this.Controls.Add(this.FilePath_groupBox);
            this.Name = "Swim_Algorithm";
            this.Text = "Swim_Algorithm";
            this.Kalman_groupBox.ResumeLayout(false);
            this.Kalman_groupBox.PerformLayout();
            this.FilePath_groupBox.ResumeLayout(false);
            this.FilePath_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox KalmanR_textBox;
        private System.Windows.Forms.Label KalmanR_label;
        private System.Windows.Forms.TextBox KalmanQ_textBox;
        private System.Windows.Forms.Label KalmanQ_label;
        private System.Windows.Forms.TextBox KalmanP_textBox;
        private System.Windows.Forms.Label KalmanP_label;
        private System.Windows.Forms.TextBox KalmanH_textBox;
        private System.Windows.Forms.Label KalmanH_label;
        private System.Windows.Forms.TextBox KalmanA_textBox;
        private System.Windows.Forms.GroupBox Kalman_groupBox;
        private System.Windows.Forms.Label KalmanA_label;
        private System.Windows.Forms.CheckBox SVM_MA_checkBox;
        private System.Windows.Forms.CheckBox SVM_checkBox;
        private System.Windows.Forms.CheckBox MA_checkBox;
        private System.Windows.Forms.CheckBox RawData_checkBox;
        private System.Windows.Forms.RadioButton LeftHand_radioButton;
        private System.Windows.Forms.RadioButton RightHand_radioButton;
        private System.Windows.Forms.Button OpenFileDialog_button;
        private System.Windows.Forms.GroupBox FilePath_groupBox;
        private System.Windows.Forms.Label FilePath_label;
        private System.Windows.Forms.TextBox FilePath_textBox;
        private System.Windows.Forms.Button Test_button;
    }
}

