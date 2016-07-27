using System;
using System.Linq.Expressions;

namespace Replace.Common
{
    public static class EnumExtensions
    {
        #region Public Static Methods

        /// <summary>
        /// Determines whether the specified value has flags. Note this method is up to 60 times faster
        /// than the one that comes with .NET 4 as it avoids any explict boxing or unboxing.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="flag">The flag.</param>
        /// <returns>
        ///  <c>true</c> if the specified value has flags; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">If TEnum is not an enum.</exception>
        public static bool HasFlags<TEnum>(this TEnum value, TEnum flag) where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return EnumExtensionsInternal<TEnum>.HasFlagsDelegate(value, flag);
        }

        #endregion Public Static Methods

        #region Nested Classes

        private static class EnumExtensionsInternal<TEnum> where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            #region Public Static Variables

            /// <summary>
            /// The delegate which determines if a flag is set.
            /// </summary>
            public static readonly Func<TEnum, TEnum, bool> HasFlagsDelegate = CreateHasFlagDelegate();

            #endregion Public Static Variables

            #region Private Static Methods

            /// <summary>
            /// Creates the has flag delegate.
            /// </summary>
            /// <returns></returns>
            private static Func<TEnum, TEnum, bool> CreateHasFlagDelegate()
            {
                if (!typeof(TEnum).IsEnum)
                {
                    throw new ArgumentException(string.Format("{0} is not an Enum", typeof(TEnum)), typeof(EnumExtensionsInternal<>).GetGenericArguments()[0].Name);
                }
                ParameterExpression valueExpression = Expression.Parameter(typeof(TEnum));
                ParameterExpression flagExpression = Expression.Parameter(typeof(TEnum));
                ParameterExpression flagValueVariable = Expression.Variable(Type.GetTypeCode(typeof(TEnum)) == TypeCode.UInt64 ? typeof(ulong) : typeof(long));
                Expression<Func<TEnum, TEnum, bool>> lambdaExpression = Expression.Lambda<Func<TEnum, TEnum, bool>>(
                  Expression.Block(
                    new[] { flagValueVariable },
                    Expression.Assign(
                      flagValueVariable,
                      Expression.Convert(
                        flagExpression,
                        flagValueVariable.Type
                      )
                    ),
                    Expression.Equal(
                      Expression.And(
                        Expression.Convert(
                          valueExpression,
                          flagValueVariable.Type
                        ),
                        flagValueVariable
                      ),
                      flagValueVariable
                    )
                  ),
                  valueExpression,
                  flagExpression
                );
                return lambdaExpression.Compile();
            }

            #endregion Private Static Methods
        }

        #endregion Nested Classes
    }
}