namespace UoW
{
	public interface IUnitOfWorkFactory
	{

		IUnitOfWork CreateUnitOfWork();
		ITransactionManager CreateTransactionManager();

	}
}