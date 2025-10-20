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

        }

        private void btnAddAnimal_Click(object sender, EventArgs e)
        {
            string selectedType = cmbAnimalType.SelectedItem?.ToString() ?? "";
            int age = (int)nudAge.Value;
            Sex sex = (Sex)Enum.Parse(typeof(Sex), cmbSex.SelectedItem?.ToString() ?? "Erkek");

            if (!string.IsNullOrEmpty(selectedType))
            {
                _farmService.AddAnimal(selectedType, age, sex);
                LoadAnimals();
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
        private void LoadAnimals()
        {
            dgvAnimals.DataSource = null;
            dgvAnimals.DataSource = _farmService.GetAnimals();
        }
        private void cmbAnimalType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}