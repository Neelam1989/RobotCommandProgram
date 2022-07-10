using System;

public class Program
{
	public static void Main()
	{
		Robot robot = new Robot();

		robot.ExecuteCommand("PLACE", "1,2,EAST");
		robot.ExecuteCommand("MOVE", null);
		robot.ExecuteCommand("MOVE", null);
		robot.ExecuteCommand("LEFT", null);
		robot.ExecuteCommand("MOVE", null);

		Console.WriteLine(robot.ExecuteCommand("REPORT", null));
	}
}

public class Robot
{
	private int? X;
	private int? Y;
	private Directions? RobotDirection;

	public string ExecuteCommand(string command, string parameters)
	{
		switch (command.ToUpper())
		{
			case "PLACE":
				ExecutePlace(parameters);
				break;
			case "MOVE":
				ExecuteMove();
				break;
			case "LEFT":
				ExecuteLeft();
				break;
			case "RIGHT":
				ExecuteRight();
				break;
			case "REPORT":
				return GetReport();
			default:
				throw new ArgumentException("Unknown command");
		}

		return null;
	}

	private void ExecutePlace(String parameters)
	{
		Position position = Position.ParsePositionParameters(parameters);

		if (!RobotDirection.HasValue && !position.RobotDirection.HasValue)
		{
			throw new ArgumentException("Invalid command parameters. Direction must be specified");
		}

		if (!IsValidPosition(position.X, position.Y, position.RobotDirection))
		{
			throw new ArgumentException("Invalid command. Robot isn't placed correctly");
		}

		X = position.X;
		Y = position.Y;

		if (position.RobotDirection.HasValue)
		{
			RobotDirection = position.RobotDirection;
		}
	}

	private void ExecuteLeft()
	{
		ValidateCurrentPosition();

		this.RobotDirection = (Directions)((((int)RobotDirection.Value) - 1 + 4) % 4);
	}

	private void ExecuteRight()
	{
		ValidateCurrentPosition();

		RobotDirection = (Directions)((((int)RobotDirection.Value) + 1) % 4);
	}

	private void ExecuteMove()
	{
		ValidateCurrentPosition();

		int newXPOS = X.Value;
		int newYPOS = Y.Value;

		if (RobotDirection.Equals(Directions.NORTH))
		{
			newYPOS++;
		}

		if (RobotDirection.Equals(Directions.WEST))
		{
			newXPOS--;
		}

		if (RobotDirection.Equals(Directions.SOUTH))
		{
			newYPOS--;
		}

		if (RobotDirection.Equals(Directions.EAST))
		{
			newXPOS++;
		}

		if (!IsValidPosition(newXPOS, newYPOS, RobotDirection))
		{
			throw new ArgumentException("Invalid command. This move will place robot in an invalid position");
		}

		X = newXPOS;
		Y = newYPOS;
	}

	private string GetReport()
	{
		return string.Format("{0},{1},{2}", X, Y, RobotDirection.ToString());
	}

	private void ValidateCurrentPosition()
	{
		if (!IsValidPosition(X, Y, RobotDirection))
		{
			throw new ArgumentException("Invalid command. Robot isn't placed correctly");
		}
	}

	private bool IsValidPosition(int? x, int? y, Directions? direction)
	{
		return x.HasValue && y.HasValue && direction.HasValue
			&& x >= 0 && x < 6
			&& y >= 0 && y < 6;
	}
}

public class Position
{
	public int X { get; private set; }
	public int Y { get; private set; }
	public Directions? RobotDirection { get; private set; }

	public static Position ParsePositionParameters(String parameters)
	{
		Position position = new Position();
		string[] paramArray = parameters.Split(',');

		if (paramArray.Length < 2 || paramArray.Length > 3)
		{
			throw new ArgumentException("Invalid command parameters. Position coordinates must be specified");
		}

		position.X = int.Parse(paramArray[0]);
		position.Y = int.Parse(paramArray[1]);

		if (paramArray.Length > 2)
		{
			position.RobotDirection = (Directions)Enum.Parse(typeof(Directions), paramArray[2]);
		}

		return position;
	}
}

public enum Directions
{
	NORTH = 0,
	EAST = 1,
	SOUTH = 2,
	WEST = 3,
}


