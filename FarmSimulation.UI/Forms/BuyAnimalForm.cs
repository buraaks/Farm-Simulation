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

        private string GetFullErrorMessage(Exception ex)
        {
            return ex.ToString(); // For debugging
        }

        private async void BuyButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string? animalName = animalNameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(animalName))
                {
                    MessageBox.Show(ErrorMessages.AnimalNameRequired, "Uyarı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string? selectedType = animalTypeComboBox.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedType))
                {
                    MessageBox.Show(ErrorMessages.AnimalTypeRequired, "Uyarı", 
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

                    MessageBox.Show($"{selectedType} '{animalName}' başarıyla satın alındı!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(string.Format(ErrorMessages.InsufficientFunds, price.ToString("C")), "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Hata", 
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
