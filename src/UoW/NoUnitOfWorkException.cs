using System;

namespace UoW
{
	public class NoUnitOfWorkException: Exception
	{

		public NoUnitOfWorkException() : base("UnitOfWork Not Found") { }

	}
}
