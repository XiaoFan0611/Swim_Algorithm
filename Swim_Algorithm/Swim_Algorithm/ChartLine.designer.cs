namespace ChartLine
{
    partial class ChartLine
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.GSensor_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Z_checkBox = new System.Windows.Forms.CheckBox();
            this.Y_checkBox = new System.Windows.Forms.CheckBox();
            this.X_checkBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.GSensor_chart)).BeginInit();
            this.SuspendLayout();
            // 
            // GSensor_chart
            // 
            this.GSensor_chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GSensor_chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            chartArea1.Name = "ChartArea1";
            this.GSensor_chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.GSensor_chart.Legends.Add(legend1);
            this.GSensor_chart.Location = new System.Drawing.Point(3, 3);
            this.GSensor_chart.Margin = new System.Windows.Forms.Padding(0);
            this.GSensor_chart.Name = "GSensor_chart";
            this.GSensor_chart.Size = new System.Drawing.Size(1344, 601);
            this.GSensor_chart.TabIndex = 3;
            this.GSensor_chart.Text = "chart1";
            this.GSensor_chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GSensor_chart_MouseMove);
            // 
            // Z_checkBox
            // 
            this.Z_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Z_checkBox.AutoSize = true;
            this.Z_checkBox.Checked = true;
            this.Z_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Z_checkBox.Location = new System.Drawing.Point(1263, 41);
            this.Z_checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.Z_checkBox.Name = "Z_checkBox";
            this.Z_checkBox.Size = new System.Drawing.Size(73, 16);
            this.Z_checkBox.TabIndex = 4;
            this.Z_checkBox.Text = "GSensor Z";
            this.Z_checkBox.UseVisualStyleBackColor = true;
            this.Z_checkBox.CheckedChanged += new System.EventHandler(this.Z_checkBox_CheckedChanged);
            // 
            // Y_checkBox
            // 
            this.Y_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Y_checkBox.AutoSize = true;
            this.Y_checkBox.Checked = true;
            this.Y_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Y_checkBox.Location = new System.Drawing.Point(1263, 25);
            this.Y_checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.Y_checkBox.Name = "Y_checkBox";
            this.Y_checkBox.Size = new System.Drawing.Size(74, 16);
            this.Y_checkBox.TabIndex = 5;
            this.Y_checkBox.Text = "GSensor Y";
            this.Y_checkBox.UseVisualStyleBackColor = true;
            this.Y_checkBox.CheckedChanged += new System.EventHandler(this.Y_checkBox_CheckedChanged);
            // 
            // X_checkBox
            // 
            this.X_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.X_checkBox.AutoSize = true;
            this.X_checkBox.Checked = true;
            this.X_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.X_checkBox.Location = new System.Drawing.Point(1263, 9);
            this.X_checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.X_checkBox.Name = "X_checkBox";
            this.X_checkBox.Size = new System.Drawing.Size(74, 16);
            this.X_checkBox.TabIndex = 6;
            this.X_checkBox.Text = "GSensor X";
            this.X_checkBox.UseVisualStyleBackColor = true;
            this.X_checkBox.CheckedChanged += new System.EventHandler(this.X_checkBox_CheckedChanged);
            // 
            // ChartLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 607);
            this.Controls.Add(this.X_checkBox);
            this.Controls.Add(this.Y_checkBox);
            this.Controls.Add(this.Z_checkBox);
            this.Controls.Add(this.GSensor_chart);
            this.Name = "ChartLine";
            this.Text = "ChartLine";
            ((System.ComponentModel.ISupportInitialize)(this.GSensor_chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart GSensor_chart;
        private System.Windows.Forms.CheckBox Z_checkBox;
        private System.Windows.Forms.CheckBox Y_checkBox;
        private System.Windows.Forms.CheckBox X_checkBox;




    }
}