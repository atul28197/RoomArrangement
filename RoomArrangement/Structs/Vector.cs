﻿using System;

namespace RoomArrangement
{
	// Replicating shit from the Rhino SDK for my purposes
	struct Vector
	{
		public double X { get; set; }
		public double Y { get; set; }

		// Use Doubles instead of Ints so for more precision and stuff
		// We'll see how that plays out when implementing a Move method.
		public Vector(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}

		public Vector(Vector v)
		{
			X = v.X;
			Y = v.Y;
		}

		public Vector(Point p)
		{
			X = p.X;
			Y = p.Y;
		}

		public Vector(Point p1, Point p2)
			: this(p2 - p1)
		{
		}

		public double Length => Math.Sqrt((X * X) + (Y * Y));
		public double Angle => Math.Atan(Y / X);

		public override string ToString() => $"V({X}, {Y})";

		#region Fun Stuff
		// Addition and subtraction.
		public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y);
		public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.X - v2.X, v1.Y - v2.Y);
		public static Vector operator *(Vector v, double d) => new Vector(v.X * d, v.Y * d);
		public static Vector operator /(Vector v, double d) => new Vector(v.X / d, v.Y / d);

		// Equality
		public static bool operator ==(Vector v1, Vector v2) => v1.Equals(v2);
		public static bool operator !=(Vector v1, Vector v2) => !v1.Equals(v2);

		public bool Equals(Vector v) => (X == v.X) && (Y == v.Y);
		public override bool Equals(object obj) => obj is Vector && this == (Vector)obj;
		public override int GetHashCode() => X.GetHashCode() ^ (13 * Y.GetHashCode());

		// Negation
		public static Vector operator -(Vector v) => new Vector(-v.X, -v.Y);
		#endregion
	}
}