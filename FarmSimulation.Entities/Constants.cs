namespace FarmSimulation.Entities
{
    public static class AnimalTypes
    {
        public const string Chicken = "chicken";
        public const string Cow = "cow";
        public const string Sheep = "sheep";
    }

    public static class ProductTypes
    {
        public const string Egg = "Egg";
        public const string Milk = "Milk";
        public const string Wool = "Wool";
        public const string Unknown = "Unknown";
    }
    
    public static class ProductNames
    {
        public const string Egg = "Egg";
        public const string Milk = "Milk";
        public const string Wool = "Wool";
        public const string Unknown = "Unknown Product";
    }

    public static class Genders
    {
        public const string Male = "Male";
        public const string Female = "Female";
    }

    public static class GameSettings
    {
        public const int SecondsPerGameDay = 30;
        public const decimal InitialCash = 1000m;
        
        // Hayvan fiyatları
        public const decimal ChickenPrice = 10m;
        public const decimal CowPrice = 500m;
        public const decimal SheepPrice = 150m;
        
        // Ürün fiyatları
        public const decimal EggPrice = 1.0m;
        public const decimal MilkPrice = 5.0m;
        public const decimal WoolPrice = 8.0m;
        
        // Hayvan özellikleri
        public const int ChickenMaxAge = 15;
        public const int ChickenProductionTime = 15;
        
        public const int CowMaxAge = 20;
        public const int CowProductionTime = 45;
        
        public const int SheepMaxAge = 25;
        public const int SheepProductionTime = 60;
    }
}
