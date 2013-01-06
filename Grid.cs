﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack_O_Tron
{
	public class Grid
	{
		private readonly int binWidth;
		private readonly int binHeight;
		public readonly List<Box> UsedBoxes = new List<Box>();
		private readonly List<Box> FreeBoxes = new List<Box>();
		public FreeRectChoiceHeuristic Method;

		private string[,] _stringMap;

		public Grid(int width, int height, FreeRectChoiceHeuristic method)
		{
			binWidth = width;
			binHeight = height;
			Method = method;
			var n = new Box(0, 0, width, height);
			FreeBoxes.Add(n);
		}
		
		public bool Insert(List<Box> rects)
		{
			UsedBoxes.Clear();
			while (rects.Count > 0)
			{
				int bestScore1 = int.MaxValue;
				int bestScore2 = int.MaxValue;
				int bestRectIndex = -1;
				Box bestNode = new Box();

				for (int i = 0; i < rects.Count; ++i)
				{
					int score1 = 0;
					int score2 = 0;
					Box newNode = new Box(ScoreRect(rects[i].width, rects[i].height, Method, out score1, out score2));

					if (score1 < bestScore1 || (score1 == bestScore1 && score2 < bestScore2))
					{
						bestScore1 = score1;
						bestScore2 = score2;
						bestNode = newNode;
						bestRectIndex = i;
					}
				}

				if (bestRectIndex == -1)
					return false;
					
				PlaceRect(bestNode);
				rects.RemoveAt(bestRectIndex);
			}
			return true;
		}

		public void PlaceRect(Box node)
		{
			int numRectanglesToProcess = FreeBoxes.Count;
			for (int i = 0; i < numRectanglesToProcess; ++i)
			{
				if (SplitFreeNode(FreeBoxes[i], node))
				{
					FreeBoxes.RemoveAt(i);
					--i;
					--numRectanglesToProcess;
				}
			}

			PruneFreeList();

			UsedBoxes.Add(node);
		}

		

		private Box ScoreRect(int width, int height, FreeRectChoiceHeuristic method, out int score1, out int score2)
		{
			Box newNode = new Box();
			score1 = int.MaxValue;
			score2 = int.MaxValue;
			switch (method)
			{
				case FreeRectChoiceHeuristic.RectBestShortSideFit:
					newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2);
					break;
				case FreeRectChoiceHeuristic.RectBottomLeftRule:
					newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2);
					break;
				//case FreeRectChoiceHeuristic.RectContactPointRule:
				//	newNode = FindPositionForNewNodeContactPoint(width, height, ref score1);
				//	score1 = -score1; // Reverse since we are minimizing, but for contact point score bigger is better.
				//	break;
				case FreeRectChoiceHeuristic.RectBestLongSideFit:
					newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1);
					break;
				case FreeRectChoiceHeuristic.RectBestAreaFit:
					newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2);
					break;
			}

			// Cannot fit the current rectangle.
			if (newNode == null)
			{
				score1 = int.MaxValue;
				score2 = int.MaxValue;
			}

			return newNode;
		}

		/// Computes the ratio of used surface area.
		public float Efficiency()
		{
			long usedSurfaceArea = UsedBoxes.Aggregate<Box, long>(0, (current, t) => current + t.width*t.height);

			return (float) usedSurfaceArea/(binWidth*binHeight);
		}

		public Box FindPositionForNewNodeBottomLeft(int width, int height, ref int bestY, ref int bestX)
		{
			Box bestNode = new Box();

			bestY = int.MaxValue;

			foreach (Box t in FreeBoxes)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (t.width >= width && t.height >= height)
				{
					int topSideY = t.y + height;
					if (topSideY < bestY || (topSideY == bestY && t.x < bestX))
					{
						bestNode.x = t.x;
						bestNode.y = t.y;
						bestNode.width = width;
						bestNode.height = height;
						bestY = topSideY;
						bestX = t.x;
					}
				}
				if (t.width >= height && t.height >= width)
				{
					int topSideY = t.y + width;
					if (topSideY < bestY || (topSideY == bestY && t.x < bestX))
					{
						bestNode.x = t.x;
						bestNode.y = t.y;
						bestNode.width = height;
						bestNode.height = width;
						bestY = topSideY;
						bestX = t.x;
					}
				}
			}
			return bestNode;
		}

		public Box FindPositionForNewNodeBestShortSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			Box bestNode = new Box();

			bestShortSideFit = int.MaxValue;

			foreach (Box rect in FreeBoxes)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (rect.width >= width && rect.height >= height)
				{
					int leftoverHoriz = Math.Abs(rect.width - width);
					int leftoverVert = Math.Abs(rect.height - height);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);
					int longSideFit = Math.Max(leftoverHoriz, leftoverVert);

					if (shortSideFit < bestShortSideFit || (shortSideFit == bestShortSideFit && longSideFit < bestLongSideFit))
					{
						bestNode.x = rect.x;
						bestNode.y = rect.y;
						bestNode.width = width;
						bestNode.height = height;
						bestShortSideFit = shortSideFit;
						bestLongSideFit = longSideFit;
					}
				}

				if (rect.width >= height && rect.height >= width)
				{
					int flippedLeftoverHoriz = Math.Abs(rect.width - height);
					int flippedLeftoverVert = Math.Abs(rect.height - width);
					int flippedShortSideFit = Math.Min(flippedLeftoverHoriz, flippedLeftoverVert);
					int flippedLongSideFit = Math.Max(flippedLeftoverHoriz, flippedLeftoverVert);

					if (flippedShortSideFit < bestShortSideFit ||
					    (flippedShortSideFit == bestShortSideFit && flippedLongSideFit < bestLongSideFit))
					{
						bestNode.x = rect.x;
						bestNode.y = rect.y;
						bestNode.width = height;
						bestNode.height = width;
						bestShortSideFit = flippedShortSideFit;
						bestLongSideFit = flippedLongSideFit;
					}
				}
			}
			return bestNode;
		}

		private Box FindPositionForNewNodeBestLongSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
		{
			Box bestNode = new Box();

			bestLongSideFit = int.MaxValue;

			foreach (Box rect in FreeBoxes)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (rect.width >= width && rect.height >= height)
				{
					int leftoverHoriz = Math.Abs(rect.width - width);
					int leftoverVert = Math.Abs(rect.height - height);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);
					int longSideFit = Math.Max(leftoverHoriz, leftoverVert);

					if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
					{
						bestNode.x = rect.x;
						bestNode.y = rect.y;
						bestNode.width = width;
						bestNode.height = height;
						bestShortSideFit = shortSideFit;
						bestLongSideFit = longSideFit;
					}
				}

				if (rect.width >= height && rect.height >= width)
				{
					int leftoverHoriz = Math.Abs(rect.width - height);
					int leftoverVert = Math.Abs(rect.height - width);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);
					int longSideFit = Math.Max(leftoverHoriz, leftoverVert);

					if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
					{
						bestNode.x = rect.x;
						bestNode.y = rect.y;
						bestNode.width = height;
						bestNode.height = width;
						bestShortSideFit = shortSideFit;
						bestLongSideFit = longSideFit;
					}
				}
			}
			return bestNode;
		}

		public Box FindPositionForNewNodeBestAreaFit(int width, int height, ref int bestAreaFit, ref int bestShortSideFit)
		{
			Box bestNode = new Box();


			bestAreaFit = int.MaxValue;

			foreach (Box rect in FreeBoxes)
			{
				int areaFit = rect.width*rect.height - width*height;

				// Try to place the rectangle in upright (non-flipped) orientation.
				if (rect.width >= width && rect.height >= height)
				{
					int leftoverHoriz = Math.Abs(rect.width - width);
					int leftoverVert = Math.Abs(rect.height - height);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);

					if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
					{
						bestNode.x = rect.x;
						bestNode.y = rect.y;
						bestNode.width = width;
						bestNode.height = height;
						bestShortSideFit = shortSideFit;
						bestAreaFit = areaFit;
					}
				}

				if (rect.width >= height && rect.height >= width)
				{
					int leftoverHoriz = Math.Abs(rect.width - height);
					int leftoverVert = Math.Abs(rect.height - width);
					int shortSideFit = Math.Min(leftoverHoriz, leftoverVert);

					if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
					{
						bestNode.x = rect.x;
						bestNode.y = rect.y;
						bestNode.width = height;
						bestNode.height = width;
						bestShortSideFit = shortSideFit;
						bestAreaFit = areaFit;
					}
				}
			}
			return bestNode;
		}

		/// Returns 0 if the two intervals i1 and i2 are disjoint, or the length of their overlap otherwise.
		public int CommonIntervalLength(int i1start, int i1end, int i2start, int i2end)
		{
			if (i1end < i2start || i2end < i1start)
				return 0;
			return Math.Min(i1end, i2end) - Math.Max(i1start, i2start);
		}

		public int ContactPointScoreNode(int x, int y, int width, int height)
		{
			int score = 0;

			if (x == 0 || x + width == binWidth)
				score += height;
			if (y == 0 || y + height == binHeight)
				score += width;

			foreach (Box t in UsedBoxes)
			{
				if (t.x == x + width || t.x + t.width == x)
					score += CommonIntervalLength(t.y, t.y + t.height, y, y + height);
				if (t.y == y + height || t.y + t.height == y)
					score += CommonIntervalLength(t.x, t.x + t.width, x, x + width);
			}
			return score;
		}

		public Box FindPositionForNewNodeContactPoint(int width, int height, ref int bestContactScore)
		{
			Box bestNode = new Box();

			bestContactScore = -1;

			for (int i = 0; i < FreeBoxes.Count; ++i)
			{
				// Try to place the rectangle in upright (non-flipped) orientation.
				if (FreeBoxes[i].width >= width && FreeBoxes[i].height >= height)
				{
					int score = ContactPointScoreNode(FreeBoxes[i].x, FreeBoxes[i].y, width, height);
					if (score > bestContactScore)
					{
						bestNode.x = FreeBoxes[i].x;
						bestNode.y = FreeBoxes[i].y;
						bestNode.width = width;
						bestNode.height = height;
						bestContactScore = score;
					}
				}
				if (FreeBoxes[i].width >= height && FreeBoxes[i].height >= width)
				{
					int score = ContactPointScoreNode(FreeBoxes[i].x, FreeBoxes[i].y, width, height);
					if (score > bestContactScore)
					{
						bestNode.x = FreeBoxes[i].x;
						bestNode.y = FreeBoxes[i].y;
						bestNode.width = height;
						bestNode.height = width;
						bestContactScore = score;
					}
				}
			}
			return bestNode;
		}

		public bool SplitFreeNode(Box freeNode, Box usedNode)
		{
			// Test with SAT if the rectangles even intersect.
			if (usedNode.x >= freeNode.x + freeNode.width || usedNode.x + usedNode.width <= freeNode.x ||
			    usedNode.y >= freeNode.y + freeNode.height || usedNode.y + usedNode.height <= freeNode.y)
				return false;

			if (usedNode.x < freeNode.x + freeNode.width && usedNode.x + usedNode.width > freeNode.x)
			{
				// New node at the top side of the used node.
				if (usedNode.y > freeNode.y && usedNode.y < freeNode.y + freeNode.height)
				{
					Box newNode = new Box(freeNode);
					newNode.height = usedNode.y - newNode.y;
					FreeBoxes.Add(newNode);
				}

				// New node at the bottom side of the used node.
				if (usedNode.y + usedNode.height < freeNode.y + freeNode.height)
				{
					Box newNode = new Box(freeNode);
					newNode.y = usedNode.y + usedNode.height;
					newNode.height = freeNode.y + freeNode.height - (usedNode.y + usedNode.height);
					FreeBoxes.Add(newNode);
				}
			}

			if (usedNode.y < freeNode.y + freeNode.height && usedNode.y + usedNode.height > freeNode.y)
			{
				// New node at the left side of the used node.
				if (usedNode.x > freeNode.x && usedNode.x < freeNode.x + freeNode.width)
				{
					Box newNode = new Box(freeNode);
					newNode.width = usedNode.x - newNode.x;
					FreeBoxes.Add(newNode);
				}

				// New node at the right side of the used node.
				if (usedNode.x + usedNode.width < freeNode.x + freeNode.width)
				{
					Box newNode = new Box(freeNode);
					newNode.x = usedNode.x + usedNode.width;
					newNode.width = freeNode.x + freeNode.width - (usedNode.x + usedNode.width);
					FreeBoxes.Add(newNode);
				}
			}

			return true;
		}

		private void PruneFreeList()
		{

			/// Go through each pair and remove any rectangle that is redundant.
			for (int i = 0; i < FreeBoxes.Count; ++i)
				for (int j = i + 1; j < FreeBoxes.Count; ++j)
				{
					if (Box.IsContainedIn(FreeBoxes[i], FreeBoxes[j]))
					{
						FreeBoxes.RemoveAt(i);
						--i;
						break;
					}
					if (Box.IsContainedIn(FreeBoxes[j], FreeBoxes[i]))
					{
						FreeBoxes.RemoveAt(j);
						--j;
					}
				}
		}

		public string ShortString()
		{
			var result = new StringBuilder(String.Format("Grid: {0},{1}:", binWidth, binHeight));
			result.Append(Environment.NewLine);
			result.Append(String.Format("Efficiency: {0} {1}", Efficiency(), Environment.NewLine));
			result.Append(String.Format("Rects: {0} {1}",UsedBoxes.Count, Environment.NewLine));
			result.Append(String.Format("Method: {0}", Method.ToString()));
			result.Append(Environment.NewLine);
			return result.ToString();
		}

		public override string ToString()
		{
			_stringMap = new string[binWidth,binHeight];
			foreach (var b in UsedBoxes)
			{
				for (int y = b.y; y < b.y + b.height; y++)
				{
					for (int x = b.x; x < b.x + b.width; x++)
					{
						_stringMap[x, y] = (b.ID % 10).ToString();
					}
				}
			}

			var result = new StringBuilder(String.Format("Grid: {0},{1}:", binWidth, binHeight));
			result.Append(Environment.NewLine);
			for (int x = 0; x < binWidth; x++)
			{
				for (int y = 0; y < binHeight; y++)
				{
					result.Append(String.Format(" {0} ", String.IsNullOrEmpty(_stringMap[x, y]) ? "." : _stringMap[x, y]));
				}
				result.Append(Environment.NewLine);
			}

			result.Append(String.Format("Efficiency: {0} {1}", Efficiency(), Environment.NewLine));
			result.Append(String.Format("Method: {0}", Method.ToString()));
			result.Append(Environment.NewLine);
			return result.ToString();
		}
	}
}
