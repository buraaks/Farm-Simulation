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
            this.businessService = businessService;
            InitializeComponent();
            InitializeAnimalTypes();
            UpdatePrice();
        }

        private void InitializeAnimalTypes()
        {
            animalTypeComboBox.Items.Add(AnimalTypes.Chicken);
            animalTypeComboBox.Items.Add(AnimalTypes.Cow);
            animalTypeComboBox.Items.Add(AnimalTypes.Sheep);
            animalTypeComboBox.SelectedIndex = 0;
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
            if (string.IsNullOrEmpty(animalType)) return 0m;
            if (businessService == null) return 0m;
            return businessService.GetAnimalPrice(animalType);
        }

        private async void BuyButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string? animalName = animalNameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(animalName))
                {
                    MessageBox.Show("Please enter an animal name.", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string? selectedType = animalTypeComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedType))
                {
                    MessageBox.Show("Please select an animal type.", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal price = GetAnimalPrice(selectedType);

                // Nakit yeterli mi?
                if (businessService.Cash.Amount >= price)
                {
                    // Nakit düş
                    businessService.Cash.Amount -= price;

                    // Hayvan oluştur/ekle
                    Animal newAnimal = businessService.CreateAnimal(selectedType, animalName);
                    await businessService.AddAnimalAsync(newAnimal);
                    
                    // Nakit kaydet
                    businessService.dataAccess.UpdateCash(businessService.Cash);
                    await businessService.dataAccess.SaveChangesAsync();

                    MessageBox.Show($"{selectedType} '{animalName}' was purchased successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Insufficient funds! You need {price:C} to buy this animal.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while purchasing animal: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
