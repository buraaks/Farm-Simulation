﻿namespace Farm_Simulation.Forms
{
    partial class MainForm
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
            
            // Timer'ı da temizle
            if (disposing)
            {
                _ageTimer?.Stop();
                _ageTimer?.Dispose();
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
            dgvAnimals = new DataGridView();
            dgvProducts = new DataGridView();
            lblCash = new Label();
            btnAddAnimal = new Button();
            btnCollectProducts = new Button();
            btnSellAll = new Button();
            cmbAnimalType = new ComboBox();
            progressProduction = new ProgressBar();
            cmbSex = new ComboBox();
            nudAge = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnZeroMoney = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAge).BeginInit();
            SuspendLayout();
            // 
            // dgvAnimals
            // 
            dgvAnimals.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAnimals.Location = new Point(350, 12);
            dgvAnimals.Name = "dgvAnimals";
            dgvAnimals.RowHeadersWidth = 51;
            dgvAnimals.Size = new Size(713, 260);
            dgvAnimals.TabIndex = 0;
            // 
            // dgvProducts
            // 
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(350, 301);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.Size = new Size(713, 260);
            dgvProducts.TabIndex = 1;
            // 
            // lblCash
            // 
            lblCash.AutoSize = true;
            lblCash.Location = new Point(12, 9);
            lblCash.Name = "lblCash";
            lblCash.Size = new Size(60, 20);
            lblCash.TabIndex = 2;
            lblCash.Text = "Para: 0₺";
            // 
            // btnAddAnimal
            // 
            btnAddAnimal.Location = new Point(238, 59);
            btnAddAnimal.Name = "btnAddAnimal";
            btnAddAnimal.Size = new Size(94, 150);
            btnAddAnimal.TabIndex = 3;
            btnAddAnimal.Text = "Ekle";
            btnAddAnimal.UseVisualStyleBackColor = true;
            btnAddAnimal.Click += btnAddAnimal_Click;
            // 
            // btnCollectProducts
            // 
            btnCollectProducts.Location = new Point(12, 482);
            btnCollectProducts.Name = "btnCollectProducts";
            btnCollectProducts.Size = new Size(159, 29);
            btnCollectProducts.TabIndex = 4;
            btnCollectProducts.Text = "Ürünleri Topla";
            btnCollectProducts.UseVisualStyleBackColor = true;
            btnCollectProducts.Click += btnCollectProducts_Click;
            // 
            // btnSellAll
            // 
            btnSellAll.Location = new Point(181, 482);
            btnSellAll.Name = "btnSellAll";
            btnSellAll.Size = new Size(159, 29);
            btnSellAll.TabIndex = 6;
            btnSellAll.Text = "Ürünleri Sat";
            btnSellAll.UseVisualStyleBackColor = true;
            btnSellAll.Click += btnSellAll_Click;
            // 
            // cmbAnimalType
            // 
            cmbAnimalType.FormattingEnabled = true;
            cmbAnimalType.Items.AddRange(new object[] { "Tavuk", "İnek", "Koyun", "Keçi" });
            cmbAnimalType.Location = new Point(81, 59);
            cmbAnimalType.Name = "cmbAnimalType";
            cmbAnimalType.Size = new Size(151, 28);
            cmbAnimalType.TabIndex = 7;
            cmbAnimalType.SelectedIndexChanged += cmbAnimalType_SelectedIndexChanged;
            // 
            // progressProduction
            // 
            progressProduction.Location = new Point(12, 532);
            progressProduction.Name = "progressProduction";
            progressProduction.Size = new Size(332, 29);
            progressProduction.TabIndex = 8;
            // 
            // cmbSex
            // 
            cmbSex.FormattingEnabled = true;
            cmbSex.Items.AddRange(new object[] { "Erkek", "Kız" });
            cmbSex.Location = new Point(81, 186);
            cmbSex.Name = "cmbSex";
            cmbSex.Size = new Size(151, 28);
            cmbSex.TabIndex = 9;
            // 
            // nudAge
            // 
            nudAge.Location = new Point(82, 124);
            nudAge.Name = "nudAge";
            nudAge.Size = new Size(150, 27);
            nudAge.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 62);
            label1.Name = "label1";
            label1.Size = new Size(33, 20);
            label1.TabIndex = 11;
            label1.Text = "Tür:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 126);
            label2.Name = "label2";
            label2.Size = new Size(33, 20);
            label2.TabIndex = 12;
            label2.Text = "Yaş:";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 189);
            label3.Name = "label3";
            label3.Size = new Size(63, 20);
            label3.TabIndex = 13;
            label3.Text = "Cinsiyet:";
            // 
            // btnZeroMoney
            // 
            btnZeroMoney.Location = new Point(12, 32);
            btnZeroMoney.Name = "btnZeroMoney";
            btnZeroMoney.Size = new Size(19, 25);
            btnZeroMoney.TabIndex = 14;
            btnZeroMoney.Text = "0";
            btnZeroMoney.UseVisualStyleBackColor = true;
            btnZeroMoney.Click += btnZeroMoney_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1075, 579);
            Controls.Add(btnZeroMoney);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(nudAge);
            Controls.Add(cmbSex);
            Controls.Add(progressProduction);
            Controls.Add(cmbAnimalType);
            Controls.Add(btnSellAll);
            Controls.Add(btnCollectProducts);
            Controls.Add(btnAddAnimal);
            Controls.Add(lblCash);
            Controls.Add(dgvProducts);
            Controls.Add(dgvAnimals);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAge).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvAnimals;
        private DataGridView dgvProducts;
        private Label lblCash;
        private Button btnAddAnimal;
        private Button btnCollectProducts;
        private Button btnSellAll;
        private ComboBox cmbAnimalType;
        private ProgressBar progressProduction;
        private ComboBox cmbSex;
        private NumericUpDown nudAge;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnZeroMoney;
    }
}