namespace Beca.Store.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
       
        public decimal TotalPrince {get; set; }

        public ICollection<ProductModel> Products { get; set; }


        public OrderModel(int id, string name, ICollection<ProductModel> products)
        {
            Id = id;
            Name = name;
            Products = products;
            TotalPrince = products.Sum(p => p.Price); 
        }
    }
}
