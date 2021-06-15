using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RoverPath.Library
{
	public static class Extensions
	{
		public static Match RegexMatch(this string value, string pattern)
		{
			return Regex.Match(value, pattern);
		}

		public static int GetDegreesByCardinal(this string cardinal)
		{
			switch (cardinal)
			{
				case "N":
					return 0;
				case "E":
					return 90;
				case "S":
					return 180;
				case "W":
					return 270;
				default:
					throw new PositionException(string.Format("The cardinal direction: \"{0}\" is invalid.", cardinal));
			}
		}

		public static string GetCardinalByDegrees(this int degrees)
		{
			switch (degrees)
			{
				case 0:
					return "N";
				case 90:
					return "E";
				case 180:
					return "S";
				case 270:
					return "W";   
				default:
					throw new PositionException(string.Format("The degrees value: \"{0}\" is invalid.", degrees.ToString()));
			}
		}

		public static int GetX(this string coordinates)
		{
			int x = -1;
			int.TryParse(coordinates.Split(' ')[0], out x);
			return x == -1 ? throw new PositionException(string.Format("The X coordinate: \"{0}\" is invalid.", coordinates.Split(' ')[0])) : x;
		}

		public static int GetY(this string coordinates)
		{
			int y = -1;
			int.TryParse(coordinates.Split(' ')[1], out y);
			return y == -1 ? throw new PositionException(string.Format("The Y coordinate: \"{0}\" is invalid.", coordinates.Split(' ')[1])) : y;
		}

		public static string GetZ(this string coordinates)
		{
			return coordinates.Split(' ').Length < 3 || !coordinates.Split(' ')[2].RegexMatch(@"^[NSEW]$").Success ? throw new PositionException("The coordinates provided do not contain a valid \"Z\" value.") : coordinates.Split(' ')[2];
		}

	}
}