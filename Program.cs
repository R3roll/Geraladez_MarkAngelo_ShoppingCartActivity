using System;
using System.Collections.Generic;
using System.Linq;

namespace newassignment.VendingMachineApp
{
    class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }

        public Item(int id, string name, int stock, double price)
        {
            Id = id;
            Name = name;
            Stock = stock;
            Price = price;
        }
    }

    class CartItem
    {
        public Item Product { get; set; }
        public int Quantity { get; set; }

        public double TotalPrice => Product.Price * Quantity;
    }

    class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public List<CartItem> Items => items;

        public void AddItem(Item product, int quantity)
        {
            var existing = items.FirstOrDefault(i => i.Product.Id == product.Id);

            if (existing != null)
                existing.Quantity += quantity;
            else
                items.Add(new CartItem { Product = product, Quantity = quantity });
        }

        public void RemoveItem(string name)
        {
            var existing = items.FirstOrDefault(i =>
                i.Product.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                items.Remove(existing);
            else
                Console.WriteLine("Item not found in cart.");
        }

        public void UpdateQuantity(string name, int newQty)
        {
            var existing = items.FirstOrDefault(i =>
                i.Product.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                existing.Quantity = newQty;
            else
                Console.WriteLine("Item not found in cart.");
        }

        public void ClearCart() => items.Clear();

        public double GetTotal() => items.Sum(i => i.TotalPrice);

        public bool IsEmpty() => !items.Any();

        public void ViewCart()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            Console.WriteLine("\nYour Cart:");
            Console.WriteLine("---------------------------------------------------");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Product.Name} (x{item.Quantity}) - PHP {item.TotalPrice}");
            }

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine($"Total: PHP {GetTotal()}");
        }
    }

    class Program
    {
        static void Main()
        {
            List<Item> inventory = new List<Item>
            {
                new Item(1, "Soda", 60, 15),
                new Item(2, "Chips", 60, 15),
                new Item(3, "Fruits", 60, 15),
                new Item(4, "Chocolate", 60, 15),
                new Item(5, "Water", 60, 15)
            };

            Cart cart = new Cart();
            bool running = true;

            Console.WriteLine("=== Welcome to the Vending Machine ===\n");

            while (running)
            {
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add item to cart");
                Console.WriteLine("2. View cart");
                Console.WriteLine("3. Remove item from cart");
                Console.WriteLine("4. Update item quantity");
                Console.WriteLine("5. Clear cart");
                Console.WriteLine("6. Checkout");
                Console.WriteLine("7. Exit");

                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":

                        DisplayInventory(inventory);

                        Console.Write("Enter Product ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int productId))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }

                        var selected = inventory.FirstOrDefault(i => i.Id == productId);

                        if (selected == null)
                        {
                            Console.WriteLine("Product not found.");
                            break;
                        }

                        Console.Write("Enter quantity: ");

                        if (int.TryParse(Console.ReadLine(), out int qty) &&
                            qty > 0 &&
                            qty <= selected.Stock)
                        {
                            cart.AddItem(selected, qty);
                            selected.Stock -= qty;

                            Console.WriteLine($"{qty} {selected.Name}(s) added to cart.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity.");
                        }

                        break;

                    case "2":
                        cart.ViewCart();
                        break;

                    case "3":

                        Console.Write("Enter item name to remove: ");
                        cart.RemoveItem(Console.ReadLine());

                        break;

                    case "4":

                        Console.Write("Enter item name to update: ");
                        string updateName = Console.ReadLine();

                        Console.Write("Enter new quantity: ");

                        if (int.TryParse(Console.ReadLine(), out int newQty) && newQty > 0)
                            cart.UpdateQuantity(updateName, newQty);
                        else
                            Console.WriteLine("Invalid quantity.");

                        break;

                    case "5":

                        cart.ClearCart();
                        Console.WriteLine("Cart cleared.");

                        break;

                    case "6":

                        Checkout(cart);
                        running = false;

                        break;

                    case "7":

                        Console.WriteLine("Exiting... Thank you!");
                        running = false;

                        break;

                    default:

                        Console.WriteLine("Invalid input! Try again.");

                        break;
                }
            }
        }

        static void DisplayInventory(List<Item> inventory)
        {
            Console.WriteLine("\nAvailable Items:");
            Console.WriteLine("---------------------------------------------------");

            foreach (var item in inventory)
            {
                Console.WriteLine(
                    $"ID: {item.Id} | {item.Name} | PHP {item.Price} | Stock: {item.Stock}");
            }

            Console.WriteLine("---------------------------------------------------");
        }

        static void Checkout(Cart cart)
        {
            if (cart.IsEmpty())
            {
                Console.WriteLine("Cart is empty. Nothing to checkout.");
                return;
            }

            double total = cart.GetTotal();
            double discount = total > 5000 ? total * 0.10 : 0;
            double finalTotal = total - discount;

            Console.WriteLine("\n=== RECEIPT ===");

            string receiptNumber =
                Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            Console.WriteLine($"Receipt No: {receiptNumber}");
            Console.WriteLine($"Date: {DateTime.Now}");

            Console.WriteLine("---------------------------------------------------");

            foreach (var item in cart.Items)
            {
                Console.WriteLine(
                    $"{item.Product.Name} x{item.Quantity} - PHP {item.TotalPrice}");
            }

            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine($"Subtotal: PHP {total}");
            Console.WriteLine($"Discount: PHP {discount}");
            Console.WriteLine($"Final Total: PHP {finalTotal}");

            Console.WriteLine("---------------------------------------------------");

            Console.Write("Enter payment amount: PHP ");

            if (double.TryParse(Console.ReadLine(), out double payment) &&
                payment >= finalTotal)
            {
                double change = payment - finalTotal;

                Console.WriteLine($"Change: PHP {change}");
                Console.WriteLine("Transaction Complete. Thank you!");
            }
            else
            {
                Console.WriteLine("Insufficient payment.");
            }
        }
    }
}