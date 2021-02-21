using System.Linq;

namespace System
{
    public static class StructExtensions
    {
        public static T NextOrFirst<T>(this T @enum) 
            where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            }

            var enumValues = (T[])Enum.GetValues(@enum.GetType());
            var indexOfNextValue = Array.IndexOf(array: enumValues, value: @enum) + 1;

            if (enumValues.Length == indexOfNextValue)
            {
                return enumValues[0];
            }

            return enumValues[indexOfNextValue];
        }
    }
}