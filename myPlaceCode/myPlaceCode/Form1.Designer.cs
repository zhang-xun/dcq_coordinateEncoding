namespace myPlaceCode
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCodeExp = new System.Windows.Forms.Label();
            this.lblCoor = new System.Windows.Forms.Label();
            this.lblExp = new System.Windows.Forms.Label();
            this.txtCoor = new System.Windows.Forms.TextBox();
            this.btnGenCode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRangeExp = new System.Windows.Forms.Label();
            this.txtUL = new System.Windows.Forms.TextBox();
            this.btnGenRangeCode = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBR = new System.Windows.Forms.TextBox();
            this.btnGenCodeShp = new System.Windows.Forms.Button();
            this.butchoosePolygonShp = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCodeExp
            // 
            this.lblCodeExp.Location = new System.Drawing.Point(95, 84);
            this.lblCodeExp.Name = "lblCodeExp";
            this.lblCodeExp.Size = new System.Drawing.Size(248, 51);
            this.lblCodeExp.TabIndex = 0;
            // 
            // lblCoor
            // 
            this.lblCoor.AutoSize = true;
            this.lblCoor.Location = new System.Drawing.Point(36, 48);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(41, 12);
            this.lblCoor.TabIndex = 0;
            this.lblCoor.Text = "坐标XY";
            // 
            // lblExp
            // 
            this.lblExp.AutoSize = true;
            this.lblExp.Location = new System.Drawing.Point(36, 84);
            this.lblExp.Name = "lblExp";
            this.lblExp.Size = new System.Drawing.Size(53, 12);
            this.lblExp.TabIndex = 0;
            this.lblExp.Text = "编码输出";
            // 
            // txtCoor
            // 
            this.txtCoor.Location = new System.Drawing.Point(97, 46);
            this.txtCoor.Name = "txtCoor";
            this.txtCoor.Size = new System.Drawing.Size(84, 21);
            this.txtCoor.TabIndex = 1;
            this.txtCoor.Text = "106.1256,28.32657";
            // 
            // btnGenCode
            // 
            this.btnGenCode.Location = new System.Drawing.Point(55, 138);
            this.btnGenCode.Name = "btnGenCode";
            this.btnGenCode.Size = new System.Drawing.Size(92, 30);
            this.btnGenCode.TabIndex = 2;
            this.btnGenCode.Text = "生成定位编码";
            this.btnGenCode.UseVisualStyleBackColor = true;
            this.btnGenCode.Click += new System.EventHandler(this.btnGenCode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "左上角XY";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "编码输出";
            // 
            // lblRangeExp
            // 
            this.lblRangeExp.Location = new System.Drawing.Point(95, 253);
            this.lblRangeExp.Name = "lblRangeExp";
            this.lblRangeExp.Size = new System.Drawing.Size(248, 51);
            this.lblRangeExp.TabIndex = 0;
            // 
            // txtUL
            // 
            this.txtUL.Location = new System.Drawing.Point(97, 215);
            this.txtUL.Name = "txtUL";
            this.txtUL.Size = new System.Drawing.Size(84, 21);
            this.txtUL.TabIndex = 1;
            this.txtUL.Text = "106.1256,28.32657";
            // 
            // btnGenRangeCode
            // 
            this.btnGenRangeCode.Location = new System.Drawing.Point(55, 307);
            this.btnGenRangeCode.Name = "btnGenRangeCode";
            this.btnGenRangeCode.Size = new System.Drawing.Size(92, 30);
            this.btnGenRangeCode.TabIndex = 2;
            this.btnGenRangeCode.Text = "生成范围编码";
            this.btnGenRangeCode.UseVisualStyleBackColor = true;
            this.btnGenRangeCode.Click += new System.EventHandler(this.btnGenRangeCode_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "右下角XY";
            // 
            // txtBR
            // 
            this.txtBR.Location = new System.Drawing.Point(259, 215);
            this.txtBR.Name = "txtBR";
            this.txtBR.Size = new System.Drawing.Size(84, 21);
            this.txtBR.TabIndex = 1;
            this.txtBR.Text = "108.36456,27.6557";
            // 
            // btnGenCodeShp
            // 
            this.btnGenCodeShp.Location = new System.Drawing.Point(349, 48);
            this.btnGenCodeShp.Name = "btnGenCodeShp";
            this.btnGenCodeShp.Size = new System.Drawing.Size(86, 58);
            this.btnGenCodeShp.TabIndex = 3;
            this.btnGenCodeShp.Text = "选择shp文件生成编码(点)";
            this.btnGenCodeShp.UseVisualStyleBackColor = true;
            this.btnGenCodeShp.Click += new System.EventHandler(this.btnGenCodeShp_Click);
            // 
            // butchoosePolygonShp
            // 
            this.butchoosePolygonShp.Location = new System.Drawing.Point(349, 212);
            this.butchoosePolygonShp.Name = "butchoosePolygonShp";
            this.butchoosePolygonShp.Size = new System.Drawing.Size(86, 53);
            this.butchoosePolygonShp.TabIndex = 4;
            this.butchoosePolygonShp.Text = "选择shp文件生成编码(面)";
            this.butchoosePolygonShp.UseVisualStyleBackColor = true;
            this.butchoosePolygonShp.Click += new System.EventHandler(this.butchoosePolygonShp_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(441, 48);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(300, 300);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 399);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.butchoosePolygonShp);
            this.Controls.Add(this.btnGenCodeShp);
            this.Controls.Add(this.btnGenRangeCode);
            this.Controls.Add(this.txtBR);
            this.Controls.Add(this.txtUL);
            this.Controls.Add(this.btnGenCode);
            this.Controls.Add(this.lblRangeExp);
            this.Controls.Add(this.txtCoor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCodeExp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblExp);
            this.Controls.Add(this.lblCoor);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCodeExp;
        private System.Windows.Forms.Label lblCoor;
        private System.Windows.Forms.Label lblExp;
        private System.Windows.Forms.TextBox txtCoor;
        private System.Windows.Forms.Button btnGenCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRangeExp;
        private System.Windows.Forms.TextBox txtUL;
        private System.Windows.Forms.Button btnGenRangeCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBR;
        private System.Windows.Forms.Button btnGenCodeShp;
        private System.Windows.Forms.Button butchoosePolygonShp;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

