namespace SteamRevenueMLP
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblHidden = new System.Windows.Forms.Label();
            this.nudHidden = new System.Windows.Forms.NumericUpDown();
            this.lblLr = new System.Windows.Forms.Label();
            this.nudLr = new System.Windows.Forms.NumericUpDown();
            this.lblEpochs = new System.Windows.Forms.Label();
            this.nudEpochs = new System.Windows.Forms.NumericUpDown();
            this.lblEvery = new System.Windows.Forms.Label();
            this.nudEvery = new System.Windows.Forms.NumericUpDown();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnTune = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSaveCsv = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.pbChart = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudHidden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEvery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(110, 28);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load CSV";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(130, 19);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(81, 13);
            this.lblFile.TabIndex = 1;
            this.lblFile.Text = "No file loaded";
            // 
            // lblHidden
            // 
            this.lblHidden.AutoSize = true;
            this.lblHidden.Location = new System.Drawing.Point(12, 58);
            this.lblHidden.Name = "lblHidden";
            this.lblHidden.Size = new System.Drawing.Size(44, 13);
            this.lblHidden.TabIndex = 2;
            this.lblHidden.Text = "Hidden:";
            // 
            // nudHidden
            // 
            this.nudHidden.Location = new System.Drawing.Point(62, 54);
            this.nudHidden.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            this.nudHidden.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudHidden.Name = "nudHidden";
            this.nudHidden.Size = new System.Drawing.Size(60, 20);
            this.nudHidden.TabIndex = 3;
            this.nudHidden.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // lblLr
            // 
            this.lblLr.AutoSize = true;
            this.lblLr.Location = new System.Drawing.Point(140, 58);
            this.lblLr.Name = "lblLr";
            this.lblLr.Size = new System.Drawing.Size(25, 13);
            this.lblLr.TabIndex = 4;
            this.lblLr.Text = "LR:";
            // 
            // nudLr
            // 
            this.nudLr.DecimalPlaces = 3;
            this.nudLr.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            this.nudLr.Location = new System.Drawing.Point(170, 54);
            this.nudLr.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudLr.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
            this.nudLr.Name = "nudLr";
            this.nudLr.Size = new System.Drawing.Size(65, 20);
            this.nudLr.TabIndex = 5;
            this.nudLr.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // lblEpochs
            // 
            this.lblEpochs.AutoSize = true;
            this.lblEpochs.Location = new System.Drawing.Point(253, 58);
            this.lblEpochs.Name = "lblEpochs";
            this.lblEpochs.Size = new System.Drawing.Size(44, 13);
            this.lblEpochs.TabIndex = 6;
            this.lblEpochs.Text = "Epochs:";
            // 
            // nudEpochs
            // 
            this.nudEpochs.Location = new System.Drawing.Point(303, 54);
            this.nudEpochs.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudEpochs.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudEpochs.Name = "nudEpochs";
            this.nudEpochs.Size = new System.Drawing.Size(70, 20);
            this.nudEpochs.TabIndex = 7;
            this.nudEpochs.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // lblEvery
            // 
            this.lblEvery.AutoSize = true;
            this.lblEvery.Location = new System.Drawing.Point(391, 58);
            this.lblEvery.Name = "lblEvery";
            this.lblEvery.Size = new System.Drawing.Size(60, 13);
            this.lblEvery.TabIndex = 8;
            this.lblEvery.Text = "Log every:";
            // 
            // nudEvery
            // 
            this.nudEvery.Location = new System.Drawing.Point(457, 54);
            this.nudEvery.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudEvery.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudEvery.Name = "nudEvery";
            this.nudEvery.Size = new System.Drawing.Size(60, 20);
            this.nudEvery.TabIndex = 9;
            this.nudEvery.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(12, 90);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(100, 30);
            this.btnTrain.TabIndex = 10;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnTune
            // 
            this.btnTune.Location = new System.Drawing.Point(120, 90);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(130, 30);
            this.btnTune.TabIndex = 11;
            this.btnTune.Text = "Tune Parameters";
            this.btnTune.UseVisualStyleBackColor = true;
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(258, 90);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 30);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSaveCsv
            // 
            this.btnSaveCsv.Enabled = false;
            this.btnSaveCsv.Location = new System.Drawing.Point(356, 90);
            this.btnSaveCsv.Name = "btnSaveCsv";
            this.btnSaveCsv.Size = new System.Drawing.Size(110, 30);
            this.btnSaveCsv.TabIndex = 13;
            this.btnSaveCsv.Text = "Save CSV";
            this.btnSaveCsv.UseVisualStyleBackColor = true;
            this.btnSaveCsv.Click += new System.EventHandler(this.btnSaveCsv_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(12, 130);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(520, 458);
            this.txtLog.TabIndex = 14;
            this.txtLog.WordWrap = false;
            // 
            // pbChart
            // 
            this.pbChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbChart.BackColor = System.Drawing.Color.White;
            this.pbChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbChart.Location = new System.Drawing.Point(542, 130);
            this.pbChart.Name = "pbChart";
            this.pbChart.Size = new System.Drawing.Size(346, 458);
            this.pbChart.TabIndex = 15;
            this.pbChart.TabStop = false;
            this.pbChart.Paint += new System.Windows.Forms.PaintEventHandler(this.pbChart_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.pbChart);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnSaveCsv);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnTune);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.nudEvery);
            this.Controls.Add(this.lblEvery);
            this.Controls.Add(this.nudEpochs);
            this.Controls.Add(this.lblEpochs);
            this.Controls.Add(this.nudLr);
            this.Controls.Add(this.lblLr);
            this.Controls.Add(this.nudHidden);
            this.Controls.Add(this.lblHidden);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.btnLoad);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "Form1";
            this.Text = "Steam Revenue Classifier (Custom MLP)";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudHidden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEvery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblHidden;
        private System.Windows.Forms.NumericUpDown nudHidden;
        private System.Windows.Forms.Label lblLr;
        private System.Windows.Forms.NumericUpDown nudLr;
        private System.Windows.Forms.Label lblEpochs;
        private System.Windows.Forms.NumericUpDown nudEpochs;
        private System.Windows.Forms.Label lblEvery;
        private System.Windows.Forms.NumericUpDown nudEvery;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnTune;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSaveCsv;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.PictureBox pbChart;
    }
}