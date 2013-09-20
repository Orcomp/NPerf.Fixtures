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
        private const int NATTRIBUTES = 10;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static object RandomSimpleObject()
        {
            var obj = new Dictionary<object, object>();
            for (var i = 0; i < NATTRIBUTES; i++)
            {
                obj["IntProp" + i] = i;
                obj["StringProp" + i] =
                    new string(Enumerable.Repeat(chars, 8).Select(s => s[r.Next(s.Length)]).ToArray());
            }

            return obj;
        }

        public static object RandomObjectOfDepth(int depth)
        {
            if (depth == 0)
            {
                return RandomSimpleObject();
            }

            var obj = new Dictionary<object, object>();
            var tmp = RandomObjectOfDepth(depth - 1);

            for (var i = 0; i < NATTRIBUTES; i++)
            {
                obj["ObjectProp" + i] = tmp;
            }

            return obj;
        }

        public static object RandomObjectOfSize(int sizeInBytes)
        {
            var obj = new Dictionary<object, object>();
            var size = 0;
            var i = 0;

            while (size < sizeInBytes)
            {
                obj["IntProp" + i] = r.Next();
                var str = new string(Enumerable.Repeat(chars, 8).Select(s => s[r.Next(s.Length)]).ToArray());
                obj["StringProp" + i] = str;
                    
                size += sizeof(int) + System.Text.ASCIIEncoding.Unicode.GetByteCount(str);
                i++;
            }

            return obj;
        }
    }
}