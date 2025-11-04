# Tarım Simülasyonu

C# ve Windows Forms kullanılarak oluşturulmuş kapsamlı bir çiftlik yönetim simülasyonu uygulaması. Uygulama, kullanıcıların hayvanları yönetmelerine, ürünleri toplamalarına ve simüle edilmiş bir ortamda çiftlik ekonomisini sürdürmelerine olanak tanır.

## Özellikler

- **Hayvan Yönetimi**: Çiftliğe hayvan ekleme, izleme ve yönetme (tavuklar, inekler, koyunlar)
- **Ürün Toplama**: Hayvanların üretim döngülerine göre ürünlerin toplanması
- **Ekonomik Simülasyon**: Nakit akışı takibi, ürün satışı ve satın alma işlemleri
- **Yaşam Döngüsü Yönetimi**: Hayvanlar zamanla yaşlanır ve sonunda ölür
- **Veri Kalıcılığı**: Tüm çiftlik verileri yerel bir veritabanında saklanır
- **Gerçek Zamanlı Simülasyon**: Sürekli simülasyon güncellemeleri hayvan durumlarını ve ürün üretimini yönetir

## Kullanılan Teknolojiler

- **Dil**: C# 9.0+
- **Çerçeve**: .NET 6.0+
- **Arayüz**: Windows Forms
- **Veri Erişimi**: Entity Framework Core
- **Veritabanı**: SQL Server LocalDB

## Mimari

Uygulama katmanlı mimari desenini takip eder:

- **Arayüz Katmanı**: Kullanıcı etkileşimi için Windows Forms uygulaması
- **İş Katmanı**: Temel iş mantığını ve simülasyon kurallarını içerir
- **Veri Katmanı**: Veri erişim ve kalıcılık işlemlerini gerçekleştirir
- **Varlıklar**: Hayvanları, ürünleri ve nakiti temsil eden veri modelleri

## Başlangıç

### Gereksinimler

- .NET 6.0 SDK veya üzeri
- Visual Studio 2022 veya üzeri (önerilen)
- SQL Server LocalDB (Visual Studio ile birlikte gelir)

## Nasıl Oynanır

1. **Hayvan Ekleme**: Çiftliğinize hayvan satın alıp ekleyin (tavuklar, inekler, koyunlar)
2. **Üretim Bekleme**: Hayvanlar zamanla ürünler üretir
3. **Ürün Toplama**: Ürünler hazır olduğunda toplayın (ilerleme %100'e ulaştığında)
4. **Ürün Satma**: Kazanç elde etmek için ürünlerinizi satın
5. **Finans Yönetimi**: Daha fazla hayvan satın almak için kazançlarınızı kullanın
6. **Yaşam Döngüsünü Takip Etme**: Hayvanların yaşlanmasını ve ömürlerini takip edin

### Hayvan Türleri ve Özellikleri

- **Tavuk**:
  - Yaşam süresi: 15 oyun günü
  - Üretim döngüsü: 15 saniyede bir (yumurta üretir)
  - Satın alma maliyeti: 10 coin
  - Ürün değeri: Yumurta başına 1 coin

- **İnek**:
  - Yaşam süresi: 20 oyun günü
  - Üretim döngüsü: 45 saniyede bir (süt üretir)
  - Satın alma maliyeti: 500 coin
  - Ürün değeri: Süt başına 5 coin

- **Koyun**:
  - Yaşam süresi: 25 oyun günü
  - Üretim döngüsü: 60 saniyede bir (yün üretir)
  - Satın alma maliyeti: 150 coin
  - Ürün değeri: Yün başına 8 coin

## Oyun Mekaniği

- Oyun zamanı hızlandırılmıştır: Gerçek zamanın 30 saniyesi 1 oyun gününe eşittir
- Hayvanlar sürekli yaşlanır ve maksimum yaşa ulaştıklarında ölürler
- Ürünler hayvan üretim döngülerine göre birikir
- Nakit merkezi olarak yönetilir ve ürünler satıldığında artar
- Hayvanlara rastgele cinsiyet atanır (erkek/dişi)

## Proje Yapısı

```
Farm-Simulation/
├── FarmSimulation.sln
├── FarmSimulation.Business/     # İş mantığı ve simülasyon servisleri
│   ├── Services/
│   └── Data/
├── FarmSimulation.Data/         # Veri erişim katmanı
│   ├── DatabaseService.cs
│   └── FarmDataAccess.cs
├── FarmSimulation.Entities/     # Veri modelleri
│   ├── Animal.cs
│   ├── Product.cs
│   └── Cash.cs
└── FarmSimulation.UI/           # Windows Forms arayüzü
    ├── Forms/
    ├── Program.cs
    └── appsettings.json
```
