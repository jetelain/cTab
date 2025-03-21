using System;
using System.Linq;

namespace cTabWebApp.Messages
{
    public static class ArmaConverter
    {
        public static DateTime ToDateTime(int[] date)
        {
            if (date.Length < 5)
            {
                return DateTime.MinValue;
            }
            return new DateTime(date[0], date[1], date[2], date[3], date[4], 0, DateTimeKind.Utc);
        }

        public static DateTime ToDateTime(object date)
        {
            return ToDateTime(ToIntegerArray(date));
        }

        public static int[] ToIntegerArray(object value)
        {
            if (value is object[] array)
            {
                return array.Cast<double?>().Select(n => (int)n).ToArray();
            }
            return Array.Empty<int>();
        }

        public static double[] ToDoubleArray(object value)
        {
            if (value is object[] array)
            {
                return array.Cast<double?>().Select(n => (double)n).ToArray();
            }
            return Array.Empty<double>();
        }
    }
}
