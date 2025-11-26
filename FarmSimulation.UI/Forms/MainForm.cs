using FarmSimulation.Business.Data;
using FarmSimulation.Business.Services;
using FarmSimulation.Data;
using FarmSimulation.Entities;
using FarmSimulation.UI.CustomControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
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
        private bool _isSimulating = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeTimer();

            this.Load += MainForm_Load;
        }

        private void InitializeGridColumns()
        {
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            await InitializeServices();
        }

        private string GetFullErrorMessage(Exception ex)
        {
            return ex.ToString(); // For debugging
        }

        private void InitializeDatabase()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Application.StartupPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var configuration = builder.Build();

                var optionsBuilder = new DbContextOptionsBuilder<FarmDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });

                dbContext = new FarmDbContext(optionsBuilder.Options);
            
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Critical Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private async Task InitializeServices()
        {
            if (dbContext == null)
            {
                MessageBox.Show(ErrorMessages.DbContextNotInitialized, "Critical Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            try
            {
                buyAnimalButton.Enabled = false;

                dataAccess = new FarmDataAccess(dbContext);
                businessService = new FarmBusinessService(dataAccess);
                await businessService.InitializeAsync();

                UpdateUI();
                buyAnimalButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeTimer()
        {
            simulationTimer.Tick += SimulationTimer_Tick;
            simulationTimer.Start();
        }

        private async void BuyAnimalButton_Click(object? sender, EventArgs e)
        {
            if (businessService == null)
            {
                MessageBox.Show(ErrorMessages.SystemStarting,
                    "Initialization", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var buyForm = new BuyAnimalForm(businessService);
                var result = buyForm.ShowDialog();

                if (result == DialogResult.OK)
                    await UpdateUIAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SellProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;
            
            try
            {
                decimal earnings = await businessService.SellProductsAsync();
                MessageBox.Show($"Ürünler {earnings:C} karşılığında satıldı!", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SellAllProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;

            try
            {
                decimal earnings = await businessService.SellProductsAsync();
                MessageBox.Show($"Tüm ürünler {earnings:C} karşılığında satıldı!", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUI()
        {
            if (businessService == null) return;
            
            try
            {
                cashLabel.Text = $"Nakit: {businessService.Cash.Amount:C}";

                animalsGrid.Rows.Clear();
                foreach (var animal in businessService.Animals)
                {
                    animalsGrid.Rows.Add(
                        animal.Id,
                        animal.Name,
                        animal.Age,
                        animal.Type,
                        (int)animal.ProductProductionProgress
                    );
                }

                productsGrid.Rows.Clear();
                
                var groupedProducts = businessService.Products
                    .GroupBy(p => p.ProductType)
                    .Select(g => new
                    {
                        ProductType = g.Key,
                        Quantity = g.Sum(p => p.Quantity),
                        Price = g.First().Price, 
                    });
                
                foreach (var product in groupedProducts)
                {
                    productsGrid.Rows.Add(
                        product.ProductType,
                        product.Quantity,
                        product.Price.ToString("C")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateUIAsync()
        {
            if (businessService == null) return;
            await Task.Run(() => 
            {
                if (InvokeRequired)
                    Invoke((MethodInvoker)UpdateUI);
                else
                    UpdateUI();
            });
        }

        private async void ResetGameButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Oyunu sıfırlamak istediğinizden emin misiniz?\nBu işlem tüm hayvanları, ürünleri ve nakiti sıfırlayacaktır.",
                "Oyunu Sıfırla",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (businessService != null)
                    {
                        businessService.Animals.Clear();
                        businessService.Products.Clear();
                        businessService.Cash.Amount = GameSettings.InitialCash;
                        businessService.ClearTimeAccumulator();

                        await ResetDatabaseAsync();
                        UpdateUI();

                        MessageBox.Show("Oyun başarıyla sıfırlandı!", "Başarılı",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(GetFullErrorMessage(ex), "Hata", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task ResetDatabaseAsync()
        {
            if (dataAccess == null) return;
            
            try
            {
                var allAnimals = await dataAccess.GetAllAnimalsAsync();
                foreach (var animal in allAnimals)
                {
                    await dataAccess.DeleteAnimalAsync(animal.Id);
                }

                var allProducts = await dataAccess.GetAllProductsAsync();
                foreach (var product in allProducts)
                {
                    await dataAccess.DeleteProductAsync(product.Id);
                }

                var cash = await dataAccess.GetCashAsync();
                cash.Amount = GameSettings.InitialCash;
                dataAccess.UpdateCash(cash);
                await dataAccess.SaveChangesAsync();

                await businessService.InitializeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ErrorMessages.ResetGameError, GetFullErrorMessage(ex)), ex);
            }
        }

        private async void SimulationTimer_Tick(object? sender, EventArgs e)
        {
            if (businessService == null) return;
            
            if (_isSimulating) return;
            _isSimulating = true;

            try
            {
                await businessService.SimulateTickAsync();
                
                if (InvokeRequired)
                    Invoke((MethodInvoker)UpdateUI);
                else
                    UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetFullErrorMessage(ex), "Simülasyon Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _isSimulating = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            simulationTimer?.Stop();
            simulationTimer?.Dispose();
            dbContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
