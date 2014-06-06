using System;
using System.Collections;
using System.Windows;

namespace PointExtensions
{
    public static class PointExtensions
    {
        private static readonly Point emtpy = new Point();

        static PointExtensions()
        {
        }

        /// <summary>
        /// Determines whether the specified point is empty.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// <c>True</c> if the specified point is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(this Point point)
        {
            return point == PointExtensions.emtpy;
        }

        /// <summary>
        /// Determines whether [is X between] [the specified point].
        /// </summary>
        /// <param name="point">The point.</param><param name="firstPoint">The first point.</param><param name="secondPoint">The second point.</param>
        /// <returns>
        /// <c>true</c> if [is X between] [the specified point]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsXBetween(this Point point, Point firstPoint, Point secondPoint)
        {
            if (firstPoint.X <= point.X && point.X <= secondPoint.X)
                return true;
            if (secondPoint.X <= point.X)
                return point.X <= firstPoint.X;
            else
                return false;
        }

        /// <summary>
        /// Determines whether [is Y between] [the specified point].
        /// </summary>
        /// <param name="point">The point.</param><param name="firstPoint">The first point.</param><param name="secondPoint">The second point.</param>
        /// <returns>
        /// <c>true</c> if [is Y between] [the specified point]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsYBetween(this Point point, Point firstPoint, Point secondPoint)
        {
            if (firstPoint.Y <= point.Y && point.Y <= secondPoint.Y)
                return true;
            if (secondPoint.Y <= point.Y)
                return point.Y <= firstPoint.Y;
            else
                return false;
        }

        /// <summary>
        /// Snaps a point by changing the X and Y coordinates to the closest value dividable by the snapping value.
        /// </summary>
        /// <param name="point">The point.</param><param name="snapX">The horizontal snapping value.</param>/// <param name="snapY">The vertical snapping value.</param>
        /// <returns>
        /// Returns the snapped point.
        /// </returns>
        public static Point Snap(this Point point, int snapX, int snapY)
        {
            double num1 = point.X % (double)snapX;
            double num2 = point.Y % (double)snapY;
            return new Point(point.X - num1 + (num1 > (double)(snapX / 2) ? (double)snapX : 0.0), point.Y - num2 + (num2 > (double)(snapY / 2) ? (double)snapY : 0.0));
        }

        /// <summary>
        /// Adds the specified point and vector together.
        /// </summary>
        /// <seealso cref="T:System.Windows.Vector">The Vector struct and its operations.</seealso><param name="point">A point.</param><param name="vector">A vector.</param>
        /// <returns>
        /// The augmented point.
        /// </returns>
        public static Point Add(this Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }

        /// <summary>
        /// Adds the specified points together.
        /// </summary>
        /// <param name="point">A point.</param><param name="point2">The point2.</param>
        /// <returns>
        /// The augmented point.
        /// </returns>
        /// <seealso cref="T:System.Windows.Vector">The Vector struct and its operations.</seealso>
        public static Point Add(this Point point, Point point2)
        {
            return new Point(point.X + point2.X, point.Y + point2.Y);
        }

