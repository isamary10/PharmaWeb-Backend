namespace PharmaWeb.Dto
{
    public class MedicineCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<MedicineRawMaterialDto> Composition { get; set; }
    }
}
