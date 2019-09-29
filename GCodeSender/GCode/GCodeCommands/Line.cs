﻿using GCodeSender.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GCodeSender.GCode.GCodeCommands
{
	class Line : Motion
	{
		public bool Rapid;
		// PositionValid[i] is true if the corresponding coordinate of the end position was defined in the file.
		// eg. for a file with "G0 Z15" as the first line, X and Y would still be false
		public bool[] PositionValid = new bool[] { false, false, false };

		public override double Length
		{
			get
			{
				return Delta.Magnitude;
			}
		}

		public override Vector3 Interpolate(double ratio)
		{
			return Start + Delta * ratio;
		}

		public override IEnumerable<Motion> Split(double length)
		{
			if (Rapid || PositionValid.Any(isValid => !isValid))  //don't split up rapid or not fully defined motions
			{
				yield return this;
				yield break;
			}

			int divisions = (int)Math.Ceiling(Length / length);

			if (divisions < 1)
				divisions = 1;

			Vector3 lastEnd = Start;

			for (int i = 1; i <= divisions; i++)
			{
				Vector3 end = Interpolate(((double)i) / divisions);

				Line immediate = new Line();
				immediate.Start = lastEnd;
				immediate.End = end;
				immediate.Feed = Feed;
				immediate.PositionValid = new bool[] { true, true, true };

				yield return immediate;

				lastEnd = end;
			}
		}
	}
}
