namespace NPerf.Fixture.ISerializer.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class ObjectRandomizerHelper
    {
        private static Random r = new Random();
        private const int ARRAY_SIZE = 10;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static object RandomSimpleObject()
        {
            var obj = new RandomObjectClass
                {
                    TheInteger = r.Next(),
                    TheString = new string(Enumerable.Repeat(chars, 8).Select(s => s[r.Next(s.Length)]).ToArray()),
                    TheIntArray = new int[ARRAY_SIZE],
                    TheObject = null
                };
            for (var i = 0; i < ARRAY_SIZE; i++)
            {
                obj.TheIntArray[i] = r.Next();
            }

            return obj;
        }

        public static object RandomObjectOfDepth(int depth)
        {
            if (depth == 0)
            {
                return RandomSimpleObject();
            }

            var obj = (RandomObjectClass)RandomSimpleObject();
            var tmp = (RandomObjectClass)RandomObjectOfDepth(depth - 1);
            obj.TheObject = tmp;

            return obj;
        }

        public static object RandomObjectOfSize(int sizeInBytes)
        {
            var obj = new RandomObjectClass();
            obj.TheInteger = r.Next();
            obj.TheObject = null;

            var halfSize = (sizeInBytes - sizeof(int)) / 2;
            var intCount = halfSize / sizeof(int);
            var stringCount = halfSize / sizeof(char);

            obj.TheString = new string(Enumerable.Repeat(chars, stringCount).Select(s => s[r.Next(s.Length)]).ToArray());
            
            obj.TheIntArray = new int[intCount]; 
            for (var i = 0; i < intCount; i++)
            {
                obj.TheIntArray[i] = r.Next();
            }

            return obj;
        }
    }

    public class RandomObjectClass
    {
        public string TheString { get; set; }

        public int TheInteger { get; set; }

        public int[] TheIntArray { get; set; }

        public RandomObjectClass TheObject { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            var tmp = (RandomObjectClass)obj;
            var j = 0;
            var arraysAreEquals = this.TheIntArray.All(i => i == tmp.TheIntArray[j++]);
            return (TheInteger == tmp.TheInteger) && (TheString.Equals(tmp.TheString))
                   && (arraysAreEquals) &&
                   ((TheObject == null && tmp.TheObject == null)||(TheObject.Equals(tmp.TheObject)));
        }
    }
}