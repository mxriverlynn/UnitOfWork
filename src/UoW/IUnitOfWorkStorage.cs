namespace UoW
{
	/// <summary>
	/// Provides storage for a unit of work implementation and transaction manager
	/// </summary>
	/// <remarks>
	/// It is the responsibility of the IUnitOfWorkStorage implementation to provide thread safe access to the IUnitOfWork and ITransactionManager objects to the static UnitOfWork class.
	/// </remarks>
	public interface IUnitOfWorkStorage
	{
		bool HasUnitOfWork { get; }
		bool HasTransactionManager { get; }

		void Store(IUnitOfWork uow);
		IUnitOfWork Retrieve();
		void Clear();

		void StoreTransactionManager(ITransactionManager transactionManager);
		ITransactionManager RetrieveTransactionManager();
		void ClearTransactionManager();
	}
}