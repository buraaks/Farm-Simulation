-- ======================================
-- Ürün Sistemi Veritabanı Güncellemeleri
-- ======================================

USE FarmSimulation;
GO

-- 1. Product tablosunu güncelle
-- Gereksiz kolonları kaldır
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'Tür' AND object_id = OBJECT_ID('Products'))
BEGIN
    ALTER TABLE Products DROP COLUMN Tür;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'BirimFiyat' AND object_id = OBJECT_ID('Products'))
BEGIN
    ALTER TABLE Products DROP COLUMN BirimFiyat;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'Tutar' AND object_id = OBJECT_ID('Products'))
BEGIN
    ALTER TABLE Products DROP COLUMN Tutar;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'ÜretimTarihi' AND object_id = OBJECT_ID('Products'))
BEGIN
    ALTER TABLE Products DROP COLUMN ÜretimTarihi;
END
GO

-- Fiyat kolonu ekle (eğer yoksa)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = 'Fiyat' AND object_id = OBJECT_ID('Products'))
BEGIN
    ALTER TABLE Products ADD Fiyat DECIMAL(18,2) NOT NULL DEFAULT 0;
END
GO

-- HayvanId kolonu ekle (nullable - Foreign Key)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE name = 'HayvanId' AND object_id = OBJECT_ID('Products'))
BEGIN
    ALTER TABLE Products ADD HayvanId INT NULL;
END
GO

-- Foreign Key ekle (eğer yoksa)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Products_Animals')
BEGIN
    ALTER TABLE Products 
    ADD CONSTRAINT FK_Products_Animals 
    FOREIGN KEY (HayvanId) REFERENCES Animals(Id) 
    ON DELETE SET NULL;
END
GO

-- 2. ProductSalesHistory tablosu oluştur
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ProductSalesHistory')
BEGIN
    CREATE TABLE ProductSalesHistory (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ÜrünAdı NVARCHAR(100) NOT NULL,
        SatılanMiktar INT NOT NULL,
        Fiyat DECIMAL(18,2) NOT NULL,
        ToplamTutar DECIMAL(18,2) NOT NULL,
        SatışTarihi DATETIME NOT NULL DEFAULT GETDATE(),
        HayvanId INT NULL
    );
END
GO

-- ProductSalesHistory'den gereksiz kolon kaldır
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'ÜrünTürü' AND object_id = OBJECT_ID('ProductSalesHistory'))
BEGIN
    ALTER TABLE ProductSalesHistory DROP COLUMN ÜrünTürü;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'BirimFiyat' AND object_id = OBJECT_ID('ProductSalesHistory'))
BEGIN
    ALTER TABLE ProductSalesHistory DROP COLUMN BirimFiyat;
END
GO

-- 3. Mevcut ürünleri güncelle (eğer varsa)
UPDATE Products 
SET Fiyat = 
    CASE 
        WHEN Ad = 'Yumurta' THEN 2.5
        WHEN Ad = 'Süt' THEN 10.0
        WHEN Ad = 'Yün' THEN 15.0
        WHEN Ad = 'Keçi Sütü' THEN 12.0
        ELSE 0
    END
WHERE Fiyat = 0;
GO

PRINT 'Ürün sistemi veritabanı güncellemeleri tamamlandı!';
GO
