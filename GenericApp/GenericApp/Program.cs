using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericApp
{
    public class SimpleGeneric<T>
    {
        private T[] values;
        private int index;
        public SimpleGeneric(int len)   // Constructor
        {
            values = new T[len];
            index = 0;
        }

        public void Add(params T[] args)
        {
            foreach (T e in args)
                values[index++] = e;
        }

        public void Print()
        {
            foreach (T e in values)
                Console.Write(e + " ");
            Console.WriteLine();
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
