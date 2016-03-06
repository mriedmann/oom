using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace lesson6
{
    public static class PullExample
    {
        public static void Run()
        {
            WriteLine("enumerables: foreach (array)");
            IEnumerable<int> xs = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            foreach (var x in xs) Write(x + " "); WriteLine();

            WriteLine("enumerables: foreach (list)");
            xs = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            foreach (var x in xs) Write(x + " "); WriteLine();
            
            WriteLine("enumerables: behind the scenes");
            var e = xs.GetEnumerator();
            while (e.MoveNext()) Write(e.Current + " "); WriteLine();

            WriteLine("enumerables: queries (filter) - Where(x => x % 2 == 0)");
            var ys = xs.Where(x => x % 2 == 0);
            foreach (var y in ys) Write(y + " "); WriteLine();

            WriteLine("enumerables: queries (map) - Select(x => x * x)");
            ys = xs.Select(x => x * x);
            foreach (var y in ys) Write(y + " "); WriteLine();
        }
    }
}
