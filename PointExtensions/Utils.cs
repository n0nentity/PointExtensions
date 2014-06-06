using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PointExtensions
{
    public static class Utils
    {

        /// <summary>
        /// Given an interval and a value this will output the value which is closer to the given value.
        /// 
        /// </summary>
        public static double Closer(double value, double choice1, double choice2)
        {
            double nearestValue;
            double otherValue;
            Utils.Closer(value, choice1, choice2, out nearestValue, out otherValue);
            return nearestValue;
        }

        /// <summary>
        /// Given an interval and a value this will output the value which is closer to the given value.
        /// 
        /// </summary>
        /// <param name="value">The value.</param><param name="choice1">The first choice.</param><param name="choice2">The second choice.</param><param name="nearestValue">The nearest value.</param><param name="otherValue">The other value.</param>
        public static void Closer(double value, double choice1, double choice2, out double nearestValue, out double otherValue)
        {
            Utils.Sort(ref choice1, ref choice2);
            if (value >= choice1 && (value > choice2 || value - choice1 >= choice2 - value))
            {
                nearestValue = choice2;
                otherValue = choice1;
            }
            else
            {
                nearestValue = choice1;
                otherValue = choice2;
            }
        }

        /// <summary>
        /// Returns the point of the interval which sits the closest to the given point.
        /// 
        /// </summary>
        /// <param name="point">The point seeking the closes neighbor.</param><param name="point1">The first point in the interval.</param><param name="point2">The second point in the interval.</param>
        public static Point Closer(Point point, Point point1, Point point2)
        {
            if (!Utils.AreDistanceOrdered(point, point1, point2))
                return point2;
            else
                return point1;
        }

        private static void Sort(ref double a, ref double b)
        {
            if (b >= a)
                return;
            double num = a;
            a = b;
            b = num;
        }

        private static bool AreDistanceOrdered(Point p, Point p1, Point p2)
        {
            return Utils.DistanceSquared(p, p1) < Utils.DistanceSquared(p, p2);
        }

        public static double DistanceSquared(Point point1, Point point2)
        {
            return (point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y);
        }

        internal static void GetBezierCoefficients(ref double a0, ref double a1, ref double a2, ref double a3, ref double b0, ref double b1, ref double b2, ref double b3, ref double u, ref double s, ref double z, ref double x4, ref double y4)
        {
            double num1 = a0 + u * (a1 + u * (a2 + u * a3)) - x4;
            double num2 = b1 + u * (2.0 * b2 + 3.0 * u * b3);
            double num3 = b0 + u * (b1 + u * (b2 + u * b3)) - y4;
            double num4 = a1 + u * (2.0 * a2 + 3.0 * u * a3);
            s = num1 * num1 + num3 * num3;
            z = num4 * num1 + num2 * num3;
        }

        public static bool IsEqual(this double value1, double value2)
        {
            return Math.Abs(value1 - value2) < 1E-06;
        }

        public static RotateTransform RotateTransform(Point center, double angle)
        {
            return new RotateTransform()
            {
                Angle = angle,
                CenterX = center.X,
                CenterY = center.Y
            };
        }

        public static bool BetweenOrEqual(double value, double lower, double upper)
        {
            Utils.Sort(ref lower, ref upper);
            return Utils.BetweenOrEqualSorted(value, lower, upper);
        }

        private static bool BetweenOrEqualSorted(double n, double boundary1, double boundary2)
        {
            if (boundary1 <= n)
                return n <= boundary2;
            else
                return false;
        }

        public static Vector Perpendicular(this Vector vector)
        {
            return new Vector(vector.Y, -vector.X);
        }

        public static Point ProjectPointOnLine(Point point, Point lineStart, Point lineEnd)
        {
            Vector vector = Utils.Perpendicular(new Vector(lineStart.X - lineEnd.X, lineStart.Y - lineEnd.Y));
            Point line2End = new Point(point.X + vector.X, point.Y + vector.Y);
            Point linesIntersection = Utils.FindLinesIntersection(lineStart, lineEnd, point, line2End, true);
            return Utils.Closer(point, lineStart, double.IsNaN(linesIntersection.X) ? lineEnd : Utils.Closer(point, linesIntersection, lineEnd));
        }

        public static Point FindLinesIntersection(Point line1Start, Point line1End, Point line2Start, Point line2End, bool acceptNaN = false)
        {
            Point point = acceptNaN ? new Point(double.NaN, double.NaN) : new Point(double.MinValue, double.MinValue);
            if (Math.Abs(line1Start.X - line1End.X) < 1E-06 && Math.Abs(line2Start.X - line2End.X) < 1E-06)
                return point;
            if (Math.Abs(line1Start.X - line1End.X) < 1E-06)
            {
                point.X = line1Start.X;
                point.Y = (line2Start.Y - line2End.Y) / (line2Start.X - line2End.X) * point.X + (line2Start.X * line2End.Y - line2End.X * line2Start.Y) / (line2Start.X - line2End.X);
                return point;
            }
            else if (Math.Abs(line2Start.X - line2End.X) < 1E-06)
            {
                point.X = line2Start.X;
                point.Y = (line1Start.Y - line1End.Y) / (line1Start.X - line1End.X) * point.X + (line1Start.X * line1End.Y - line1End.X * line1Start.Y) / (line1Start.X - line1End.X);
                return point;
            }
            else
            {
                double num1 = (line1Start.Y - line1End.Y) / (line1Start.X - line1End.X);
                double num2 = (line1Start.X * line1End.Y - line1End.X * line1Start.Y) / (line1Start.X - line1End.X);
                double num3 = (line2Start.Y - line2End.Y) / (line2Start.X - line2End.X);
                double num4 = (line2Start.X * line2End.Y - line2End.X * line2Start.Y) / (line2Start.X - line2End.X);
                if (Math.Abs(num1 - num3) > 1E-06 || acceptNaN)
                {
                    point.X = (num4 - num2) / (num1 - num3);
                    point.Y = num1 * (num4 - num2) / (num1 - num3) + num2;
                }
                return point;
            }
        }
    }
}
