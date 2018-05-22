namespace ShopHierarchy
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Startup
    {
        public static void Main()
        {
            var db = new ShopDbContext();
            using (ShopDbContext context = new ShopDbContext())
            {
                ClearDatabase(context);
                FillSalesmen(context);
                SaveItems(context);
                ReadCommands(context);
                //PrintSalesmenCustomersCount(context);
                //PrintCustomersWithOrdersAndReviewsCount(context);
                //PrintCustomersWithOrdersAndReviews(context);
                //PrintCustomersOrdersRivewsSalesman(context);
                PrintOrderWithMoreThanOneItem(context);
            }
        }

        private static void PrintOrderWithMoreThanOneItem(ShopDbContext context)
        {
            int customerId = int.Parse(Console.ReadLine());

            var orders = context
               .Orders
               .Where(o => o.CustomerId == customerId)
               .Where(o => o.Items.Count > 1)
               .Count();

            Console.WriteLine($"Orders count: {orders}");
        }

        private static void PrintCustomersOrdersRivewsSalesman(ShopDbContext context)
        {
            int customerId = int.Parse(Console.ReadLine());

            var customerData = context
                .Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    Name = c.Name,
                    Orders = c.Orders.Count,
                    Reviews = c.Reviews.Count,
                    SalesmanName = c.Salesman.Name
                })
                .FirstOrDefault();
            Console.WriteLine($"Customer: {customerData.Name}");
            Console.WriteLine($"Orders count: {customerData.Orders}");
            Console.WriteLine($"Reviews: {customerData.Reviews}");
            Console.WriteLine($"Saleman: {customerData.SalesmanName}");
        }

        private static void PrintCustomersWithOrdersAndReviews(ShopDbContext context)
        {
            int customerId = int.Parse(Console.ReadLine());

            var customerData = context
                .Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    Orders = c.Orders.Select(o => new
                    {
                        o.Id,
                        Items = o.Items.Count
                    })
                    .OrderBy(o => o.Id),
                    Reviews = c.Reviews.Count
                })
                .FirstOrDefault();

            foreach (var order in customerData.Orders)
            {
                Console.WriteLine($"order {order.Id}: {order.Items} items");
            }
            Console.WriteLine($"reviews: {customerData.Reviews}");
        }

        private static void SaveItems(ShopDbContext context)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "END")
                {
                    break;
                }
                var parts = line.Split(';');
                var itemName = parts[0];
                var itemPrice = decimal.Parse(parts[1]);

                context.Add(new Item
                {
                    Name = itemName,
                    Price = itemPrice
                });
            }
        }

        private static void PrintCustomersWithOrdersAndReviewsCount(ShopDbContext context)
        {
            var customerData = context
                .Customers
                .Select(c => new
                {
                    c.Name,
                    Orders = c.Orders.Count,
                    Reviews = c.Reviews.Count
                })
           .OrderByDescending(c => c.Orders)
           .ThenByDescending(c => c.Reviews)
           .ToList();

            foreach (var customer in customerData)
            {
                Console.WriteLine($"{customer.Name}");
                Console.WriteLine($"Orders: {customer.Orders}");
                Console.WriteLine($"Reviews: {customer.Reviews}");
            }
        }

        private static void PrintSalesmenCustomersCount(ShopDbContext context)
        {
            var salesmenData = context.Salesmen
                .Select(s => new
                {
                    s.Name,
                    Customers = s.Customers.Count
                })
                .OrderByDescending(s => s.Customers)
                .ThenBy(s => s.Name)
                .ToList();

            foreach (var salesname in salesmenData)
            {
                Console.WriteLine($"{salesname.Name} - {salesname.Customers} customers");
            }
        }

        private static void ReadCommands(ShopDbContext context)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "END")
                {
                    break;
                }
                var parts = line.Split('-');
                var command = parts[0];

                var arguments = parts[1];
                switch (command)
                {
                    case "register":
                        RegisterCustomer(context, arguments);
                        break;
                    case "order":
                        SaveOrder(context, arguments);
                        break;
                    case "review":
                        SaveReview(context, arguments);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void SaveReview(ShopDbContext context, string arguments)
        {
            var parts = arguments.Split(';');
            var customerId = int.Parse(parts[0]);
            var itemId = int.Parse(parts[1]);


            context.Add(new Review
            {
                CustomerId = customerId,
                ItemId = itemId
            });
            context.SaveChanges();
        }

        private static void SaveOrder(ShopDbContext context, string arguments)
        {
            var parts = arguments.Split(';');
            var customerId = int.Parse(parts[0]);
            var itemIds = new HashSet<int>();
            var order = new Order { CustomerId = customerId };

            for (int i = 1; i < parts.Length; i++)
            {
                var itemId = int.Parse(parts[i]);
                order.Items.Add(new ItemOrder
                {
                    ItemId = itemId,
                });
            }

            context.Add(order);

            context.SaveChanges();
        }

        private static void RegisterCustomer(ShopDbContext context, string arguments)
        {
            var parts = arguments.Split(';');
            var customerName = parts[0];
            var salesmanId = int.Parse(parts[1]);

            context.Add(new Customer
            {
                Name = customerName,
                SalesmanId = salesmanId
            });
            context.SaveChanges();

        }

        private static void FillSalesmen(ShopDbContext context)
        {
            var salesmanNames = Console.ReadLine().Split(';');
            foreach (var name in salesmanNames)
            {
                context.Salesmen.Add(new Salesman() { Name = name });
            }
            context.SaveChanges();

        }

        private static void ClearDatabase(ShopDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
