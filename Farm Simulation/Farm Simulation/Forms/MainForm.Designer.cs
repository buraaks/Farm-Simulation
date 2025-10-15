namespace Farm_Simulation.Forms
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
            ((System.ComponentModel.ISupportInitialize)dgvAnimals).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // dgvAnimals
            // 
            dgvAnimals.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAnimals.Location = new Point(465, 12);
            dgvAnimals.Name = "dgvAnimals";
            dgvAnimals.RowHeadersWidth = 51;
            dgvAnimals.Size = new Size(300, 188);
            dgvAnimals.TabIndex = 0;
            // 
            // dgvProducts
            // 
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(465, 221);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.Size = new Size(300, 188);
            dgvProducts.TabIndex = 1;
            // 
            // lblCash
            // 
            lblCash.AutoSize = true;
            lblCash.Location = new Point(75, 35);
            lblCash.Name = "lblCash";
            lblCash.Size = new Size(63, 20);
            lblCash.TabIndex = 2;
            lblCash.Text = "Cash: 0₺";
            // 
            // btnAddAnimal
            // 
            btnAddAnimal.Location = new Point(185, 77);
            btnAddAnimal.Name = "btnAddAnimal";
            btnAddAnimal.Size = new Size(94, 29);
            btnAddAnimal.TabIndex = 3;
            btnAddAnimal.Text = "Ekle";
            btnAddAnimal.UseVisualStyleBackColor = true;
            btnAddAnimal.Click += btnAddAnimal_Click;
            // 
            // btnCollectProducts
            // 
            btnCollectProducts.Location = new Point(57, 155);
            btnCollectProducts.Name = "btnCollectProducts";
            btnCollectProducts.Size = new Size(122, 29);
            btnCollectProducts.TabIndex = 4;
            btnCollectProducts.Text = "Ürünleri Topla";
            btnCollectProducts.UseVisualStyleBackColor = true;
            // 
            // btnSellAll
            // 
            btnSellAll.Location = new Point(57, 190);
            btnSellAll.Name = "btnSellAll";
            btnSellAll.Size = new Size(122, 29);
            btnSellAll.TabIndex = 6;
            btnSellAll.Text = "Ürünleri Sat";
            btnSellAll.UseVisualStyleBackColor = true;
            // 
            // cmbAnimalType
            // 
            cmbAnimalType.FormattingEnabled = true;
            cmbAnimalType.Location = new Point(28, 78);
            cmbAnimalType.Name = "cmbAnimalType";
            cmbAnimalType.Size = new Size(151, 28);
            cmbAnimalType.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}