# 🌾 Çiftlik Simülasyonu - Ürün Yönetim Sistemi

## 📋 Genel Bakış

Farm Simulation uygulamasına kapsamlı bir ürün yönetim sistemi eklenmiştir. Bu sistem, ürünlerin üretiminden satışına kadar tüm süreçleri yönetir ve detaylı raporlama sağlar.

## ✨ Yeni Özellikler

### 1️⃣ Gelişmiş Ürün Modeli
- **Tür**: Ürün türü (Yumurta, Süt, Yün, Keçi Sütü)
- **BirimFiyat**: Her ürünün birim fiyatı
- **ÜretimTarihi**: Ürünün üretildiği tarih ve saat
- **HayvanId**: Ürünü üreten hayvan bilgisi (opsiyonel)
- **Tutar**: Otomatik hesaplanan toplam değer (Miktar × BirimFiyat)

### 2️⃣ Satış Geçmişi Sistemi
- Tüm satışlar `ProductSalesHistory` tablosunda kayıt altına alınır
- Satış tarihi, miktar, fiyat ve toplam tutar bilgileri saklanır
- Satış istatistikleri ve raporlama yapılabilir

### 3️⃣ Akıllı Ürün Toplama
- Aynı türdeki ürünler otomatik olarak birleştirilir
- Her hayvan kendi ürününü üretir:
  - **Tavuk** → Yumurta (2.5₺)
  - **İnek** → Süt (10₺)
  - **Koyun** → Yün (15₺)
  - **Keçi** → Keçi Sütü (12₺)

### 4️⃣ Gelişmiş UI Özellikleri
- **Seçili Ürün Satışı**: Sadece seçtiğiniz ürünleri satabilirsiniz
- **Ürün İstatistikleri**: Başlık çubuğunda türlere göre ürün miktarları
- **Detaylı Kasa Bilgisi**: Para ve toplam ürün değeri ayrı ayrı gösterilir
- **Çoklu Seçim**: DataGridView'de birden fazla ürün seçebilirsiniz

### 5️⃣ İstatistiksel Raporlar
- Türe göre ürün sayıları
- Türe göre satış istatistikleri
- Toplam satış tutarı
- Toplam ürün değeri

## 🗄️ Veritabanı Değişiklikleri

### Product Tablosu Güncellemeleri
```sql
-- Yeni kolonlar
Tür NVARCHAR(50) NOT NULL
BirimFiyat DECIMAL(18,2) NOT NULL
ÜretimTarihi DATETIME NOT NULL DEFAULT GETDATE()
HayvanId INT NULL
Tutar AS (Miktar * BirimFiyat) PERSISTED  -- Computed column

-- Foreign Key
FK_Products_Animals (HayvanId → Animals.Id) ON DELETE SET NULL
```

### Yeni Tablo: ProductSalesHistory
```sql
CREATE TABLE ProductSalesHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ÜrünAdı NVARCHAR(100) NOT NULL,
    ÜrünTürü NVARCHAR(50) NOT NULL,
    SatılanMiktar INT NOT NULL,
    BirimFiyat DECIMAL(18,2) NOT NULL,
    ToplamTutar DECIMAL(18,2) NOT NULL,
    SatışTarihi DATETIME NOT NULL DEFAULT GETDATE(),
    HayvanId INT NULL
);
```

## 🚀 Kurulum ve Kullanım

### 1. SQL Script Çalıştırma
```bash
# SQL Server Management Studio'da şu dosyayı çalıştırın:
Farm Simulation/SQL_Scripts/02_Product_System_Update.sql
```

### 2. Uygulamayı Çalıştırma
```bash
cd "Farm Simulation/FarmSimulation.UI"
dotnet run
```

## 🎮 Kullanım Kılavuzu

### Ürün Toplama
1. **"Ürünleri Topla"** butonuna tıklayın
2. Tüm yaşayan hayvanlardan ürün toplanır
3. Aynı türdeki ürünler otomatik birleştirilir
4. Ölü hayvanlar otomatik olarak çiftlikten kaldırılır

### Ürün Satışı

#### Tümünü Sat
- **"Tümünü Sat"** butonuna tıklayın
- Tüm ürünler satılır ve satış geçmişine kaydedilir
- Kazanç kasaya eklenir

#### Seçilileri Sat
1. Ürün tablosundan satmak istediğiniz ürünleri seçin (Ctrl veya Shift ile çoklu seçim)
2. **"Seçilileri Sat"** butonuna tıklayın
3. Sadece seçili ürünler satılır

