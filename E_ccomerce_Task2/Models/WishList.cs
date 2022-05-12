namespace E_ccomerce_Task2.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public List<Product> products { get; set; }
        public String UserId { get; set; }
    }
}
