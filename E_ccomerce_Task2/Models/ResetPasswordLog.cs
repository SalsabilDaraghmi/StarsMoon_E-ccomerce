namespace E_ccomerce_Task2.Models
{
    public class ResetPasswordLog
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public String Email { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
