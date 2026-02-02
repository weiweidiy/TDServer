using JFramework;
using JFramework.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFrameTest
{
    public class Comp : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if(x == y) return 0;
            if(x < y) return -1;
            return 1;
        }
    }
    internal class TestBinaryInsertSort
    {
        [Test]
        public void TestSort()
        {
            var arr = new int[10] { 1, 3, 9, 7, 11, 9, 13, 15, 8, 19 };

            var utility = new Utility();
            utility.BinarySort<int>(arr, new Comp());

            foreach (var i in arr)
                Console.WriteLine(i);

            //var lst = arr.ToList();
            //lst.Sort(new Comp());

            //foreach (var i in lst)
            //    Console.WriteLine(i);
        }

        [Test]
        public void TestInsert()
        {
            var arr = new int[10] { 1, 3, 9, 10, 11, 12, 13, 15, 17, 19 };
            var lst = arr.ToList();
            var utility = new Utility();
            int index = utility.BinarySearch(lst, 5, new Comp());
            lst.Insert(index, 5);

            foreach (var i in lst)
                Console.WriteLine(i);
        }

        [Test]
        public void TestSearchInsert()
        {
            var arr = new int[10] { 1, 3, 9, 10, 11, 12, 13, 15, 17, 19 };
            var lst = arr.ToList();
            var utility = new Utility();
            utility.BinarySearchInsert(lst, 14, new Comp());

            foreach (var i in lst)
                Console.WriteLine(i);
        }
    }
}
