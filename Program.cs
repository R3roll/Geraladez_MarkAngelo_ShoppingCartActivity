using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Products
        string[] productNames = { "Water", "Soda", "Chips", "Chocolate", "Apple" };
        decimal[] productPrices = { 10, 15, 7, 25, 12 };
        int[] productStock = { 50, 50, 50, 50, 50 };

        List<string> cartItems = new List<string>();
        List<int> cartQty = new List<int>();

        bool shopping = true;

        while (shopping)
        {
            Console.WriteLine("\n===== STORE MENU =====");

            for (int i = 0; i < productNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {productNames[i]} - {productPrices[i]} PHP ({productStock[i]} left)");
            }

            Console.WriteLine("\nV - View Cart");
            Console.WriteLine("C - Checkout");

            Console.Write("Enter choice: ");
            string? inputRaw = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputRaw))
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            string input = inputRaw.Trim().ToUpper();

            // VIEW CART
            if (input == "V")
            {
                while (true)
                {
                    Console.WriteLine("\n===== CART MENU =====");

                    if (cartItems.Count == 0)
                    {
                        Console.WriteLine("Cart is empty.");
                    }
                    else
                    {
                        decimal cartTotal = 0;

                        for (int i = 0; i < cartItems.Count; i++)
                        {
                            int productIndex = Array.IndexOf(productNames, cartItems[i]);
                            decimal itemTotal = cartQty[i] * productPrices[productIndex];
                            cartTotal += itemTotal;

                            Console.WriteLine($"{i + 1}. {cartItems[i]} - Qty: {cartQty[i]} - Total: {itemTotal} PHP");
                        }

                        Console.WriteLine($"Cart Total: {cartTotal} PHP");
                    }

                    Console.WriteLine("\n1. Remove Item");
                    Console.WriteLine("2. Update Quantity");
                    Console.WriteLine("3. Clear Cart");
                    Console.WriteLine("4. Back");

                    Console.Write("Choose option: ");
                    string? cartChoiceRaw = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(cartChoiceRaw))
                    {
                        Console.WriteLine("Invalid option.");
                        continue;
                    }

                    string cartChoice = cartChoiceRaw.Trim();

                    // REMOVE ITEM
                    if (cartChoice == "1")
                    {
                        if (cartItems.Count == 0)
                        {
                            Console.WriteLine("Cart is empty.");
                            continue;
                        }

                        Console.Write("Enter item number to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int removeIndex)
                            && removeIndex >= 1 && removeIndex <= cartItems.Count)
                        {
                            int actualIndex = removeIndex - 1;
                            int productIndex = Array.IndexOf(productNames, cartItems[actualIndex]);

                            productStock[productIndex] += cartQty[actualIndex]; // return stock

                            Console.WriteLine($"{cartItems[actualIndex]} removed.");

                            cartItems.RemoveAt(actualIndex);
                            cartQty.RemoveAt(actualIndex);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    }
                    // UPDATE QUANTITY
                    else if (cartChoice == "2")
                    {
                        if (cartItems.Count == 0)
                        {
                            Console.WriteLine("Cart is empty.");
                            continue;
                        }

                        Console.Write("Enter item number to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateIndex)
                            && updateIndex >= 1 && updateIndex <= cartItems.Count)
                        {
                            int actualIndex = updateIndex - 1;
                            int productIndex = Array.IndexOf(productNames, cartItems[actualIndex]);

                            Console.Write("Enter new quantity: ");
                            if (int.TryParse(Console.ReadLine(), out int newQty) && newQty > 0)
                            {
                                int oldQty = cartQty[actualIndex];
                                int difference = newQty - oldQty;

                                if (difference > productStock[productIndex])
                                {
                                    Console.WriteLine("Not enough stock.");
                                }
                                else
                                {
                                    productStock[productIndex] -= difference;
                                    cartQty[actualIndex] = newQty;
                                    Console.WriteLine("Quantity updated.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid quantity.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    }
                    // CLEAR CART
                    else if (cartChoice == "3")
                    {
                        for (int i = 0; i < cartItems.Count; i++)
                        {
                            int productIndex = Array.IndexOf(productNames, cartItems[i]);
                            productStock[productIndex] += cartQty[i];
                        }

                        cartItems.Clear();
                        cartQty.Clear();

                        Console.WriteLine("Cart cleared.");
                    }
                    // BACK
                    else if (cartChoice == "4")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                    }
                }
            }
            // CHECKOUT
            else if (input == "C")
            {
                shopping = false;
            }
            // ADD PRODUCT
            else if (int.TryParse(input, out int choice)
                && choice >= 1 && choice <= productNames.Length)
            {
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

                int existingIndex = cartItems.IndexOf(productNames[index]);
                if (existingIndex != -1)
                {
                    cartQty[existingIndex] += qty;
                }
                else
                {
                    cartItems.Add(productNames[index]);
                    cartQty.Add(qty);
                }

                Console.WriteLine($"{qty} {productNames[index]}(s) added to cart.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        // RECEIPT
        Console.WriteLine("\n===== RECEIPT =====");
        decimal total = 0;

        for (int i = 0; i < cartItems.Count; i++)
        {
            int productIndex = Array.IndexOf(productNames, cartItems[i]);
            decimal subtotal = cartQty[i] * productPrices[productIndex];

            Console.WriteLine($"{cartItems[i]} - {cartQty[i]} x {productPrices[productIndex]} PHP = {subtotal} PHP");
            total += subtotal;
        }

        Console.WriteLine($"Final Total: {total} PHP");
        Console.WriteLine("\nThank you for shopping!");
    }
}
