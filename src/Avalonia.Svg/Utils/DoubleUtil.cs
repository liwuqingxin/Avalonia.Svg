using System;
using System.Runtime.CompilerServices;

namespace Avalonia.Svg.Utils
{
    internal static class DoubleExtensions
    {
        // Const values come from sdk/inc/crt/float.h
        private const double DBL_EPSILON = 2.2204460492503131e-016; // Smallest such that 1.0+DBL_EPSILON != 1.0

        /// <summary>
        /// 返回两个<see langword="double"/>类型的值是否相等（在<see langword="epsilon"/>误差允许范围内接近）。<br/><br/>
        /// <see langword="epsilon"/>=(Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131e-016。
        /// </summary>
        /// AreClose - Returns whether or not two doubles are "close".  That is, whether or 
        /// not they are within epsilon of each other.  Notes that this epsilon is proportional
        /// to the numbers themselves to that AreClose survives scalar multiplication.
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this 
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// <returns>
        /// bool - AreClose的比较结果。
        /// </returns>
        /// <param name="value1"> 待比较的第一个值。 </param>
        /// <param name="value2"> 待比较的第二个值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreClose(this double value1, double value2)
        {
            // in case they are Infinities (then epsilon check does not work)
            // if (value1 == value2) return true;
            if (value1.IsInfinity() && value2.IsInfinity()) return true;

            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DBL_EPSILON
            var eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DBL_EPSILON;
            var delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        /// <summary>
        /// 返回两个<see langword="double"/>类型的值是否是小于关系。
        /// </summary>
        /// LessThan - Returns whether or not the first double is less than the second double.
        /// That is, whether or not the first is strictly less than *and* not within epsilon of
        /// the other number. Note that this epsilon is proportional to the numbers themselves
        /// to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this 
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// <returns>
        /// bool - LessThan的比较结果。
        /// </returns>
        /// <param name="value1"> 待比较的第一个值。 </param>
        /// <param name="value2"> 待比较的第二个值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this double value1, double value2) => (value1 < value2) && !AreClose(value1, value2);

        /// <summary>
        /// 返回两个<see langword="double"/>类型的值是否是大于关系。
        /// </summary>
        /// GreaterThan - Returns whether or not the first double is greater than the second double.
        /// That is, whether or not the first is strictly greater than *and* not within epsilon of
        /// the other number.  Note that this epsilon is proportional to the numbers themselves
        /// to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this 
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// <returns>
        /// bool - GreaterThan的比较结果。
        /// </returns>
        /// <param name="value1"> 待比较的第一个值。 </param>
        /// <param name="value2"> 待比较的第二个值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this double value1, double value2) => (value1 > value2) && !AreClose(value1, value2);

        /// <summary>
        /// 返回两个<see langword="double"/>类型的值是否是小于等于关系。
        /// </summary>
        /// LessThanOrClose - Returns whether or not the first double is less than or close to
        /// the second double.  That is, whether or not the first is strictly less than or within
        /// epsilon of the other number.  Note that this epsilon is proportional to the numbers 
        /// themselves to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this 
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// <returns>
        /// bool - LessThanOrClose的比较结果。
        /// </returns>
        /// <param name="value1"> 待比较的第一个值。 </param>
        /// <param name="value2"> 待比较的第二个值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrClose(this double value1, double value2) => (value1 < value2) || AreClose(value1, value2);

        /// <summary>
        /// 返回两个<see langword="double"/>类型的值是否是大于等于关系。
        /// </summary>
        /// GreaterThanOrClose - Returns whether or not the first double is greater than or close to
        /// the second double.  That is, whether or not the first is strictly greater than or within
        /// epsilon of the other number.  Note that this epsilon is proportional to the numbers 
        /// themselves to that AreClose survives scalar multiplication.  Note,
        /// There are plenty of ways for this to return false even for numbers which
        /// are theoretically identical, so no code calling this should fail to work if this 
        /// returns false.  This is important enough to repeat:
        /// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
        /// used for optimizations *only*.
        /// <returns>
        /// bool - GreaterThanOrClose的比较结果。
        /// </returns>
        /// <param name="value1"> 待比较的第一个值。 </param>
        /// <param name="value2"> 待比较的第二个值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrClose(this double value1, double value2) => (value1 > value2) || AreClose(value1, value2);

        /// <summary>
        /// 返回double值是否等于1（在<see langword="epsilon"/>误差允许范围内）。
        /// 这个方法和AreClose(double, 1)的效果是一样的，但是会更高效一些。<br/><br/>
        /// <see langword="epsilon"/>=(Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131e-016。
        /// </summary>
        /// IsOne - Returns whether or not the double is "close" to 1.  Same as AreClose(double, 1),
        /// but this is faster.
        /// <returns>
        /// bool - IsOne的结果。
        /// </returns>
        /// <param name="value"> 待比较的值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(this double value) => Math.Abs(value - 1.0) < 10.0 * DBL_EPSILON;

        /// <summary>
        /// 返回double值是否等于0（在<see langword="epsilon"/>误差允许范围内）。
        /// 这个方法和AreClose(double, 0)的效果是一样的，但是会更高效一些。<br/><br/>
        /// </summary>
        /// <returns>
        /// bool - IsZero的结果。
        /// </returns>
        /// <param name="value"> 待比较的值。 </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this double value) => Math.Abs(value) < 10.0 * DBL_EPSILON;

        /// <summary>
        /// 值是否在0-1之间，包含0和1（在<see langword="epsilon"/>误差允许范围内）。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBetweenZeroAndOne(this double val) => GreaterThanOrClose(val, 0) && LessThanOrClose(val, 1);

        /// <summary>
        /// 四舍五入。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(this double val) => (0 < val) ? (int)(val + 0.5) : (int)(val - 0.5);

        /// <summary>
        /// 检查给定的值是否是无穷大（PositiveInfinity或者NegativeInfinity）。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(this double val) => double.IsInfinity(val);

        /// <summary>
        /// 检查给定的值是否处于限定范围内（不为无穷且不为NaN）。
        /// </summary>
        /// <param name='val'>The value to test.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this double val)
        {
            return !double.IsNaN(val) && !double.IsInfinity(val);
        }

        /// <summary>
        /// 检查给定的值的合法性（不为无穷不为NaN，且大于等于0（在<see langword="epsilon"/>误差允许范围内））。
        /// </summary>
        /// <param name='val'>The value to test.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidSize(this double val)
        {
            return IsFinite(val) && val.GreaterThanOrClose(0);
        }

        /// <summary>
        /// 检查给定的值是否处于限定范围内（不为无穷不为NaN，且大于等于0）。
        /// </summary>
        /// <param name="val"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFiniteAndNonNegative(this double val)
        {
            if (double.IsNaN(val) || double.IsInfinity(val) || val < 0)
            {
                return false;
            }

            return true;
        }
    }
}
