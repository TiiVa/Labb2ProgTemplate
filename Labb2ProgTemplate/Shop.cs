using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Labb2ProgTemplate
{
    public class Shop
    {
        private Customer CurrentCustomer { get; set; }
        private List<Customer> AllCustomersList { get; set; } = new List<Customer>();

        public CurrencyHandler CurrencyChoice = new CurrencyHandler();

        private List<Product> Products { get; set; } = new List<Product>()
        {  new Product("Apple", 10),
           new Product("Hot Dog", 20),
           new Product("Lemonade", 15) };

        public Shop()
        {
           
            AllCustomersList = new List<Customer>()
            {
                new Customer("Knatte", "123", "Gold"),
                new Customer("Fnatte", "321", "Silver"),
                new Customer("Tjatte", "213", "Bronze"),
                new Customer("Nemo", "231", "Gold"),
                new Customer("Doris", "312", "Bronze"),
            };
        }
        public void MainMenu()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Clear();

            Console.WriteLine("Welcome to pinkDragonFly!\n\n1 - Log in\n2 - Register as a new customer");
            string mainMenuChoice = Console.ReadLine();

            for (int i = 0; i < mainMenuChoice.Length; i++)
            {
                
                if (mainMenuChoice != "1" && mainMenuChoice != "2")
                {
                    Console.WriteLine("Invalid selection, please try again:");
                    mainMenuChoice = Console.ReadLine();
                }
                if (mainMenuChoice == "1")
                {
                    Login();
                }
                else if (mainMenuChoice == "2")
                {
                    Register();
                }
            }
        }
        private void Login()
        {
            Console.Clear();
            Console.WriteLine("Please enter your username: ");
            string userName = Console.ReadLine();
            bool customerFound = false;


            for (int i = 0; i < AllCustomersList.Count; i++)
            {
                if (userName == AllCustomersList[i].Name)
                {
                    customerFound = true;
                    Console.WriteLine("Please enter your password: ");
                    var inputPassword = Console.ReadLine();

                    while (!AllCustomersList[i].CheckPassword(inputPassword))
                    {
                        Console.WriteLine("Incorrect password, please try again:");
                        inputPassword = Console.ReadLine();

                    }
                    if (AllCustomersList[i].CheckPassword(inputPassword))
                    {
                        CurrentCustomer = AllCustomersList[i];

                        Console.Clear();
                        Console.WriteLine($"Welcome {userName}! You logged in successfully.\n\n1 - Go to the shop\n2 - View your cart\n3 - Go to checkout");

                        string choiceForLoggedInCustomer = Console.ReadLine();

                        if (choiceForLoggedInCustomer != "1" && choiceForLoggedInCustomer != "2" && choiceForLoggedInCustomer != "3")
                        {
                            Console.WriteLine("Something went wrong, please try again:");
                            choiceForLoggedInCustomer = Console.ReadLine();
                        }
                        if (choiceForLoggedInCustomer == "1")
                        {
                            ShopMenu();
                        }
                        else if (choiceForLoggedInCustomer == "2")
                        {
                            ViewCart();
                        }
                        else if (choiceForLoggedInCustomer == "3")
                        {
                            Checkout();
                        }
                    }
                }
            }
            if (!customerFound)
            {
                Console.WriteLine("Customer name could not be found.\nWould you like to register?\nY for yes or N for no.");
                string choiceForNotFoundCustomer = Console.ReadLine().ToUpper();

                while (true)
                {

                    if (choiceForNotFoundCustomer != "Y" && choiceForNotFoundCustomer != "N")
                    {
                        Console.WriteLine("Something went wrong, please try again.");
                        choiceForNotFoundCustomer = Console.ReadLine().ToUpper();
                    }
                    if (choiceForNotFoundCustomer == "Y")
                    {
                        Register();
                    }
                    else if (choiceForNotFoundCustomer == "N")
                    {
                        MainMenu();
                    }
                }
            }
        }
        private void Register()
        {
            Console.Clear();
            Console.WriteLine("Welcome to register as a new customer!\n\nPlease enter new username: ");
            string newUsername = Console.ReadLine();

            Console.WriteLine("Please enter new password: ");
            string newPassword = Console.ReadLine();

            AllCustomersList.Add(new Customer(newUsername, newPassword, "New"));
            Console.Clear();
            Console.WriteLine($"New customer has been registered, welcome {newUsername}!");
            Console.WriteLine("\nWould you like to log in?\nSelect Y for yes, N for no.");
            var newCustomerChoice = Console.ReadLine().ToUpper();

            if (newCustomerChoice != "Y" && newCustomerChoice != "N")
            {
                Console.WriteLine("Something went wrong. Please try again.");
                newCustomerChoice = Console.ReadLine();
            }
            if (newCustomerChoice == "Y")
            {
                Login();
            }
            else if (newCustomerChoice == "N")
            {
                MainMenu();
            }
        }
        private void ShopMenu()
        {
            Console.Clear();

            double total = 0;
            
            Console.WriteLine($"Welcome to the shop!\n\n1 - Add items to cart\n2 - Remove items from cart\n3 - View cart\n0 - Log out\n");
            var inputNumber = int.Parse(Console.ReadLine());

            while (inputNumber != 0)
            {
                Console.Clear();

                if (inputNumber == 1)
                {
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Console.WriteLine($"{Products[i].Name}, price: {Products[i].Price}SEK/pcs");
                        Console.WriteLine($"Please select the quantity you want to add: ");
                        var numberOfProductToAdd = int.Parse(Console.ReadLine());

                        for (int j = 0; j < numberOfProductToAdd; j++)
                        {
                            CurrentCustomer.AddToCart(Products[i]);
                            CurrentCustomer.QuantityOfEachProduct(Products[i]);

                        }
                        total += Products[i].Price * numberOfProductToAdd;
                    }

                    Console.WriteLine($"Current total: {total}\n\nPlease choose: \n");
                }
                else if (inputNumber == 2)
                {

                    foreach (var product in Products)
                    {
                        Console.WriteLine($"{product.Name}, price: {product.Price}SEK/pcs, quantity: {CurrentCustomer.QuantityOfEachProduct(product)}");
                    }

                    Console.WriteLine($"\nPlease remove 1pc of:\n1 - apple\n2 - hot dog\n3 - lemonade ");
                    var itemNumberToRemove = int.Parse(Console.ReadLine());

                    if (itemNumberToRemove >= 1 && itemNumberToRemove <= Products.Count)
                    {
                        var productToRemove = Products[itemNumberToRemove - 1];
                        CurrentCustomer.RemoveFromCart(productToRemove);
                        total -= productToRemove.Price;
                    }

                    Console.WriteLine($"Current total: {total}\n\nPlease choose: \n");
                }
                else if (inputNumber == 3)
                {
                    ViewCart();
                }

                Console.WriteLine($"1 - Add items to cart\n2 - Remove items from cart\n3 - View cart\n0 - Log out");
                inputNumber = int.Parse(Console.ReadLine());
            }
            if (inputNumber == 0)
            {
                LogOut();
            }
        }
        private void ViewCart()
        {
            Console.Clear();

            double cartTotal = CurrentCustomer.CartTotal();

            Console.WriteLine(CurrentCustomer.ToString());

            Console.WriteLine("Here is an overview of your current cart:\n");

            foreach (var item in Products)
            {
                if (CurrentCustomer.Cart.Contains(item))
                {
                    Console.WriteLine($"{item.Name}, {item.Price}SEK/pcs, quantity: {CurrentCustomer.QuantityOfEachProduct(item)}," + " part total: " + item.Price * CurrentCustomer.QuantityOfEachProduct(item) + "SEK");
                }
            }
            Console.WriteLine($"Cart total after discount: {cartTotal} SEK\n");
            Console.WriteLine("If you want to see your final price in another currency, please choose below, otherwise choose SEK\n1 - EUR\n2 - GBP\n3 - SEK\n");
            var otherCurrencyChoice = int.Parse(Console.ReadLine());
            string inputAsKey = "";

            if (otherCurrencyChoice != 1 && otherCurrencyChoice != 2 && otherCurrencyChoice != 3)
            {
                Console.WriteLine($"Something went wrong, please try again: ");
                otherCurrencyChoice = int.Parse(Console.ReadLine());
            }
            if (otherCurrencyChoice == 1)
            {
                inputAsKey = "EUR";
            }
            else if (otherCurrencyChoice == 2)
            {
                inputAsKey = "GBP";
            }
            else if (otherCurrencyChoice == 3)
            {
                inputAsKey = "SEK";
            }

            var newTotal = cartTotal * CurrencyChoice.PriceForDifferentCurrencies[inputAsKey];
            newTotal = Math.Round(newTotal, 2);
            Console.WriteLine($"Total with chosen currency: {newTotal}");

            Console.WriteLine($"\n1 - Checkout\n2 - Log out\n3 - Back to shop");
            var choice = int.Parse(Console.ReadLine());

            if (choice != 1 && choice != 2 && choice != 3)
            {
                Console.WriteLine($"Something went wrong, please try again: ");
                choice = int.Parse(Console.ReadLine());
            }
            if (choice == 1)
            {
                Checkout();
            }
            else if (choice == 2)
            {
                LogOut();
            }
            else if (choice == 3)
            {
                ShopMenu();
            }   
        }
        private void Checkout()
        {
            Console.Clear();
            
            CurrentCustomer.Cart.Clear();

            Console.WriteLine("You have checked out. Thank you for your purchase!");

            Console.WriteLine("\n1 - Back to the shop\n2 - Log out");
            var choice = Console.ReadLine();

            if (choice != "1" && choice != "2")
            {
                Console.WriteLine("Invalid input, please try again: ");
                choice = Console.ReadLine();
            }
            if (choice == "1")
            {
                ShopMenu();
            }
            else if (choice == "2")
            {
                LogOut();   
            }
        }
        private void LogOut()
        {
            Console.Clear();

            CurrentCustomer = null;

            Console.WriteLine("Thank you and hope to see you again soon!");

            Thread.Sleep(2000);

            MainMenu();

        }
        
    }
}
