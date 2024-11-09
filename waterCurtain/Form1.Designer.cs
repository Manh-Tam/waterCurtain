namespace waterCurtain
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            imageBox = new PictureBox();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            ((System.ComponentModel.ISupportInitialize)imageBox).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(22, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(163, 27);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(22, 235);
            button1.Name = "button1";
            button1.Size = new Size(89, 31);
            button1.TabIndex = 1;
            button1.Text = "Insert";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Insert_Click;
            // 
            // button2
            // 
            button2.Location = new Point(149, 236);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 2;
            button2.Text = "Retrieve";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Retrieve_Click;
            // 
            // button3
            // 
            button3.Location = new Point(274, 237);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 3;
            button3.Text = "Update";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Update_Click;
            // 
            // button4
            // 
            button4.Location = new Point(22, 288);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 4;
            button4.Text = "Delete";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Delete_Click;
            // 
            // button5
            // 
            button5.Location = new Point(149, 288);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 5;
            button5.Text = "Delete all elements";
            button5.UseVisualStyleBackColor = true;
            button5.Click += DeleteAll_Click;
            // 
            // imageBox
            // 
            imageBox.BackColor = SystemColors.GradientInactiveCaption;
            imageBox.Location = new Point(428, 12);
            imageBox.Name = "imageBox";
            imageBox.Size = new Size(305, 219);
            imageBox.TabIndex = 6;
            imageBox.TabStop = false;
            // 
            // button6
            // 
            button6.Location = new Point(426, 237);
            button6.Name = "button6";
            button6.Size = new Size(94, 29);
            button6.TabIndex = 7;
            button6.Text = "Browse";
            button6.UseVisualStyleBackColor = true;
            button6.Click += Browse_Click;
            // 
            // button7
            // 
            button7.Location = new Point(526, 237);
            button7.Name = "button7";
            button7.Size = new Size(94, 29);
            button7.TabIndex = 8;
            button7.Text = "Insert";
            button7.UseVisualStyleBackColor = true;
            button7.Click += InsertImg_Click;
            // 
            // button8
            // 
            button8.Location = new Point(639, 237);
            button8.Name = "button8";
            button8.Size = new Size(94, 29);
            button8.TabIndex = 9;
            button8.Text = "Retrieve";
            button8.UseVisualStyleBackColor = true;
            button8.Click += RetrieveImg_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(800, 450);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(imageBox);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)imageBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private PictureBox imageBox;
        private Button button6;
        private Button button7;
        private Button button8;
    }
}
