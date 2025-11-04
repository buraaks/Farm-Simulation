using FarmSimulation.Business.Services;
using FarmSimulation.Entities;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmSimulation.UI.Forms
{
    public partial class BuyAnimalForm : Form
    {
        private FarmBusinessService businessService;

        public BuyAnimalForm(FarmBusinessService businessService)
        {
            InitializeComponent();
            this.businessService = businessService;
            UpdatePrice();
        }

        private void AnimalTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            string? selectedType = animalTypeComboBox.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedType))
            {
                decimal price = GetAnimalPrice(selectedType);
                priceLabel.Text = price.ToString("C");
            }
        }

        private decimal GetAnimalPrice(string? animalType)
        {
            if (string.IsNullOrEmpty(animalType))
                return 0m;
                
            return businessService.GetAnimalPrice(animalType);
        }

        private async void BuyButton_Click(object? sender, EventArgs e)
        {
            string? animalName = animalNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(animalName))
            {
                MessageBox.Show("Please enter an animal name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? selectedType = animalTypeComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedType))
            {
                MessageBox.Show("Please select an animal type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal price = GetAnimalPrice(selectedType);

            if (businessService.Cash.Subtract(price))
            {
                // Create and add the new animal
                Animal newAnimal = businessService.CreateAnimal(selectedType, animalName);
                await businessService.AddAnimalAsync(newAnimal);

                MessageBox.Show($"{selectedType} named '{animalName}' purchased successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Insufficient funds!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}