        /// <summary>
        /// Subtracts point2 from point1.
        /// </summary>
        public static Point Substract(this Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        /// <summary>
        /// Subtracts the specified point1.
        /// 
        /// </summary>
        public static Vector Subtract(this Point point1, Point point2)
        {
            return new Vector(point1.X - point2.X, point1.Y - point2.Y);
        }

        /// <summary>
        /// Multiplies the point with the specified multiplier.
        /// </summary>
        /// <param name="point">The point.</param><param name="multiplier">The multiplier.</param>
        /// <returns/>
        public static Point Multiply(this Point point, double multiplier)
        {
            return new Point(point.X * multiplier, point.Y * multiplier);
        }

        /// <summary>
        /// Divides the point with the specified factor.
        /// 
        /// </summary>
        /// <param name="point">The point.</param><param name="factor">The factor.</param>
        /// <returns/>
        public static Point Divide(this Point point, double factor)
        {
            return new Point(point.X / factor, point.Y / factor);
        }

        /// <summary>
        /// Rotates the point.
        /// </summary>
        /// <param name="point">The point.</param><param name="pivot">The pivot.</param><param name="angle">The angle.</param>
        /// <returns/>
        public static Point Rotate(this Point point, Point pivot, double angle)
        {
            if (!Utils.IsEqual(angle, 0.0))
                return Utils.RotateTransform(pivot, angle).Transform(point);
            else
                return point;
        }

        /// <summary>
        /// Distance to rectangle.
        /// </summary>
        /// <param name="point">The point.</param><param name="rect">The rectangle.</param>
        public static Point DistancePoint(this Point point, Rect rect)
        {
            return new Point(PointExtensions.DistanceSelect(point.X, rect.Left, rect.Right), PointExtensions.DistanceSelect(point.Y, rect.Top, rect.Bottom));
        }

        /// <summary>
        /// Distances to rectangle.
        /// </summary>
        /// <param name="point">The point.</param><param name="rect">The rectangle.</param>
        public static double Distance(this Point point, Rect rect)
        {
            Point endPoint = PointExtensions.DistancePoint(point, rect);
            return PointExtensions.Distance(point, endPoint);
        }

        /// <summary>
        /// Returns the distance of the point to the origin.
        /// 
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns/>
        public static double Distance(this Point point)
        {
            return PointExtensions.Distance(point, new Point(0.0, 0.0));
        }

        /// <summary>
        /// Returns the distance between the specified points.
        /// 
        /// </summary>
        /// <param name="startPoint">The start point.</param><param name="endPoint">The end point.</param>
        /// <returns/>
        public static double Distance(this Point startPoint, Point endPoint)
        {
            return Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2.0) + Math.Pow(startPoint.Y - endPoint.Y, 2.0));
        }

