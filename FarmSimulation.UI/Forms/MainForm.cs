using FarmSimulation.Business.Data;
using FarmSimulation.Business.Services;
using FarmSimulation.Data;
using FarmSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmSimulation.UI.Forms
{
    public partial class MainForm : Form
    {
        private FarmBusinessService? businessService;
        private FarmDataAccess? dataAccess;
        private FarmDbContext? dbContext;

        public MainForm()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeTimer();

            // async işlemleri form yüklendikten sonra başlat
            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            await InitializeServices();
        }

        private void InitializeDatabase()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Application.StartupPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<FarmDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            dbContext = new FarmDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();
        }

        private async Task InitializeServices()
        {
            buyAnimalButton.Enabled = false;

            dataAccess = new FarmDataAccess(dbContext);
            businessService = new FarmBusinessService(dataAccess);
            await businessService.InitializeAsync();

            UpdateUI();
            buyAnimalButton.Enabled = true;
        }

        private void InitializeTimer()
        {
            simulationTimer = new System.Windows.Forms.Timer();
            simulationTimer.Interval = 1000;
            simulationTimer.Tick += SimulationTimer_Tick;
            simulationTimer.Start();
        }

        private async void BuyAnimalButton_Click(object? sender, EventArgs e)
        {
            if (businessService == null)
            {
                MessageBox.Show("Sistem hala başlatılıyor. Lütfen biraz bekleyin.",
                    "Başlatma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var buyForm = new BuyAnimalForm(businessService);
            var result = buyForm.ShowDialog();

            if (result == DialogResult.OK)
                await UpdateUIAsync();
        }

        private async void CollectSelectedAnimalProductsButton_Click(object? sender, EventArgs e)
        {
            if (businessService == null) return;
            if (animalsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen önce bir hayvan seçin.");
                return;
            }

            var selectedRow = animalsGrid.SelectedRows[0];
            var animalId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            var animal = businessService.Animals.FirstOrDefault(a => a.Id == animalId);

            if (animal == null)
            {
                MessageBox.Show("Seçilen hayvan bulunamadı.");
                return;
            }

            var product = await businessService.CollectProductFromAnimalAsync(animal);
            if (product != null && product.Name != "Unknown Product")
            {
                MessageBox.Show($"{animal.Name} hayvanından {product.Name} toplandı!");
                await UpdateUIAsync();
            }
            else
            {
                MessageBox.Show("Bu hayvandan ürün toplanamadı.");
            }
        }

        private async void SellProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;
            decimal earnings = await businessService.SellProductsAsync();
            MessageBox.Show($"Ürünler {earnings:C} karşılığında satıldı!");
            await UpdateUIAsync();
        }

        private async void DeleteSoldProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;
            int deletedCount = await businessService.DeleteSoldProductsAsync();
            MessageBox.Show($"{deletedCount} satılmış ürün silindi!");
            await UpdateUIAsync();
        }

        private async void CollectAllProductsButton_Click(object? sender, EventArgs e)
        {
            if (businessService == null) return;
            int collectedCount = 0;
            foreach (var animal in businessService.Animals.Where(a => a.IsAlive && a.CanProduce).ToList())
            {
                var product = await businessService.CollectProductFromAnimalAsync(animal);
                if (product != null) collectedCount++;
            }

            MessageBox.Show($"{collectedCount} ürün toplandı!");
            await UpdateUIAsync();
        }

        private async void SellSelectedProductButton_Click(object? sender, EventArgs e)
        {
            if (businessService == null) return;
            if (productsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen önce bir ürün seçin.");
                return;
            }

            var selectedRow = productsGrid.SelectedRows[0];
            var productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            var product = businessService.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null) return;

            if (!product.IsSold)
            {
                product.IsSold = true;
                businessService.Cash.Amount += product.Price * product.Quantity;
                await businessService.dataAccess.UpdateProductAsync(product);
                MessageBox.Show($"{product.Name} {product.Price * product.Quantity:C} karşılığında satıldı!");
                await UpdateUIAsync();
            }
            else
            {
                MessageBox.Show("Bu ürün zaten satılmış.");
            }
        }

        private async void SellAllProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;
            decimal totalEarnings = 0m;
            int soldCount = 0;

            foreach (var product in businessService.Products.Where(p => !p.IsSold).ToList())
            {
                product.IsSold = true;
                totalEarnings += product.Price * product.Quantity;
                soldCount++;
                await businessService.dataAccess.UpdateProductAsync(product);
            }

            if (soldCount > 0)
            {
                businessService.Cash.Amount += totalEarnings;
                MessageBox.Show($"{soldCount} ürün {totalEarnings:C} karşılığında satıldı!");
                await UpdateUIAsync();
            }
            else
            {
                MessageBox.Show("Satılmamış ürün bulunamadı.");
            }
        }

        private void UpdateUI()
        {
            if (businessService == null) return;
            cashLabel.Text = $"Nakit: {businessService.Cash.Amount:C}";

            animalsGrid.Rows.Clear();
            foreach (var animal in businessService.Animals)
            {
                int index = animalsGrid.Rows.Add(
                    animal.Id,
                    animal.Name,
                    animal.Age,
                    animal.Type,
                    animal.IsAlive ? "Evet" : "Hayır",
                    animal.ProductProductionProgress + "%"
                );
                if (!animal.IsAlive)
                    animalsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
            }

            productsGrid.Rows.Clear();
            foreach (var product in businessService.Products)
            {
                int index = productsGrid.Rows.Add(
                    product.Id,
                    product.ProductType,
                    product.Quantity,
                    product.Price.ToString("C"),
                    product.IsSold ? "Evet" : "Hayır"
                );
                productsGrid.Rows[index].DefaultCellStyle.BackColor =
                    product.IsSold ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightBlue;
            }
        }

        private async Task UpdateUIAsync()
        {
            if (businessService == null) return;
            await Task.Run(() => Invoke((MethodInvoker)UpdateUI));
        }

        private async void ResetGameButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Oyunu sıfırlamak istediğinize emin misiniz?\nBu işlem tüm hayvanları, ürünleri ve nakit miktarını sıfırlayacak.",
                "Oyunu Sıfırla",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (businessService != null)
                {
                    // Business servis içindeki verileri sıfırla
                    businessService.Animals.Clear();
                    businessService.Products.Clear();
                    businessService.Cash.Amount = 1000m; // Başlangıç parası
                    
                    // Veritabanını da sıfırla
                    await ResetDatabaseAsync();
                    
                    // UI'yi güncelle
                    UpdateUI();
                    
                    MessageBox.Show("Oyun başarıyla sıfırlandı!", "Başarılı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async Task ResetDatabaseAsync()
        {
            if (dataAccess != null)
            {
                // Veritabanındaki tüm hayvanları sil
                var allAnimals = await dataAccess.GetAllAnimalsAsync();
                foreach (var animal in allAnimals)
                {
                    await dataAccess.DeleteAnimalAsync(animal.Id);
                }

                // Veritabanındaki tüm ürünleri sil
                var allProducts = await dataAccess.GetAllProductsAsync();
                foreach (var product in allProducts)
                {
                    await dataAccess.DeleteProductAsync(product.Id);
                }

                // Nakiti başlangıç değerine getir
                var cash = await dataAccess.GetCashAsync();
                cash.Amount = 1000m;
                await dataAccess.UpdateCashAsync(cash);
                
                // Business servisi yeniden başlat
                await businessService.InitializeAsync();
            }
        }

        private async void SimulationTimer_Tick(object? sender, EventArgs e)
        {
            if (businessService == null) return;
            try
            {
                await businessService.SimulateTickAsync();
                Invoke((MethodInvoker)UpdateUI);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Simülasyon adımı sırasında hata: {ex.Message}", "Simülasyon Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}