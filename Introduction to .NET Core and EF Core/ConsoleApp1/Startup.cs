namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Startup
    {
        public static void Main()
        {
            var result = new Dictionary<string, int>();
            var categories = new Dictionary<string, HashSet<string>>();
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "END")
                {
                    break;
                }
                var args = input.Split(' ').ToArray();
                var name = args[0];
                var cat = args[1];
                var point = int.Parse(args[2]);

                if (!result.ContainsKey(name))
                {
                    result[name] = 0;
                }
                if (!categories.ContainsKey(name))
                {
                    categories.Add(name, new HashSet<string>());
                }
                result[name] += point;
                categories[name].Add(cat);

            }
            var orderedResult = result
                .OrderByDescending(p => p.Value)
                .ThenBy(p => p.Key);

            foreach (var player in orderedResult)
            {
                var orderedCategories = categories[player.Key].OrderBy(c => c);
                Console.WriteLine($"{player.Key}: {player.Value} [{string.Join(", ", orderedCategories)}]");
            }
        }
    }
}
