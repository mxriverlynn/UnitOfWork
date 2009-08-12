namespace UoW
{
	public interface IUnitOfWork
	{
		void Start();
		void Shutdown(IUnitOfWorkStorage storage);
	}
}