using System;
using System.Reflection;

namespace RewriteExploratoryUnitTester.Extensions
{
    public static class EnumExtensions
    {
        ///// <summary>
        ///// Check to see if a flags enumeration has a specific flag set.
        ///// </summary>
        ///// <see cref="http://stackoverflow.com/questions/4108828/generic-extension-method-to-see-if-an-enum-contains-a-flag"/>
        ///// <param name="variable">Flags enumeration to check</param>
        ///// <param name="value">Flag to check for</param>
        ///// <returns></returns>
        //public static bool HasFlag<T>(this Enum variable, T value)
        //{
        //    if (variable == null)
        //        return false;

        //    //if (value == null)
        //    //    throw new ArgumentNullException("value");

        //    if (variable.GetType() != typeof(T))
        //        throw new ArgumentException("Invalid enum type", "value");

        //    // Not as good as the .NET 4 version of this function, but should be good enough
        //    if (!Enum.IsDefined(variable.GetType(), value))
        //    {
        //        throw new ArgumentException(string.Format(
        //            "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
        //            value.GetType(), variable.GetType()));
        //    }

        //    ulong num = Convert.ToUInt64(value);
        //    return ((Convert.ToUInt64(variable) & num) == num);
        //}

        #region From http://stackoverflow.com/a/4110912
        delegate bool HasFlag<T>(T left, T right);
        static readonly HasFlag<Byte> Byte = (x, y) => (x & y) == y;
        static readonly HasFlag<SByte> Sbyte = (x, y) => (x & y) == y;
        static readonly HasFlag<Int16> Int16 = (x, y) => (x & y) == y;
        static readonly HasFlag<UInt16> UInt16 = (x, y) => (x & y) == y;
        static readonly HasFlag<Int32> Int32 = (x, y) => (x & y) == y;
        static readonly HasFlag<UInt32> UInt32 = (x, y) => (x & y) == y;
        static readonly HasFlag<Int64> Int64 = (x, y) => (x & y) == y;
        static readonly HasFlag<UInt64> UInt64 = (x, y) => (x & y) == y;

        public static bool HasFlags<TEnum>(this TEnum @enum, TEnum flag)
            where TEnum : struct,IConvertible, IComparable, IFormattable
        {
            return Enum<TEnum>.HasFlag(@enum, flag);
        }
        class Enum<TEnum> where TEnum : struct,IConvertible, IComparable, IFormattable
        {
            public static HasFlag<TEnum> HasFlag = CreateDelegate();
            static HasFlag<TEnum> CreateDelegate()
            {
                if (!typeof(TEnum).IsEnum) throw new ArgumentException(string.Format("{0} is not an enum", typeof(TEnum)), typeof(Enum<>).GetGenericArguments()[0].Name);
                var delegateName = Type.GetTypeCode(typeof(TEnum)).ToString();
                var @delegate = typeof(EnumExtensions).GetField(delegateName, BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Delegate;
                return Delegate.CreateDelegate(typeof(HasFlag<TEnum>), @delegate.Method) as HasFlag<TEnum>;
            }
        }
        #endregion From http://stackoverflow.com/a/4110912

        #region Taken from http://stackoverflow.com/questions/276585/enumeration-extension-methods

        public static bool TryParse<T>(this Enum theEnum, string strType,
                                       out T result)
        {
            string strTypeFixed = strType.Replace(' ', '_');
            if (Enum.IsDefined(typeof (T), strTypeFixed))
            {
                result = (T) Enum.Parse(typeof (T), strTypeFixed, true);
                return true;
            }
            else
            {
                foreach (string value in Enum.GetNames(typeof (T)))
                {
                    if (value.Equals(strTypeFixed,
                                     StringComparison.OrdinalIgnoreCase))
                    {
                        result = (T) Enum.Parse(typeof (T), value);
                        return true;
                    }
                }
                result = default(T);
                return false;
            }
        }

        public static void ForEach(this Enum enumType, Action<Enum> action)
        {
            foreach (var type in Enum.GetValues(enumType.GetType()))
            {
                action((Enum) type);
            }
        }

        public static ConvertType ConvertTo<ConvertType>(this Enum e)
        {
            object o = null;
            Type type = typeof (ConvertType);

            if (type == typeof (int))
            {
                o = Convert.ToInt32(e);
            }
            else if (type == typeof (long))
            {
                o = Convert.ToInt64(e);
            }
            else if (type == typeof (short))
            {
                o = Convert.ToInt16(e);
            }
            else
            {
                o = Convert.ToString(e);
            }

            return (ConvertType) o;
        }

        #endregion Taken from http://stackoverflow.com/questions/276585/enumeration-extension-methods
    }
}
