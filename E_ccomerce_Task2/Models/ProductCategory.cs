namespace E_ccomerce_Task2.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategorieId { get; set; }
        public Product product { get; set; }
        public Categorie Categorie { get; set; }
    }
}
