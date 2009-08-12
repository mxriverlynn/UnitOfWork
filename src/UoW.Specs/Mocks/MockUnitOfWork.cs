using UoW;

namespace UoW.Specs.Mocks
{
	public class MockUnitOfWork : IUnitOfWork
	{
		public bool WasStarted;
		public bool WasStopped;

		#region IUnitOfWork Members

		public void Start()
		{
			WasStarted = true;
		}

		public void Shutdown(IUnitOfWorkStorage storage)
		{
			WasStopped = true;
		}

		#endregion
	}
}