        internal static double DistanceToBezierCurve(this Point point, IList bezierPoints)
        {
            double num1 = 1000000.0;
            int num2 = bezierPoints.Count / 3;
            for (int index1 = 0; index1 < num2; ++index1)
            {
                double x1 = ((Point)bezierPoints[index1 * 3]).X;
                double y1 = ((Point)bezierPoints[index1 * 3]).Y;
                double x2 = ((Point)bezierPoints[index1 * 3 + 1]).X;
                double y2 = ((Point)bezierPoints[index1 * 3 + 1]).Y;
                double x3 = ((Point)bezierPoints[index1 * 3 + 2]).X;
                double y3 = ((Point)bezierPoints[index1 * 3 + 2]).Y;
                double x4 = ((Point)bezierPoints[index1 * 3 + 3]).X;
                double y4 = ((Point)bezierPoints[index1 * 3 + 3]).Y;
                double a3 = (x4 - x1 + 3.0 * (x2 - x3)) / 8.0;
                double b3 = (y4 - y1 + 3.0 * (y2 - y3)) / 8.0;
                double a2 = (x4 + x1 - x2 - x3) * 3.0 / 8.0;
                double b2 = (y4 + y1 - y2 - y3) * 3.0 / 8.0;
                double a1 = (x4 - x1) / 2.0 - a3;
                double b1 = (y4 - y1) / 2.0 - b3;
                double a0 = (x4 + x1) / 2.0 - a2;
                double b0 = (y4 + y1) / 2.0 - b2;
                double x5 = point.X;
                double y5 = point.Y;
                double s = 0.0;
                double z = 0.0;
                double num3 = 0.0;
                double num4 = -1.0;
                double num5 = 0.0;
                double u = -1.0;
                while (u < 1.0)
                {
                    Utils.GetBezierCoefficients(ref a0, ref a1, ref a2, ref a3, ref b0, ref b1, ref b2, ref b3, ref u, ref s, ref z, ref x5, ref y5);
                    if (Math.Abs(s) < 1E-06)
                    {
                        num4 = u;
                        num5 = z;
                        num3 = s;
                        break;
                    }
                    else
                    {
                        if (Math.Abs(u + 1.0) < 1E-06)
                        {
                            num4 = u;
                            num5 = z;
                            num3 = s;
                        }
                        if (s < num3)
                        {
                            num4 = u;
                            num5 = z;
                            num3 = s;
                        }
                        u += 2.0 / 9.0;
                    }
                }
                if (Math.Abs(num3) > 1E-06)
                {
                    u = num4 + 2.0 / 9.0;
                    if (u > 1.0)
                        u = 7.0 / 9.0;
                    for (int index2 = 0; index2 < 20; ++index2)
                    {
                        Utils.GetBezierCoefficients(ref a0, ref a1, ref a2, ref a3, ref b0, ref b1, ref b2, ref b3, ref u, ref s, ref z, ref x5, ref y5);
                        if (Math.Abs(s) >= 1E-06 && Math.Abs(z) >= 1E-06)
                        {
                            double num6 = u;
                            double num7 = z;
                            double num8 = num7 - num5;
                            u = Math.Abs(num8) <= 1E-06 ? (num4 + num6) / 2.0 : (num7 * num4 - num5 * num6) / num8;
                            if (u > 1.0)
                                u = 1.0;
                            else if (u < -1.0)
                                u = -1.0;
                            if (Math.Abs(u - num6) >= 1E-06)
                            {
                                num4 = num6;
                                num5 = num7;
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                if (num1 > Math.Sqrt(s))
                    num1 = Math.Sqrt(s);
                if (Math.Abs(num1) < 1E-06)
                    return 0.0;
            }
            return num1;
        }

        internal static double DistanceToPolyline(this Point point, IList polyline)
        {
            int closestSegmentToPoint = 0;
            return PointExtensions.DistanceToPolyline(point, polyline, ref closestSegmentToPoint);
        }

        internal static double DistanceToPolyline(this Point point, IList polyline, ref int closestSegmentToPoint)
        {
            double num1 = double.MaxValue;
            closestSegmentToPoint = 0;
            for (int index = 0; index < polyline.Count - 1; ++index)
            {
                Point a = (Point)polyline[index];
                Point b = (Point)polyline[index + 1];
                double num2 = PointExtensions.DistanceToLine(point, a, b);
                if (num2 < num1)
                {
                    num1 = num2;
                    closestSegmentToPoint = index;
                }
            }
            return num1;
        }

        internal static double DistanceToLineSegment(this Point point, IList polyline, double delta)
        {
            double num1 = double.NaN;
            for (int index = 0; index < polyline.Count - 1; ++index)
            {
                Point point1 = (Point)polyline[index];
                Point point2 = (Point)polyline[index + 1];
                double num2 = PointExtensions.DistanceToLine(point, point1, point2);
                bool flag = Math.Abs(point1.X - point2.X) < 1E-06 ? PointExtensions.IsYBetween(point, point1, point2) : PointExtensions.IsXBetween(point, point1, point2);
                if (num2 < delta && flag || RectExtensions.AroundPoint(point1, point, delta))
                {
                    num1 = num2;
                    break;
                }
            }
            return num1;
        }

        internal static double DistanceToLine(this Point p, Point a, Point b)
        {
            return PointExtensions.Distance(p, Utils.ProjectPointOnLine(p, a, b));
        }

        internal static double DistanceToLineSquared(this Point p, Point a, Point b)
        {
            if (a == b)
                return Utils.DistanceSquared(p, a);
            double num1 = b.X - a.X;
            double num2 = b.Y - a.Y;
            double num3 = (p.Y - a.Y) * num1 - (p.X - a.X) * num2;
            return num3 * num3 / (num1 * num1 + num2 * num2);
        }

        internal static double DistanceToSegmentSquared(this Point p, Point a, Point b)
        {
            if (a == b)
                return Utils.DistanceSquared(p, a);
            double num1 = b.X - a.X;
            double num2 = b.Y - a.Y;
            if ((p.X - a.X) * num1 + (p.Y - a.Y) * num2 < 0.0)
                return Utils.DistanceSquared(a, p);
            if ((b.X - p.X) * num1 + (b.Y - p.Y) * num2 < 0.0)
                return Utils.DistanceSquared(b, p);
            else
                return PointExtensions.DistanceToLineSquared(p, a, b);
        }

        private static double DistanceSelect(double pointX, double rectX1, double rectX2)
        {
            double nearestValue;
            double otherValue;
            Utils.Closer(pointX, rectX1, rectX2, out nearestValue, out otherValue);
            if (!Utils.BetweenOrEqual(pointX, nearestValue, otherValue))
                return nearestValue;
            else
                return pointX;
        }
    }
}
