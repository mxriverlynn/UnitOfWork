namespace UoW.Specs.Mocks
{
	public class MockTransactionManager: ITransactionManager
	{

		public bool TransactionBegan;
		public bool TransactionCommitted;
		public bool TransactionRolledBack;
		public bool IsInTransaction { get; private set; }

		#region ITransactionManager Members

		public void Begin()
		{
			TransactionBegan = true;
			IsInTransaction = true;
		}

		public void Commit()
		{
			TransactionCommitted = true;
			IsInTransaction = false;
		}

		public void Rollback()
		{
			TransactionRolledBack = true;
			IsInTransaction = false;
		}

		#endregion
	}
}