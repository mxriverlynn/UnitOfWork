namespace UoW
{
	public interface IRepositoryFactory
	{
		tRepo GetRepository<tRepo>();
	}
}