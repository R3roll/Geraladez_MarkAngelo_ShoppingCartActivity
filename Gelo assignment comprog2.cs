using System;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Simple product list
        string[] productNames = { "Water", "Soda", "Chips", "Chocolate", "Apple" };
        decimal[] productPrices = { 10, 15, 7, 25, 12 };
        int[] productStock = { 50, 50, 50, 50, 50 };

        List<string> cartItems = new List<string>();
        List<int> cartQty = new List<int>();
        List<decimal> cartTotals = new List<decimal>();

        while (true)
        {
            Console.WriteLine("\nStore Menu:");
            for (int i = 0; i < productNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {productNames[i]} - {productPrices[i]} PHP ({productStock[i]} left)");
            }

            Console.Write("Enter product number (or 'N' to checkout): ");
            string input = Console.ReadLine().Trim().ToUpper();

            if (input == "N") break;

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > productNames.Length)
            {
                Console.WriteLine("Invalid choice. Try again.");
                continue;
            }

            int index = choice - 1;
            if (productStock[index] == 0)
            {
                Console.WriteLine("Out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (qty > productStock[index])
            {
                Console.WriteLine("Not enough stock.");
                continue;
            }

            productStock[index] -= qty;
            cartItems.Add(productNames[index]);
            cartQty.Add(qty);
            cartTotals.Add(productPrices[index] * qty);

            Console.WriteLine($"{qty} {productNames[index]}(s) added to cart.");
        }

        // Receipt
        Console.WriteLine("\nReceipt:");
        decimal total = 0;
        for (int i = 0; i < cartItems.Count; i++)
        {
            Console.WriteLine($"{cartItems[i]} - {cartQty[i]} x {cartTotals[i] / cartQty[i]} PHP = {cartTotals[i]} PHP");
            total += cartTotals[i];
        }

        decimal discount = 0;
        if (total >= 5000)
        {
            discount = total * 0.10m;
            total -= discount;
            Console.WriteLine($"Discount (10%): -{discount} PHP");
        }

        Console.WriteLine($"Final Total: {total} PHP");

        // Updated stock
        Console.WriteLine("\nUpdated Stock:");
        for (int i = 0; i < productNames.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {productNames[i]} - {productPrices[i]} PHP ({productStock[i]} left)");
        }

        Console.WriteLine("\nThank you for shopping!");
    }
}