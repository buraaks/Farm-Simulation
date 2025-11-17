namespace FarmSimulation.UI.Forms
{
    partial class BuyAnimalForm
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
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Text = "Buy Animal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Animal name input
            var nameLabel = new System.Windows.Forms.Label();
            nameLabel.Location = new System.Drawing.Point(20, 20);
            nameLabel.Size = new System.Drawing.Size(100, 20);
            nameLabel.Text = "Animal Name:";

            this.animalNameTextBox = new System.Windows.Forms.TextBox();
            this.animalNameTextBox.Location = new System.Drawing.Point(130, 20);
            this.animalNameTextBox.Size = new System.Drawing.Size(120, 25);

            // Animal type selection
            var typeLabel = new System.Windows.Forms.Label();
            typeLabel.Location = new System.Drawing.Point(20, 60);
            typeLabel.Size = new System.Drawing.Size(100, 20);
            typeLabel.Text = "Animal Type:";

            this.animalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.animalTypeComboBox.Location = new System.Drawing.Point(130, 60);
            this.animalTypeComboBox.Size = new System.Drawing.Size(120, 25);
            this.animalTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.AnimalTypeComboBox_SelectedIndexChanged);

            // Price display
            var priceTextLabel = new System.Windows.Forms.Label();
            priceTextLabel.Location = new System.Drawing.Point(20, 100);
            priceTextLabel.Size = new System.Drawing.Size(100, 20);
            priceTextLabel.Text = "Price:";

            this.priceLabel = new System.Windows.Forms.Label();
            this.priceLabel.Location = new System.Drawing.Point(130, 100);
            this.priceLabel.Size = new System.Drawing.Size(100, 20);
            this.priceLabel.Text = GetAnimalPrice("Chicken").ToString("C");
            this.priceLabel.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);

            // Buy button
            this.buyButton = new System.Windows.Forms.Button();
            this.buyButton.Location = new System.Drawing.Point(50, 140);
            this.buyButton.Size = new System.Drawing.Size(80, 30);
            this.buyButton.Text = "Buy";
            this.buyButton.BackColor = System.Drawing.Color.LightGreen;
            this.buyButton.Click += new System.EventHandler(this.BuyButton_Click);

            // Cancel button
            this.cancelButton = new System.Windows.Forms.Button();
            this.cancelButton.Location = new System.Drawing.Point(170, 140);
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.Text = "Cancel";
            this.cancelButton.BackColor = System.Drawing.Color.LightCoral;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);

            // Add controls to the form
            this.Controls.Add(nameLabel);
            this.Controls.Add(this.animalNameTextBox);
            this.Controls.Add(typeLabel);
            this.Controls.Add(this.animalTypeComboBox);
            this.Controls.Add(priceTextLabel);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.buyButton);
            this.Controls.Add(this.cancelButton);

            // Set initial price
            UpdatePrice();
        }

        #endregion

        private System.Windows.Forms.TextBox animalNameTextBox;
        private System.Windows.Forms.ComboBox animalTypeComboBox;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.Button buyButton;
        private System.Windows.Forms.Button cancelButton;
    }
}