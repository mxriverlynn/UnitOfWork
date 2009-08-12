namespace UoW
{
	/// <summary>
	/// Provides transactional support for an IUnitOfWork implementation
	/// </summary>
	/// <remarks>
	/// It is the responsibility of the implementation to handle (or not handle) nested transactions, the Transaction class using your implementation makes no assumptions about implementors ability or lack thereof to do nested transactions and allows Begin() to be invoked multiple times before a Commit() or RollBack().
	/// </remarks>
	public interface ITransactionManager
	{
		bool IsInTransaction { get; }

		void Begin();
		void Commit();
		void Rollback();
	}
}