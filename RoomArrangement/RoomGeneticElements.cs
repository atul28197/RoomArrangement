﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using GAF;

namespace RoomArrangement
{
	static class GACompanions
	{
		public static double CalculateFitness(Chromosome c)
		{
			// Assuming each chromosome represents a certain arrangmenet of THREE rooms
			// The chrome will have, for each room:
			// 4 bits for X location , 4 bits for Y location , 1 bit for Orientation
			//
			// Since we have three rooms for the proof of concept, each chromosome
			// will be 27 bits long. TWENTY SEVEN
			//
			// Each 9 bits is one room. A loop through the chromose should do it.
			//
			// Example Chromosome:	000100101001101010110101101
			// First Room:		000100101
			// Second Room:		001101010
			// Third Room:		110101101

			var fitnessList = new List<double>();

			// Adjusting the Rooms
			for (int i = 0; i < c.Count; i += 9)
			{
				int x = Convert.ToInt32(c.ToBinaryString(i, 4), 2);
				int y = Convert.ToInt32(c.ToBinaryString(i + 4, 4), 2);
				int oTemp = Convert.ToInt32(c.ToBinaryString(i + 8, 1), 2);

				bool o = Convert.ToBoolean(oTemp);

				var j = i / 9;

				Database.List[j].Adjust(x, y, o);
			}

			// Evaluate fitness for every couple so it is a fraction of 1 (1 being perfectly adjacent)
			// then add the individual fitness value to fitnessList
			//
			// So each fitness value is	1 / (1 + fValue)
			// Which becomes smaller the large fValue is. But if fValue == 0 then it becomes 1.


			// Related rooms logic
			for (int i = 0; i < Database.Count; i++)
			{
				// Using double because all the numbers will factor into fValue, which has to be a double.
				var r1 = Database.List[i];
				double rec1X = r1.Space.XDimension;
				double rec1Y = r1.Space.YDimension;
				double cnt1X = r1.Center.X;
				double cnt1Y = r1.Center.Y;

				for (int j = 0; j < r1.AdjacentRooms.Count; j++)
				{

					double fValue;

					var r2 = r1.AdjacentRooms[j];
					double rec2X = r2.Space.XDimension;
					double rec2Y = r2.Space.YDimension;
					double cnt2X = r2.Center.X;
					double cnt2Y = r2.Center.Y;

					var xDistance = Abs(cnt1X - cnt2X);
					var yDistance = Abs(cnt1Y - cnt2Y);

					var xSize = (rec1X / 2) + (rec2X / 2);
					var ySize = (rec1Y / 2) + (rec2Y / 2);

					if (xDistance > xSize)
					{
						fValue = 1 - ((xDistance - xSize) / xDistance);
						fitnessList.Add(fValue);
					}
					else if (xDistance == xSize)
					{
						fitnessList.Add(1);
					}

					if (yDistance > ySize)
					{
						fValue = 1 - ((yDistance - ySize) / yDistance);
						fitnessList.Add(fValue);
					}
					else if (yDistance == ySize)
					{
						fitnessList.Add(1);
					}

				}
			}

			// Intersection Logic
			for (int i = 0; i < Database.Count; i++)
			{
				// Using double because all the numbers will factor into fValue, which is a double.
				var r1 = Database.List[i];
				double rec1X = r1.Space.XDimension;
				double rec1Y = r1.Space.YDimension;
				double cnt1X = r1.Center.X;
				double cnt1Y = r1.Center.Y;

				for (int j = 0; j < Database.Count; j++)
				{

					if (j != i)
					{
						double fValue;

						var r2 = Database.List[j];
						double rec2X = r2.Space.XDimension;
						double rec2Y = r2.Space.YDimension;
						double cnt2X = r2.Center.X;
						double cnt2Y = r2.Center.Y;

						var xDistance = Abs(cnt1X - cnt2X);
						var yDistance = Abs(cnt1Y - cnt2Y);

						var xSize = (rec1X / 2) + (rec2X / 2);
						var ySize = (rec1Y / 2) + (rec2Y / 2);

						if (xDistance < xSize && yDistance < ySize)
						{
							var x = 1 - ((xSize - xDistance) / xSize);
							var y = 1 - ((ySize - yDistance) / ySize);

							// fValue = x < y ? x : y;
							// fValue = x * y;
							fValue = (x + y) / 2;
							fitnessList.Add(fValue);


						}
						else
						{
							fitnessList.Add(1);
						}
					}
				}
			}

			fitnessList.Add(1d);
			//return fitnessList.Average();

			double fitness = 1;
			foreach (double d in fitnessList)
				fitness *= d;
			return fitness; 
		}

		public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
		{
			var a = currentGeneration > 1000;
			var b = population.MaximumFitness == 1;

			return (a || b);
		}
	}
}
