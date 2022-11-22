namespace TRPO_1.Models
{
    public class Product
    {
        public string Name { get; set; }
        public float Price { get; set; }

        public Product Clone()
        {
            return (Product)MemberwiseClone();
        }
    }
}
