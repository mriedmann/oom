using System;
using System.Collections.Generic;

// variables, primitive types (int, double, string, bool), arrays, type inference
// collections (List<T>), generics
// operators (+,-,*,/,%,++,--,<=,<,==,!=,>,>=,&&,||,?:,??,!,&,|,^,~,<<,>>,op=)
// statements (if, switch, for, while, do, foreach)
// methods/functions
// exception handling

namespace lesson1
{
    class Program
    {
        static void Main(string[] args)
        {
            ///////////////////////////////////////////////////////////////////////
            // variables
            int a = 5;
            string hello = "Hello World!";
            double x = 123.456;
            bool b = true;

            ///////////////////////////////////////////////////////////////////////
            // print to console
            Console.WriteLine(hello);

            ///////////////////////////////////////////////////////////////////////
            // string and other values can be concatenated using + 
            Console.WriteLine("The value of x*a is " + (x * a) + ", and b is " + b + ".");
            // ... or you can use place holders ...
            Console.WriteLine("The value of x*a is {0}, and b is {1}.", x * a, b);
            // ... or string interpolation
            Console.WriteLine($"The value of x*a is {x * a}, and b is {b}.");

            ///////////////////////////////////////////////////////////////////////
            // arrays
            int[] xs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            ///////////////////////////////////////////////////////////////////////
            // type inference (compiler automatically finds out that ys is of type int[])
            var ys = new [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            ///////////////////////////////////////////////////////////////////////
            // collections (e.g. List<T>), generics
            var list1 = new List<string> { "This", "is", "a", "list", "of", "strings", "!" };
            var list2 = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            ///////////////////////////////////////////////////////////////////////
            // operators
            var o1 = a * 7 + xs[5] % ys[2];
            var o2 = a > 0 && a < 10 || !b || hello == "Hallo";

            ///////////////////////////////////////////////////////////////////////
            // statements
            if (x > 100)
            {
                Console.WriteLine ("x is greater than 100");
            }
            else
            {
                Console.WriteLine ("x is NOT greater than 100");
            }

            switch (a)
            {
                case 0:
                    Console.WriteLine ("a is 0");
                    break;
                case 5:
                    Console.WriteLine ("a is 5");
                    break;
                default:
                    Console.WriteLine ("a is something else");
                    break;
            }

            for (var i = 0; i < xs.Length; i++)
            {
                Console.WriteLine(i);
            }

            while (a < 100)
            {
                Console.WriteLine($"a = {a}");
                a *= 2;
            }

            do
            {
                Console.WriteLine($"a = {a}");
                a /= 2;
            } while (a > 5);

            foreach (var s in list1)
            {
                Console.WriteLine(s);
            }

            ///////////////////////////////////////////////////////////////////////
            // methods/functions
            Console.WriteLine("The square of 5 is " + Square(5));

            ///////////////////////////////////////////////////////////////////////
            // exceptions
            try
            {
                string s1 = "Some string ...";
                string s2 = null;
                Console.WriteLine($"s1 has length " + s1.Length);
                Console.WriteLine($"s2 has length " + s2.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        /// <summary>
        /// Square the specified x.
        /// </summary>
        static int Square(int x)
        {
            return x * x;
        }
    }
}
