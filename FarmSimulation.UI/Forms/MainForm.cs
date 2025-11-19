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
            InitializeGridColumns();
            InitializeTimer();

            this.Load += MainForm_Load;
        }

        private void InitializeGridColumns()
        {
            sellProductsButton.Location = new Point(650, 220);
            deleteSoldProductsButton.Location = new Point(650, 270);
            resetGameButton.Location = new Point(650, 320);

            animalsGrid.Columns.Clear();
            
            animalsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", ReadOnly = true, FillWeight = 30 });
            animalsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AnimalName", HeaderText = "Name", ReadOnly = true, FillWeight = 70 });
            animalsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Age", HeaderText = "Age", ReadOnly = true, FillWeight = 30 });
            animalsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Type", ReadOnly = true, FillWeight = 60 });
            animalsGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "IsAlive", HeaderText = "Is Alive", ReadOnly = true, FillWeight = 40 });
            
            var progressColumn = new DataGridViewProgressBarColumn();
            progressColumn.Name = "ProductionProgress";
            progressColumn.HeaderText = "Production Progress";
            progressColumn.ReadOnly = true;
            progressColumn.FillWeight = 100;
            animalsGrid.Columns.Add(progressColumn);
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            await InitializeServices();
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
                
                // Veritabanının mevcut modele uyması için silinip yeniden oluşturulduğundan emin ol.
                // Bu işlem geliştirme sürecinde faydalıdır ancak her başlatmada tüm verileri siler.
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}", "Critical Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private async Task InitializeServices()
        {
            if (dbContext == null)
            {
                MessageBox.Show("Database context could not be initialized.", "Critical Error", 
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
                MessageBox.Show($"Error while initializing services: {ex.Message}", "Error", 
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
                MessageBox.Show("The system is still starting. Please wait a moment.",
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
                MessageBox.Show($"Error while buying animal: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SellProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;
            
            try
            {
                decimal earnings = await businessService.SellProductsAsync();
                MessageBox.Show($"Products sold for {earnings:C}!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while selling products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeleteSoldProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;
            
            try
            {
                int deletedCount = await businessService.DeleteSoldProductsAsync();
                MessageBox.Show($"{deletedCount} sold products were deleted!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while deleting products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void SellAllProductsButton_Click(object sender, EventArgs e)
        {
            if (businessService == null) return;

            try
            {
                decimal earnings = await businessService.SellProductsAsync();
                MessageBox.Show($"All products sold for {earnings:C}!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while selling products: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUI()
        {
            if (businessService == null) return;
            
            try
            {
                cashLabel.Text = $"Cash: {businessService.Cash.Amount:C}";

                animalsGrid.Rows.Clear();
                foreach (var animal in businessService.Animals)
                {
                    int index = animalsGrid.Rows.Add(
                        animal.Id,
                        animal.Name,
                        animal.Age,
                        animal.Type,
                        animal.IsAlive ? "Yes" : "No",
                        (int)animal.ProductProductionProgress
                    );
                    if (!animal.IsAlive)
                        animalsGrid.Rows[index].DefaultCellStyle.BackColor = Color.LightCoral;
                }

                productsGrid.Rows.Clear();
                foreach (var product in businessService.Products)
                {
                    int index = productsGrid.Rows.Add(
                        product.Id,
                        product.ProductType,
                        product.Quantity,
                        product.Price.ToString("C"),
                        product.IsSold ? "Yes" : "No"
                    );
                    productsGrid.Rows[index].DefaultCellStyle.BackColor =
                        product.IsSold ? Color.LightGreen : Color.LightBlue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating UI: {ex.Message}", "Error", 
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
                "Are you sure you want to reset the game?\nThis action will reset all animals, products, and cash.",
                "Reset Game",
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

                        MessageBox.Show("Game has been reset successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while resetting game: {ex.Message}", "Error", 
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
                throw new Exception($"Error while resetting database: {ex.Message}", ex);
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
                MessageBox.Show($"Error during simulation step: {ex.Message}", "Simulation Error",
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
