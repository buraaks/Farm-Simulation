# 🌾 Çiftlik Simülasyonu

**.NET 9** tabanlı masaüstü çiftlik yönetim simülasyonu uygulaması.

## 📋 Genel Bakış

Bu proje, hayvan yetiştirme, ürün toplama ve satış işlemlerini simüle eden bir çiftlik yönetim uygulamasıdır. Windows Forms kullanılarak geliştirilmiştir.

## ✨ Özellikler

### 🐔 Hayvan Yönetimi
- **4 Hayvan Türü**: Tavuk, İnek, Koyun, Keçi
- **Yaşlanma Sistemi**: Her 10 saniyede bir otomatik yaşlanma
- **Yaşam Süresi**: Her hayvan türünün kendine özgü maksimum yaşı
  - Tavuk: 15 yaş
  - İnek: 25 yaş
  - Koyun: 20 yaş
  - Keçi: 18 yaş
- **Çoklu Seçim**: Birden fazla hayvanı seçerek silme
- **Otomatik Temizleme**: Ölü hayvanlar ürün toplarken otomatik kaldırılır

### 🥚 Ürün Sistemi
- **Otomatik Gruplama**: Aynı türdeki ürünler tek satırda toplanır
- **Ürün Türleri**:
  - Yumurta (Tavuk) - 2.5₺
  - Süt (İnek) - 10₺
  - Yün (Koyun) - 15₺
  - Keçi Sütü (Keçi) - 12₺
- **Seçili Satış**: İstediğiniz ürünleri seçerek satabilirsiniz
- **Toplu Satış**: Tüm ürünleri tek seferde satma

### 💰 Kasa Yönetimi
- Ürün satışlarından gelir
- Para sıfırlama özelliği
- Toplam ürün değeri gösterimi

## 🗂️ Proje Yapısı

```
Farm-Simulation/
├── Farm Simulation/
│   ├── FarmSimulation.UI/          # Windows Forms kullanıcı arayüzü
│   ├── FarmSimulation.Business/    # İş mantığı katmanı
│   └── FarmSimulation.Data/        # Veri modelleri ve Entity Framework
│       └── Models/
│           ├── Animals/            # Hayvan sınıfları
│           │   ├── Chicken.cs
│           │   ├── Cow.cs
│           │   ├── Sheep.cs
│           │   └── Goat.cs
│           ├── AnimalBase.cs       # Temel hayvan sınıfı
│           ├── AnimalLifecycle.cs  # Yaşam döngüsü
│           ├── Product.cs          # Ürün modeli
│           ├── Cash.cs            # Kasa
│           └── FarmDbContext.cs   # Veritabanı bağlamı
└── README.md
```

## 🛠️ Teknolojiler

- **.NET 9.0**
- **C# 13**
- **Windows Forms** (WinForms)
- **Entity Framework Core 9.0.10**
- **SQL Server**

## 🗄️ Veritabanı Yapısı

### Tablolar

1. **Animals** - Hayvan bilgileri
   - Id, Ad, Yaş, Cinsiyet

2. **AnimalLifecycle** - Yaşam döngüsü
   - AnimalId, MaksimumYaş, SonYaşArtışZamanı, Ölü

3. **Products** - Ürün envanteri
   - Id, Ad, Miktar, Fiyat, ToplamTutar, HayvanId

4. **Cash** - Kasa durumu
   - Id, Tutar

### Bağlantı Dizesi
```
Server=JEFT;Database=FarmSimulation;Trusted_Connection=True;TrustServerCertificate=True;
```

## 🚀 Kurulum

### Gereksinimler
- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 veya üzeri (opsiyonel)

### Adımlar

1. **Projeyi Klonlayın**
   ```bash
   git clone <repository-url>
   cd Farm-Simulation
   ```

2. **Veritabanını Oluşturun**
   - SQL Server Management Studio'yu açın
   - "FarmSimulation" adında yeni bir veritabanı oluşturun
   - Tabloları manuel olarak oluşturun (SQL scriptler kullanıcı tarafından yönetilir)

3. **Projeyi Derleyin**
   ```bash
   cd "Farm Simulation/FarmSimulation.UI"
   dotnet build
   ```

4. **Uygulamayı Çalıştırın**
   ```bash
   dotnet run
   ```

## 🎮 Kullanım

### Hayvan Ekleme
1. Tür seçin (Tavuk, İnek, Koyun, Keçi)
2. Yaş girin
3. Cinsiyet seçin
4. "Ekle" butonuna basın

### Ürün Toplama
1. "Ürünleri Topla" butonuna basın
2. Yaşayan hayvanlar ürün üretir
3. Aynı türdeki ürünler otomatik birleştirilir
4. Ölü hayvanlar otomatik temizlenir

### Ürün Satma
- **Tümünü Sat**: Tüm ürünleri sat, parayı kasaya ekle
- **Seçilileri Sat**: Sadece seçtiğiniz ürünleri sat (Ctrl/Shift ile çoklu seçim)

### Hayvan Silme
1. Silmek istediğiniz hayvanları seçin (Ctrl/Shift ile çoklu seçim)
2. "Seçili Hayvanları Sil" butonuna basın
3. Onaylayın

## 📊 Özellik Detayları

### Otomatik Yaşlanma
- Her 10 saniyede bir çalışan timer
- Tüm hayvanların yaşı otomatik artar
- Maksimum yaşa ulaşan hayvanlar "ölü" olarak işaretlenir

### Ürün Gruplama
- Aynı türdeki tüm ürünler tek satırda
- HayvanId bağımsız gruplama
- Veritabanındaki mevcut kayıtlar otomatik birleştirilir

### Veri Bütünlüğü
- Foreign Key ilişkileri
- Cascade Delete (Hayvan silinince lifecycle kaydı da silinir)
- Null-safe navigation properties

## 📚 Ek Dökümanlar

- **[URUN_SISTEMI.md](Farm%20Simulation/URUN_SISTEMI.md)** - Detaylı ürün sistemi dokümantasyonu
- **[HIZLI_BASLANGIC.md](Farm%20Simulation/HIZLI_BASLANGIC.md)** - Hızlı başlangıç kılavuzu

## 🔧 Geliştirme

### Mimari Prensipler
- **Katmanlı Mimari**: UI, Business, Data katmanları ayrı
- **Separation of Concerns**: Her katmanın kendi sorumluluğu
- **Entity Framework**: Code-First yaklaşımı
- **SOLID Prensipleri**: Özellikle Single Responsibility

### Yeni Hayvan Türü Ekleme
1. `FarmSimulation.Data/Models/Animals/` altında yeni sınıf oluştur
2. `AnimalBase`'den türet
3. `Produce()` metodunu override et
4. `FarmDbContext`'te discriminator ekle

## 🐛 Bilinen Sorunlar

Şu anda bilinen bir sorun bulunmamaktadır.

## 📞 İletişim

Sorularınız için Issue açabilirsiniz.

---

**Versiyon**: 1.0  
**Son Güncelleme**: 2025-01-21  
**Lisans**: MIT
