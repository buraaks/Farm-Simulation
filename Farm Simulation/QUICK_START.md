# 🚀 Hızlı Başlangıç Kılavuzu - Ürün Sistemi

## ⚡ 3 Adımda Kurulum

### 1️⃣ Veritabanını Güncelle
SQL Server Management Studio'yu açın ve şu komutu çalıştırın:
```sql
USE FarmSimulation;
GO
```

Ardından `SQL_Scripts/02_Product_System_Update.sql` dosyasını çalıştırın.

### 2️⃣ Uygulamayı Çalıştır
```bash
cd "c:\Users\burak\OneDrive\Desktop\Farm-Simulation\Farm Simulation\FarmSimulation.UI"
dotnet run
```

### 3️⃣ Test Et! 🎉
1. Hayvan ekle (örn: 2 Tavuk, 1 İnek)
2. "Ürünleri Topla" butonuna bas
3. Ürünleri gör ve sat!

## 🎯 Temel Kullanım

### Hayvan Ekleme
```
Tür: Tavuk ↓
Yaş: 2
Cinsiyet: Kız ↓
[Ekle] → Tıkla
```

### Ürün Toplama
```
[Ürünleri Topla] → Tıkla
→ Her hayvan ürününü üretir
→ Aynı türler birleşir
→ Ölü hayvanlar temizlenir
```

### Ürün Satışı

**Tümünü Sat:**
```
[Tümünü Sat] → Tıkla
→ Tüm ürünler satılır
→ Para kasaya eklenir
```

**Seçilileri Sat:**
```
1. Ürün tablosundan satmak istediğin ürünleri seç
2. [Seçilileri Sat] → Tıkla
→ Sadece seçtiklerin satılır
```

## 📊 Ekran Bilgileri

### Üst Başlık
```
Farm Simulation - Yaşayan: 5 | Ölü: 1 | Yumurta: 10, Süt: 3
```

### Kasa Bilgisi
```
Para: 125.50 ₺ | Ürün Değeri: 45.00 ₺
```

## 🔥 İpuçları

1. **10 saniyede bir** hayvanların yaşı otomatik artar
2. **Aynı türdeki ürünler** otomatik birleşir
3. **Ölü hayvanlar** ürün toplanırken otomatik kaldırılır
4. **Ctrl tuşu** ile çoklu ürün seçimi yapabilirsin
5. **Computed column** sayesinde Tutar otomatik hesaplanır

## 🎨 Hayvan Türleri ve Ürünleri

| Hayvan | Ürün | Birim Fiyat | Max Yaş |
|--------|------|-------------|---------|
| 🐔 Tavuk | Yumurta | 2.5₺ | 15 |
| 🐄 İnek | Süt | 10₺ | 25 |
| 🐑 Koyun | Yün | 15₺ | 20 |
| 🐐 Keçi | Keçi Sütü | 12₺ | 18 |

## ⚠️ Önemli Notlar

- Veritabanı bağlantısı: `Server=JEFT;Database=FarmSimulation`
- .NET 9.0 gereklidir
- Windows Forms uygulamasıdır
- SQL Server kullanılmaktadır

## 🐞 Sorun Giderme

### Derleme Hatası
```bash
cd FarmSimulation.UI
dotnet clean
dotnet build
```

### Veritabanı Hatası
```sql
-- Primary Key kontrolü
SELECT * FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID('Animals');

-- Foreign Key kontrolü
SELECT * FROM sys.foreign_keys WHERE name = 'FK_Products_Animals';
```

### Ürün Görünmüyor
1. Hayvanların yaşının maksimum yaştan düşük olduğundan emin ol
2. Lifecycle kayıtlarının oluştuğunu kontrol et
3. `RefreshUI()` metodunun çağrıldığını doğrula

## 📞 Hızlı Yardım

**Hata:** `Product.Tutar` özelliğine atama yapılamaz
**Çözüm:** Tutar artık computed property, BirimFiyat kullan

**Hata:** Foreign Key hatası
**Çözüm:** Animals tablosunda Primary Key olduğundan emin ol

**Hata:** Ürünler birleşmiyor
**Çözüm:** HayvanId null olmalı gruplu ürünlerde

---

**Başarılar! 🌾**
