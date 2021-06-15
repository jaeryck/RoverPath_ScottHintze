using RoverPath.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RoverPath.Models
{
	public class RoverModels
	{
		
	}

	public class Heading
	{
		public string Cardinal { get; set; }
		public int Degrees { get; set; }
		public int Offset { get; set; }

		public Heading(string cardinal, int degrees, int offset)
		{
			Cardinal = cardinal;
			Degrees = degrees;
			Offset = offset;
		}
	}

	/// <summary>
	/// The Grid class contains the input grid (X, Y) information.
	/// </summary>
	public class Grid
	{
		public int GridX { get; set; }
		public int GridY { get; set; }

		public Grid(string gridInput)
		{
			var gridCheck = gridInput.RegexMatch(@"^[0-9]+[ ][0-9]+$");
			if (!gridCheck.Success)
			{
				string badCoords = string.Empty;
				foreach (Group coordMatch in gridCheck.Groups)
				{
					if (!coordMatch.Success)
					{
						badCoords += coordMatch.Value + " ";
					}
				}
				throw new GridException(string.Format("The grid coordinate(s) {0} is/are invalid.", badCoords.TrimEnd(',')));
			}

			//since the grid is 0-based, the "size" is actually x-1;
			GridX = gridInput.GetX() - 1;
			//since the grid is 0-based, the "size" is actually y-1;
			GridY = gridInput.GetY() - 1;
		}
	}

	/// <summary>
	/// The RoverPosition class contains the input rover position and heading.
	/// </summary>
	public class RoverPosition
	{
		public int RoverX { get; set; }
		public int RoverY { get; set; }
		public string RoverHeading { get; set; }

		private int headingDegrees { get; set; }

		public RoverPosition(string positionInput)
		{
			var positionCheck = positionInput.RegexMatch(@"^[0-9]+[ ][0-9]+[ ][NSEW]$");
			if (!positionCheck.Success)
			{
				string badCoords = string.Empty;
				foreach (Group coordMatch in positionCheck.Groups)
				{
					if (!coordMatch.Success)
					{
						badCoords += coordMatch.Value + " ";
					}
				}
				throw new PositionException(string.Format("The position coordinate(s) {0} is/are invalid or exceed(s) the input grid size.", badCoords.TrimEnd(',')));
			}

			RoverX = positionInput.GetX();
			RoverY = positionInput.GetY();
			RoverHeading = positionInput.GetZ();
			headingDegrees = RoverHeading.GetDegreesByCardinal();
		}

		public void Move(string sequence, string gridInput, RoverPosition occupiedSpace)
		{
			//modify the position properties based on the input command and current heading. S, E = -1, N, W = +1
			if (!ValidateSequence(sequence))
				return;

			var grid = new Grid(gridInput);
			for (int i = 0; i < sequence.Length; i++)
			{
				string command = sequence[i].ToString();
				var newPosition = new RoverPosition(string.Format("{0} {1} {2}", RoverX, RoverY, RoverHeading));
				switch (command.ToString())
				{
					case "L":
						newPosition.headingDegrees = headingDegrees == 0 ? 270 : headingDegrees - 90;
						break;
					case "R":
						newPosition.headingDegrees = headingDegrees == 270 ? 0 : headingDegrees + 90;
						break;
					case "M":
						switch (headingDegrees)
						{
							case 0:
								if (RoverY + 1 > grid.GridY)
									throw new SequenceException(string.Format("The sequence command {0} at position {1} exceeds the Northern grid boundary.", command, i + 1));
								newPosition.RoverY++;
								break;
							case 90:
								if (RoverX + 1 > grid.GridX)
									throw new SequenceException(string.Format("The sequence command {0} at position {1} exceeds the Eastern grid boundary.", command, i + 1));
								newPosition.RoverX++;
								break;
							case 180:
								if (RoverY - 1 < 0)
									throw new SequenceException(string.Format("The sequence command {0} at position {1} exceeds the Southern grid boundary.", command, i + 1));
								newPosition.RoverY--;
								break;
							case 270:
								if (RoverX - 1 < 0)
									throw new SequenceException(string.Format("The sequence command {0} at position {1} exceeds the Western grid boundary.", command, i + 1));
								newPosition.RoverX--;
								break;
							default:
								break;
						}
						break;
					default:
						break;
				}
				if (newPosition.RoverX != occupiedSpace.RoverX && newPosition.RoverY != occupiedSpace.RoverY)
				{
					RoverX = newPosition.RoverX;
					RoverY = newPosition.RoverY;
					RoverHeading = newPosition.headingDegrees.GetCardinalByDegrees();
					headingDegrees = newPosition.headingDegrees;
				}
				else
				{
					throw new PositionException(string.Format("The sequence command {0} at index {1} will occupy the same current position as the other rover.", command, i + 1));
				}
			}

			RoverHeading = headingDegrees.GetCardinalByDegrees();
		}

		private bool ValidateSequence(string sequenceInput)
		{
			var sequenceCheck = sequenceInput.RegexMatch(@"^[LRM]+$|^$");
			if (!sequenceCheck.Success)
			{
				string badCommands = string.Empty;
				foreach (Group commandMatch in sequenceCheck.Groups)
				{
					if (!commandMatch.Success)
					{
						badCommands += commandMatch.Value + " ";
					}
				}
				throw new SequenceException(string.Format("The sequence command(s) {0} is/are invalid or exceed(s) the input grid size.", badCommands.TrimEnd(',')));
			}
			return true;
		}
	}

	/// <summary>
	/// The Rover class is used to track the rover information.
	/// </summary>
	public class Rover
	{
		public int RoverNumber { get; set; }
		public RoverPosition Position { get; set; }

		public Rover(int roverNumber, RoverPosition intialPosition)
		{
			RoverNumber = roverNumber;
			Position = intialPosition;
		}
	}
}