using System.Collections.Generic;
using UoW.Specs.Mocks;

namespace UoW.Specs.Mocks
{
	public class MockUoWFactory: IUnitOfWorkFactory
	{
		private readonly MockUnitOfWork _uow;

		public bool UoWRetrieved;
		private int _createUoWCalled;

		public IList<MockTransactionManager> GeneratedTransactionManagers { get; private set; }

		public MockUoWFactory(MockUnitOfWork uow)
		{
			GeneratedTransactionManagers = new List<MockTransactionManager>();
			_uow = uow;
		}

		public int CreateUoWCalled
		{
			get { return _createUoWCalled; }
		}

		public IUnitOfWork CreateUnitOfWork()
		{
			_createUoWCalled++;
			UoWRetrieved = true;
			return _uow;
		}

		public ITransactionManager CreateTransactionManager()		
		{
			MockTransactionManager transactionManager = new MockTransactionManager();
			GeneratedTransactionManagers.Add(transactionManager);
			return transactionManager;
		}
	}
}