## 📊 Yeni API Metodları

### FarmService Metodları

```csharp
// Ürün Yönetimi
List<Product> GetProducts()
Dictionary<string, List<Product>> GetProductsByType()
Dictionary<string, int> GetProductStatistics()
decimal GetTotalProductValue()

// Satış İşlemleri
void SellAllProducts()
void SellSelectedProducts(List<int> productIds)

// Satış Geçmişi
List<ProductSalesHistory> GetSalesHistory(int topN = 50)
decimal GetTotalSalesAmount()
Dictionary<string, decimal> GetSalesStatisticsByType()
```

## 📁 Proje Yapısı

```
FarmSimulation.Data/
├── Models/
│   ├── Animals/
│   │   ├── Chicken.cs        (BirimFiyat: 2.5₺)
│   │   ├── Cow.cs            (BirimFiyat: 10₺)
│   │   ├── Sheep.cs          (BirimFiyat: 15₺)
│   │   └── Goat.cs           (BirimFiyat: 12₺)
│   ├── Product.cs            ✨ Güncellenmiş
│   ├── ProductSalesHistory.cs ✨ Yeni
│   └── FarmDbContext.cs      ✨ Güncellenmiş

FarmSimulation.Business/
└── Services/
    └── FarmService.cs        ✨ Geliştirilmiş

FarmSimulation.UI/
└── Forms/
    ├── MainForm.cs           ✨ Yeni özellikler
    └── MainForm.Designer.cs  ✨ Yeni butonlar
```

## 🎯 Özellik Detayları

### Computed Column (Tutar)
Tutar artık veritabanı seviyesinde hesaplanır:
```csharp
public decimal Tutar => Miktar * BirimFiyat;
```

SQL tarafında:
```sql
Tutar AS (Miktar * BirimFiyat) PERSISTED
```

### Otomatik Gruplama
Aynı türdeki ürünler toplanırken birleştirilir:
```csharp
var existingProduct = _context.Products
    .FirstOrDefault(p => p.Tür == product.Tür && p.HayvanId == null);

if (existingProduct != null)
{
    existingProduct.Miktar += product.Miktar;
}
```

### Satış Geçmişi Kaydı
Her satış işlemi otomatik kaydedilir:
```csharp
var salesHistory = new ProductSalesHistory
{
    ÜrünAdı = product.Ad,
    ÜrünTürü = product.Tür,
    SatılanMiktar = product.Miktar,
    BirimFiyat = product.BirimFiyat,
    ToplamTutar = product.Tutar,
    SatışTarihi = DateTime.Now
};
```

## 🔧 Teknik Notlar

### Entity Framework Yapılandırması
```csharp
// Product için computed column
modelBuilder.Entity<Product>()
    .Property(p => p.Tutar)
    .HasComputedColumnSql("[Miktar] * [BirimFiyat]");

// Foreign Key ilişkisi
modelBuilder.Entity<Product>()
    .HasOne(p => p.Hayvan)
    .WithMany()
    .HasForeignKey(p => p.HayvanId)
    .OnDelete(DeleteBehavior.SetNull);
```

### UI Güncellemeleri
```csharp
// Detaylı kasa bilgisi
lblCash.Text = $"Para: {cash:F2} ₺ | Ürün Değeri: {totalProductValue:F2} ₺";

// Ürün istatistikleri
var statsText = string.Join(", ", productStats.Select(p => $"{p.Key}: {p.Value}"));
this.Text = $"Farm Simulation - Yaşayan: {aliveCount} | Ölü: {deadCount} | {statsText}";
```

## 📈 Gelecek Geliştirmeler

- [ ] Ürün filtreleme (tarih, tür, fiyat aralığı)
- [ ] Grafik ve görselleştirme
- [ ] Excel'e aktarma
- [ ] Otomatik satış sistemi
- [ ] Fiyat dinamikleri (pazar fiyatları)
- [ ] Stok uyarıları
- [ ] Gelişmiş raporlar

## 🐛 Bilinen Sorunlar

Şu anda bilinen bir sorun bulunmamaktadır. ✅

## 📞 Destek

Herhangi bir sorun veya öneri için lütfen Issue açın.

---

**Versiyon**: 2.0  
**Tarih**: 2025-01-21  
**Geliştirici**: Farm Simulation Team
