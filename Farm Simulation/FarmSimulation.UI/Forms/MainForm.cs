﻿﻿﻿﻿﻿﻿using FarmSimulation.Business.Services;
using FarmSimulation.Data.Models;


namespace Farm_Simulation.Forms
{
    public partial class MainForm : Form
    {
        private readonly FarmService _farmService = new FarmService();
        private System.Windows.Forms.Timer _ageTimer = null!;
        private const int AGE_INCREASE_INTERVAL = 10000; // 10 saniye (milisaniye cinsinden)

        public MainForm()
        {
            InitializeComponent();
            InitializeAgeTimer();
        }

        private void InitializeAgeTimer()
        {
            _ageTimer = new System.Windows.Forms.Timer();
            _ageTimer.Interval = AGE_INCREASE_INTERVAL;
            _ageTimer.Tick += AgeTimer_Tick;
            _ageTimer.Start();
        }

        private void AgeTimer_Tick(object? sender, EventArgs e)
        {
            // Tüm hayvanların yaşını artır
            _farmService.IncreaseAllAnimalsAge();
            
            // UI'yi güncelle
            RefreshUI();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddAnimal_Click(object sender, EventArgs e)
        {
            string selectedName = cmbAnimalType.SelectedItem?.ToString() ?? "";
            int age = (int)nudAge.Value;
            Sex sex = (Sex)Enum.Parse(typeof(Sex), cmbSex.SelectedItem?.ToString() ?? "Erkek");

            if (!string.IsNullOrEmpty(selectedName))
            {
                _farmService.AddAnimal(selectedName, age, sex);
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
            lblCash.Text = $"Para: {cash} ₺";

            // Yaşam süresi bilgilerini güncelle
            int aliveCount = _farmService.GetAliveAnimalCount();
            int deadCount = _farmService.GetDeadAnimalCount();
            this.Text = $"Farm Simulation - Yaşayan: {aliveCount} | Ölü: {deadCount}";
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
            
            // Ölü hayvanları temizle
            int removedCount = _farmService.RemoveDeadAnimals();
            if (removedCount > 0)
            {
                MessageBox.Show($"{removedCount} ölü hayvan çiftlikten kaldırıldı.", "Bilgi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
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

        private void btnZeroMoney_Click(object sender, EventArgs e)
        {
            _farmService.ResetCash();
            RefreshUI();
        }
    }
}