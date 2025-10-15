using FarmSimulation.Business.Services;
using FarmSimulation.Data.Models;


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
                    animal = new Chicken { Name = "Tavuk", Age = 0, Sex = Sex.Female };
                    break;
                case "Cow":
                    animal = new Cow { Name = "İnek", Age = 0, Sex = Sex.Female };
                    break;
                case "Sheep":
                    animal = new Sheep { Name = "Koyun", Age = 0, Sex = Sex.Female };
                    break;
                case "Goat":
                    animal = new Goat { Name = "Keçi", Age = 0, Sex = Sex.Female };
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
            // Hayvanları listele
            dgvAnimals.DataSource = null;
            var animals = _farmService.GetAnimals();
            dgvAnimals.DataSource = animals ?? new List<AnimalBase>();

            // Ürünleri listele
            dgvProducts.DataSource = null;
            var products = _farmService.GetProducts();
            dgvProducts.DataSource = products ?? new List<Product>();

            // Kasa güncelle
            var cash = _farmService.GetCash();
            lblCash.Text = $"Cash: {cash} ₺";

        }

        private void btnCollectProducts_Click(object sender, EventArgs e)
        {
            progressProduction.Minimum = 0;
            progressProduction.Maximum = 100;
            progressProduction.Value = 0;

            for (int i = 0; i <= 100; i += 20)
            {
                progressProduction.Value = i;
                progressProduction.Refresh();
                System.Threading.Thread.Sleep(100);
            }

            _farmService.CollectProducts();
            RefreshUI();

            progressProduction.Value = 0;
        }

        private void btnSellAll_Click(object sender, EventArgs e)
        {
            _farmService.SellAllProducts();
            RefreshUI();
            progressProduction.Value = 0;
        }

        private void cmbAnimalType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}