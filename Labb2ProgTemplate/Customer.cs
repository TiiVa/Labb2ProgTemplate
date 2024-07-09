using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2ProgTemplate
{
    public class Customer
    {
        public string Name { get; private set; }
        private string Password { get; set; }
        public string Level { get; set; }

        private List<Product> _cart;
        public List<Product> Cart { get { return _cart; } }


        public Customer(string name, string password, string level)
        {
            Name = name;
            Password = password;
            _cart = new List<Product>();
            Level = level;
            
        }
        
        public override string ToString()
        {

            return $"Name: {Name}\nPassword: {Password}\nYour current level: {Level}\nProducts in cart: {_cart.Count}\n";

        }
        public bool CheckPassword(string password)
        {
            if (password == Password)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void AddToCart(Product product)
        {
            _cart.Add(product);
        }

        public void RemoveFromCart(Product product) 
        {
            _cart.Remove(product);
        }

        public int QuantityOfEachProduct(Product product)
        {
            int amount = Cart.Count(str => str.Name.Contains(product.Name));
            return amount;
        }

        public double CartTotal()
        {
            double total = 0;

            double discountedTotal = 0;

            var uniqueProducts = _cart.Distinct();

            foreach (Product product in uniqueProducts)
            {
                int quantity = _cart.Count(p => p.Name == product.Name);
                total += product.Price * quantity;
            }
            if (Level == "Bronze")
            {
                discountedTotal = total * 0.95;
            }
            else if (Level == "Silver")
            {
                discountedTotal = total * 0.90;
            }
            else if (Level == "Gold")
            {
                discountedTotal = total * 0.85;
            }
            else if (Level == "New")
            {
                discountedTotal = total * 1;
            }

            return discountedTotal;
            
        }
    }
}
