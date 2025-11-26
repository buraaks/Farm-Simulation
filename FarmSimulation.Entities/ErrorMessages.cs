namespace FarmSimulation.Entities
{
    public static class ErrorMessages
    {
        public const string DatabaseInitializationError = "Veritabanı başlatılırken bir hata oluştu: {0}";
        public const string DbContextNotInitialized = "Veritabanı bağlamı başlatılamadı.";
        public const string ServiceInitializationError = "Servisler başlatılırken bir hata oluştu: {0}";
        public const string SystemStarting = "Sistem henüz başlatılıyor. Lütfen bir süre bekleyin.";
        public const string AnimalPurchaseError = "Hayvan satın alınırken bir hata oluştu: {0}";
        public const string ProductSaleError = "Ürünler satılırken bir hata oluştu: {0}";
        public const string UISyncError = "Kullanıcı arayüzü güncellenirken bir hata oluştu: {0}";
        public const string ResetGameError = "Oyun sıfırlanırken bir hata oluştu: {0}";
        public const string SimulationTickError = "Simülasyon adımı sırasında bir hata oluştu: {0}";

        public const string AnimalNameRequired = "Lütfen bir hayvan adı girin.";
        public const string AnimalTypeRequired = "Lütfen bir hayvan türü seçin.";
        public const string InsufficientFunds = "Yetersiz bakiye! Bu hayvanı satın almak için {0} gerekiyor.";

        // Business Layer Exceptions
        public const string DataLoadError = "Veritabanından veri yüklenirken bir hata oluştu: {0}";
        public const string AddAnimalError = "Hayvan eklenirken bir hata oluştu: {0}";
        public const string UpdateAnimalError = "Hayvan güncellenirken bir hata oluştu: {0}";
        public const string DeleteAnimalError = "Hayvan silinirken bir hata oluştu: {0}";
        public const string AddProductError = "Ürün eklenirken bir hata oluştu: {0}";
        public const string UpdateProductError = "Ürün güncellenirken bir hata oluştu: {0}";
        public const string DeleteProductError = "Ürün silinirken bir hata oluştu: {0}";
        public const string SimulationError = "Simülasyon döngüsünde bir hata oluştu: {0}";
        public const string SaveChangesError = "Değişiklikler kaydedilirken bir hata oluştu: {0}";
        public const string CollectProductError = "Ürün toplanırken bir hata oluştu: {0}";
        public const string CollectAllProductsError = "Tüm ürünler toplanırken bir hata oluştu: {0}";
    }
}