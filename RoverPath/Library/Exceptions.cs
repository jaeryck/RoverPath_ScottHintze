using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoverPath.Library
{
	public class PositionException : Exception
	{
		public PositionException(string innerException = "") 
			: base("The input position is invalid. See InnerException for details.", new Exception(innerException))
		{
			
		}
	}

	public class GridException : Exception
	{
		public GridException(string innerException = "")
			: base("The input grid size is invalid. See InnerException for details.", new Exception(innerException))
		{

		}
	}

	public class SequenceException : Exception
	{
		public SequenceException(string innerException = "")
			: base("The input sequence is invalid. See InnerException for details.", new Exception(innerException))
		{

		}
	}
}