using System;

namespace UoW
{

	public class NoUnitOfWorkConfigurationException : Exception
	{

		public NoUnitOfWorkConfigurationException() : base("UnitOfWork Has Not Been Configured.") { }
	}

}