namespace FarmSimulation.UI.Forms
{
    partial class MainForm
    {
        // Required designer variable.
        private System.ComponentModel.IContainer components = null;

        // Clean up any resources being used.
        // disposing: true if managed resources should be disposed; otherwise, false.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 750);
            this.Text = "Farm Simulation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
            // Create animals grid
            this.animalsGrid = new DataGridView();
            this.animalsGrid.Location = new System.Drawing.Point(20, 20);
            this.animalsGrid.Size = new System.Drawing.Size(600, 200);
            this.animalsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.animalsGrid.AllowUserToAddRows = false;
            this.animalsGrid.AllowUserToDeleteRows = false;
            this.animalsGrid.ReadOnly = true;

            // Add columns to animals grid
            this.animalsGrid.Columns.Add("Id", "ID");
            this.animalsGrid.Columns.Add("Name", "Name");
            this.animalsGrid.Columns.Add("Age", "Age");
            this.animalsGrid.Columns.Add("Type", "Type");
            this.animalsGrid.Columns.Add("IsAlive", "Is Alive");
            this.animalsGrid.Columns.Add("ProductionProgress", "Production Progress");

            // Create products grid
            this.productsGrid = new DataGridView();
            this.productsGrid.Location = new System.Drawing.Point(20, 250);
            this.productsGrid.Size = new System.Drawing.Size(600, 200);
            this.productsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.productsGrid.AllowUserToAddRows = false;
            this.productsGrid.AllowUserToDeleteRows = false;
            this.productsGrid.ReadOnly = true;

            // Add columns to products grid
            this.productsGrid.Columns.Add("Id", "ID");
            this.productsGrid.Columns.Add("ProductType", "Product Type");
            this.productsGrid.Columns.Add("Quantity", "Quantity");
            this.productsGrid.Columns.Add("Price", "Price");
            this.productsGrid.Columns.Add("IsSold", "Is Sold");

            // Create cash label
            this.cashLabel = new Label();
            this.cashLabel.Location = new System.Drawing.Point(650, 20);
            this.cashLabel.Size = new System.Drawing.Size(300, 30);
            this.cashLabel.Text = "Cash: 0.00 TL";
            this.cashLabel.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);

            // Create buttons
            this.buyAnimalButton = new Button();
            this.buyAnimalButton.Location = new System.Drawing.Point(650, 70);
            this.buyAnimalButton.Size = new System.Drawing.Size(120, 40);
            this.buyAnimalButton.Text = "Buy Animal";
            this.buyAnimalButton.Font = new System.Drawing.Font("Arial", 10);
            this.buyAnimalButton.Click += new System.EventHandler(this.BuyAnimalButton_Click);

            this.collectSelectedAnimalProductsButton = new Button();
            this.collectSelectedAnimalProductsButton.Location = new System.Drawing.Point(650, 120);
            this.collectSelectedAnimalProductsButton.Size = new System.Drawing.Size(120, 40);
            this.collectSelectedAnimalProductsButton.Text = "Collect Selected Animal Products";
            this.collectSelectedAnimalProductsButton.Font = new System.Drawing.Font("Arial", 8);
            this.collectSelectedAnimalProductsButton.Click += new System.EventHandler(this.CollectSelectedAnimalProductsButton_Click);

            this.collectAllProductsButton = new Button();
            this.collectAllProductsButton.Location = new System.Drawing.Point(650, 170);
            this.collectAllProductsButton.Size = new System.Drawing.Size(120, 40);
            this.collectAllProductsButton.Text = "Collect All Products";
            this.collectAllProductsButton.Font = new System.Drawing.Font("Arial", 10);
            this.collectAllProductsButton.Click += new System.EventHandler(this.CollectAllProductsButton_Click);

            this.sellSelectedProductButton = new Button();
            this.sellSelectedProductButton.Location = new System.Drawing.Point(650, 220);
            this.sellSelectedProductButton.Size = new System.Drawing.Size(120, 40);
            this.sellSelectedProductButton.Text = "Sell Selected Product";
            this.sellSelectedProductButton.Font = new System.Drawing.Font("Arial", 9);
            this.sellSelectedProductButton.Click += new System.EventHandler(this.SellSelectedProductButton_Click);

            this.sellAllProductsButton = new Button();
            this.sellAllProductsButton.Location = new System.Drawing.Point(650, 270);
            this.sellAllProductsButton.Size = new System.Drawing.Size(120, 40);
            this.sellAllProductsButton.Text = "Sell All Products";
            this.sellAllProductsButton.Font = new System.Drawing.Font("Arial", 10);
            this.sellAllProductsButton.Click += new System.EventHandler(this.SellAllProductsButton_Click);

            this.sellProductsButton = new Button();
            this.sellProductsButton.Location = new System.Drawing.Point(650, 320);
            this.sellProductsButton.Size = new System.Drawing.Size(120, 40);
            this.sellProductsButton.Text = "Sell Products";
            this.sellProductsButton.Font = new System.Drawing.Font("Arial", 10);
            this.sellProductsButton.Click += new System.EventHandler(this.SellProductsButton_Click);

            this.deleteSoldProductsButton = new Button();
            this.deleteSoldProductsButton.Location = new System.Drawing.Point(650, 370);
            this.deleteSoldProductsButton.Size = new System.Drawing.Size(120, 40);
            this.deleteSoldProductsButton.Text = "Delete Sold Products";
            this.deleteSoldProductsButton.Font = new System.Drawing.Font("Arial", 10);
            this.deleteSoldProductsButton.Click += new System.EventHandler(this.DeleteSoldProductsButton_Click);

            // Add controls to the form
            this.Controls.Add(this.animalsGrid);
            this.Controls.Add(this.productsGrid);
            this.Controls.Add(this.cashLabel);
            this.Controls.Add(this.buyAnimalButton);
            this.Controls.Add(this.collectSelectedAnimalProductsButton);
            this.Controls.Add(this.collectAllProductsButton);
            this.Controls.Add(this.sellSelectedProductButton);
            this.Controls.Add(this.sellAllProductsButton);
            this.Controls.Add(this.sellProductsButton);
            this.Controls.Add(this.deleteSoldProductsButton);

            this.ResumeLayout(false);
        }

        #endregion

        private DataGridView animalsGrid;
        private DataGridView productsGrid;
        private Label cashLabel;
        private Button buyAnimalButton;
        private Button collectSelectedAnimalProductsButton;
        private Button collectAllProductsButton;
        private Button sellSelectedProductButton;
        private Button sellAllProductsButton;
        private Button sellProductsButton;
        private Button deleteSoldProductsButton;
        private System.Windows.Forms.Timer simulationTimer;
    }
}