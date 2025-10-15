using FarmSim.Business.Services;
using FarmSim.Data.Models;
using FarmSimulation.Business.Services;
using FarmSimulation.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Farm_Simulation.Forms
{
    public partial class MainForm : Form
    {
        private readonly FarmService _farmService = new FarmService();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbAnimalType.Items.AddRange(new object[]
            {
                "Chicken",
                "Cow",
                "Sheep",
                "Goat"
            });
            cmbAnimalType.SelectedIndex = 0;

            RefreshUI();
        }

        private void btnAddAnimal_Click(object sender, EventArgs e)
        {
            if (cmbAnimalType.SelectedItem is null)
                return;
            string selected = cmbAnimalType.SelectedItem.ToString()!;
            AnimalBase? animal = null;

            switch (selected)
            {
                case "Chicken":
                    animal = new Chicken("Tavuk", 0, Sex.Female);
                    break;
                case "Cow":
                    animal = new Cow("İnek", 0, Sex.Female);
                    break;
                case "Sheep":
                    animal = new Sheep("Koyun", 0, Sex.Female);
                    break;
                case "Goat":
                    animal = new Goat("Keçi", 0, Sex.Female);
                    break;
            }

            if (animal != null)
            {
                _farmService.AddAnimal(animal);
                RefreshUI();
            }
        }
        private void RefreshUI()
        {
            dgvAnimals.DataSource = null;
            dgvAnimals.DataSource = _farmService.GetAnimals();

            dgvProducts.DataSource = null;
            dgvProducts.DataSource = _farmService.GetProducts();

            lblCash.Text = $"Cash: {_farmService.CashBalance} ₺";
        }

    }
}
