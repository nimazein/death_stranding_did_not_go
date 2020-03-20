namespace csv_Parser
{
    partial class Queries
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbYear = new System.Windows.Forms.CheckBox();
            this.cbUnique = new System.Windows.Forms.CheckBox();
            this.cbAuthorsNumber = new System.Windows.Forms.CheckBox();
            this.cbArticle = new System.Windows.Forms.CheckBox();
            this.cbChapter = new System.Windows.Forms.CheckBox();
            this.tbMinYear = new System.Windows.Forms.TextBox();
            this.tbAuthorsNumber = new System.Windows.Forms.TextBox();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbMaxYear = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.cbKeywords = new System.Windows.Forms.CheckBox();
            this.cbSource = new System.Windows.Forms.CheckBox();
            this.btnToExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 333);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(598, 198);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExecute.Location = new System.Drawing.Point(12, 282);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(99, 45);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Выполнить запрос";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Год выпуска";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Название источника";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "Количество авторов";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(35, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 22);
            this.label4.TabIndex = 5;
            this.label4.Text = "Уникальные";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(34, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 22);
            this.label5.TabIndex = 6;
            this.label5.Text = "Конференция";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(34, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 22);
            this.label6.TabIndex = 7;
            this.label6.Text = "Ключевые слова";
            // 
            // cbYear
            // 
            this.cbYear.AutoSize = true;
            this.cbYear.Location = new System.Drawing.Point(12, 35);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(15, 14);
            this.cbYear.TabIndex = 8;
            this.cbYear.UseVisualStyleBackColor = true;
            this.cbYear.CheckedChanged += new System.EventHandler(this.cbYear_CheckedChanged);
            // 
            // cbUnique
            // 
            this.cbUnique.AutoSize = true;
            this.cbUnique.Location = new System.Drawing.Point(14, 250);
            this.cbUnique.Name = "cbUnique";
            this.cbUnique.Size = new System.Drawing.Size(15, 14);
            this.cbUnique.TabIndex = 9;
            this.cbUnique.UseVisualStyleBackColor = true;
            this.cbUnique.CheckedChanged += new System.EventHandler(this.cbUnique_CheckedChanged);
            // 
            // cbAuthorsNumber
            // 
            this.cbAuthorsNumber.AutoSize = true;
            this.cbAuthorsNumber.Location = new System.Drawing.Point(13, 116);
            this.cbAuthorsNumber.Name = "cbAuthorsNumber";
            this.cbAuthorsNumber.Size = new System.Drawing.Size(15, 14);
            this.cbAuthorsNumber.TabIndex = 10;
            this.cbAuthorsNumber.UseVisualStyleBackColor = true;
            this.cbAuthorsNumber.CheckedChanged += new System.EventHandler(this.cbAuthorsNumber_CheckedChanged);
            // 
            // cbArticle
            // 
            this.cbArticle.AutoSize = true;
            this.cbArticle.Location = new System.Drawing.Point(203, 208);
            this.cbArticle.Name = "cbArticle";
            this.cbArticle.Size = new System.Drawing.Size(15, 14);
            this.cbArticle.TabIndex = 12;
            this.cbArticle.UseVisualStyleBackColor = true;
            this.cbArticle.CheckedChanged += new System.EventHandler(this.cbArticle_CheckedChanged);
            // 
            // cbChapter
            // 
            this.cbChapter.AutoSize = true;
            this.cbChapter.Location = new System.Drawing.Point(12, 208);
            this.cbChapter.Name = "cbChapter";
            this.cbChapter.Size = new System.Drawing.Size(15, 14);
            this.cbChapter.TabIndex = 13;
            this.cbChapter.UseVisualStyleBackColor = true;
            this.cbChapter.CheckedChanged += new System.EventHandler(this.cbChapter_CheckedChanged);
            // 
            // tbMinYear
            // 
            this.tbMinYear.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.tbMinYear.Location = new System.Drawing.Point(325, 23);
            this.tbMinYear.Name = "tbMinYear";
            this.tbMinYear.Size = new System.Drawing.Size(81, 29);
            this.tbMinYear.TabIndex = 14;
            // 
            // tbAuthorsNumber
            // 
            this.tbAuthorsNumber.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.tbAuthorsNumber.Location = new System.Drawing.Point(257, 107);
            this.tbAuthorsNumber.Name = "tbAuthorsNumber";
            this.tbAuthorsNumber.Size = new System.Drawing.Size(353, 29);
            this.tbAuthorsNumber.TabIndex = 18;
            // 
            // tbSource
            // 
            this.tbSource.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.tbSource.Location = new System.Drawing.Point(257, 68);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(353, 29);
            this.tbSource.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(253, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 22);
            this.label7.TabIndex = 20;
            this.label7.Text = "между";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(424, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 22);
            this.label8.TabIndex = 21;
            this.label8.Text = "и";
            // 
            // tbMaxYear
            // 
            this.tbMaxYear.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.tbMaxYear.Location = new System.Drawing.Point(464, 23);
            this.tbMaxYear.Name = "tbMaxYear";
            this.tbMaxYear.Size = new System.Drawing.Size(81, 29);
            this.tbMaxYear.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(225, 202);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 22);
            this.label9.TabIndex = 23;
            this.label9.Text = "Журнал";
            // 
            // tbKeywords
            // 
            this.tbKeywords.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.tbKeywords.Location = new System.Drawing.Point(257, 148);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Size = new System.Drawing.Size(353, 29);
            this.tbKeywords.TabIndex = 24;
            this.tbKeywords.TextChanged += new System.EventHandler(this.tbKeywords_TextChanged);
            // 
            // cbKeywords
            // 
            this.cbKeywords.AutoSize = true;
            this.cbKeywords.Location = new System.Drawing.Point(13, 157);
            this.cbKeywords.Name = "cbKeywords";
            this.cbKeywords.Size = new System.Drawing.Size(15, 14);
            this.cbKeywords.TabIndex = 25;
            this.cbKeywords.UseVisualStyleBackColor = true;
            this.cbKeywords.CheckedChanged += new System.EventHandler(this.cbKeywords_CheckedChanged);
            // 
            // cbSource
            // 
            this.cbSource.AutoSize = true;
            this.cbSource.Location = new System.Drawing.Point(12, 77);
            this.cbSource.Name = "cbSource";
            this.cbSource.Size = new System.Drawing.Size(15, 14);
            this.cbSource.TabIndex = 26;
            this.cbSource.UseVisualStyleBackColor = true;
            this.cbSource.CheckedChanged += new System.EventHandler(this.cbSource_CheckedChanged);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnToExcel.Location = new System.Drawing.Point(499, 282);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(111, 45);
            this.btnToExcel.TabIndex = 27;
            this.btnToExcel.Text = "Экспорт в Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // Queries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 543);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.cbSource);
            this.Controls.Add(this.cbKeywords);
            this.Controls.Add(this.tbKeywords);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbMaxYear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.tbAuthorsNumber);
            this.Controls.Add(this.tbMinYear);
            this.Controls.Add(this.cbChapter);
            this.Controls.Add(this.cbArticle);
            this.Controls.Add(this.cbAuthorsNumber);
            this.Controls.Add(this.cbUnique);
            this.Controls.Add(this.cbYear);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Queries";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Queries";
            this.Load += new System.EventHandler(this.Queries_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbYear;
        private System.Windows.Forms.CheckBox cbUnique;
        private System.Windows.Forms.CheckBox cbAuthorsNumber;
        private System.Windows.Forms.CheckBox cbArticle;
        private System.Windows.Forms.CheckBox cbChapter;
        private System.Windows.Forms.TextBox tbMinYear;
        private System.Windows.Forms.TextBox tbAuthorsNumber;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbMaxYear;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbKeywords;
        private System.Windows.Forms.CheckBox cbKeywords;
        private System.Windows.Forms.CheckBox cbSource;
        private System.Windows.Forms.Button btnToExcel;
    }
}