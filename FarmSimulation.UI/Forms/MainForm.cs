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
            InitializeServices();
            InitializeTimer();
        }

        private void InitializeDatabase()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Application.StartupPath)  // Use application startup path instead of current directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<FarmDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            dbContext = new FarmDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();
        }

        private async void InitializeServices()
        {
            dataAccess = new FarmDataAccess(dbContext);
            businessService = new FarmBusinessService(dataAccess);
            
            // Initialize the business service with data from database
            await businessService.InitializeAsync();
            // Update UI on the UI thread
            UpdateUI();
        }

        private void InitializeTimer()
        {
            simulationTimer = new System.Windows.Forms.Timer();
            simulationTimer.Interval = 1000; // 1 second interval
            simulationTimer.Tick += SimulationTimer_Tick;
            simulationTimer.Start(); // Start the simulation automatically
        }

        private async void BuyAnimalButton_Click(object? sender, EventArgs e)
        {
            var buyForm = new BuyAnimalForm(businessService);
            var result = buyForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                await UpdateUIAsync();
            }
        }

        private async void CollectSelectedAnimalProductsButton_Click(object? sender, EventArgs e)
        {
            if (animalsGrid.SelectedRows.Count > 0)
            {
                var selectedRow = animalsGrid.SelectedRows[0];
                var animalId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                
                // Find the animal in the business service
                var animal = businessService.Animals.FirstOrDefault(a => a.Id == animalId);
                if (animal != null)
                {
                    // Collect product from the selected animal
                    var product = await businessService.CollectProductFromAnimalAsync(animal);
                    if (product != null && product.Name != "Unknown Product") // Check if product was created properly
                    {
                        MessageBox.Show($"{product.Name} collected from {animal.Name}!", "Product Collected", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Update UI
                        await UpdateUIAsync();
                    }
                    else
                    {
                        MessageBox.Show("Could not collect product from this animal.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Selected animal not found.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an animal first.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        

        private async void SellProductsButton_Click(object sender, EventArgs e)
        {
            decimal earnings = await businessService.SellProductsAsync();
            MessageBox.Show($"Sold products for {earnings:C}!", "Products Sold", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await UpdateUIAsync();
        }

        private async void DeleteSoldProductsButton_Click(object sender, EventArgs e)
        {
            int deletedCount = await businessService.DeleteSoldProductsAsync();
            
            if (deletedCount > 0)
            {
                MessageBox.Show($"{deletedCount} sold products deleted!", "Products Deleted", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            else
            {
                MessageBox.Show("No sold products found to delete.", "No Products", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void CollectAllProductsButton_Click(object? sender, EventArgs e)
        {
            // Collect products from all animals that can produce
            int collectedCount = 0;
            foreach (var animal in businessService.Animals.Where(a => a.IsAlive && a.CanProduce).ToList())
            {
                var product = await businessService.CollectProductFromAnimalAsync(animal);
                if (product != null)
                {
                    collectedCount++;
                }
            }
            
            if (collectedCount > 0)
            {
                MessageBox.Show($"{collectedCount} products collected!", "Products Collected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            else
            {
                MessageBox.Show("No products could be collected.", "No Products", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void SellSelectedProductButton_Click(object? sender, EventArgs e)
        {
            if (productsGrid.SelectedRows.Count > 0)
            {
                var selectedRow = productsGrid.SelectedRows[0];
                var productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                
                // Find the product in the business service using the ID
                var product = businessService.Products.FirstOrDefault(p => p.Id == productId);
                
                if (product != null)
                {
                    if (!product.IsSold)
                    {
                        // Mark product as sold
                        product.IsSold = true;
                        businessService.Cash.Amount += product.Price * product.Quantity;
                        
                        // Update product in database
                        await businessService.dataAccess.UpdateProductAsync(product);
                        
                        MessageBox.Show($"{product.Name} sold for {product.Price * product.Quantity:C}!", "Product Sold", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await UpdateUIAsync();
                    }
                    else
                    {
                        MessageBox.Show("This product is already sold.", "Already Sold", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Selected product not found.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a product first.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void SellAllProductsButton_Click(object sender, EventArgs e)
        {
            decimal totalEarnings = 0m;
            int soldCount = 0;
            
            // Sell all unsold products
            foreach (var product in businessService.Products.Where(p => !p.IsSold).ToList())
            {
                product.IsSold = true;
                totalEarnings += product.Price * product.Quantity;
                soldCount++;
                
                // Update product in database
                await businessService.dataAccess.UpdateProductAsync(product);
            }
            
            if (soldCount > 0)
            {
                businessService.Cash.Amount += totalEarnings;
                MessageBox.Show($"{soldCount} products sold for {totalEarnings:C}!", "Products Sold", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdateUIAsync();
            }
            else
            {
                MessageBox.Show("No unsold products found.", "No Products", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void UpdateUI()
        {
            // Update cash display
            cashLabel.Text = $"Cash: {businessService.Cash.Amount:C}";

            // Update animals grid
            animalsGrid.Rows.Clear();
            foreach (var animal in businessService.Animals)
            {
                int index = animalsGrid.Rows.Add(
                    animal.Id,
                    animal.Name,
                    animal.Age,
                    animal.Type,
                    animal.IsAlive ? "Yes" : "No",
                    animal.ProductProductionProgress + "%"
                );

                // Change row color based on animal status
                if (!animal.IsAlive)
                {
                    animalsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                }
            }

            // Update products grid - add an ID column to help with identification
            productsGrid.Rows.Clear();
            foreach (var product in businessService.Products)
            {
                int index = productsGrid.Rows.Add(
                    product.Id, // Add ID column
                    product.ProductType,
                    product.Quantity,
                    product.Price.ToString("C"),
                    product.IsSold ? "Yes" : "No"
                );

                // Change row color based on product status
                if (product.IsSold)
                {
                    productsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    productsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }

        private async Task UpdateUIAsync()
        {
            // Update cash display
            cashLabel.Text = $"Cash: {businessService.Cash.Amount:C}";

            // Update animals grid
            animalsGrid.Rows.Clear();
            foreach (var animal in businessService.Animals)
            {
                int index = animalsGrid.Rows.Add(
                    animal.Id,
                    animal.Name,
                    animal.Age,
                    animal.Type,
                    animal.IsAlive ? "Yes" : "No",
                    animal.ProductProductionProgress + "%"
                );

                // Change row color based on animal status
                if (!animal.IsAlive)
                {
                    animalsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                }
            }

            // Update products grid - add an ID column to help with identification
            productsGrid.Rows.Clear();
            foreach (var product in businessService.Products)
            {
                int index = productsGrid.Rows.Add(
                    product.Id, // Add ID column
                    product.ProductType,
                    product.Quantity,
                    product.Price.ToString("C"),
                    product.IsSold ? "Yes" : "No"
                );

                // Change row color based on product status
                if (product.IsSold)
                {
                    productsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    productsGrid.Rows[index].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }

        private async void SimulationTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                await businessService.SimulateTickAsync();
                this.Invoke((MethodInvoker)delegate {
                    UpdateUI();
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                MessageBox.Show($"Error during simulation tick: {ex.Message}", "Simulation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}