namespace HomeTechBlazor.Model
{
    public class Equipments
    {
        public int IdProduct { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public int Quantity { get; set; } = 1;
        public int Price { get; set; } = 0;
        public string? Description { get; set; } = "";
    }
}
