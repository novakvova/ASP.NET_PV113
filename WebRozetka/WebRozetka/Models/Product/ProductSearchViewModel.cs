namespace WebRozetka.Models.Product
{
    public class ProductSearchViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }

        /// <summary>
        /// Номер сторінки
        /// </summary>
        /// <example>1</example>
        public int? Page { get; set; } = 1;

        /// <summary>
        /// Кількість записів на сторінці
        /// </summary>
        /// <example>5</example>
        public int? PageSize { get; set; }
    }
}
