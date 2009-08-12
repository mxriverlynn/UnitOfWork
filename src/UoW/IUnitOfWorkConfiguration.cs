namespace UoW
{
	public interface IUnitOfWorkConfiguration
	{
		IUnitOfWorkFactory UnitOfWorkFactory { get; }
		IUnitOfWorkStorage UnitOfWorkStorage { get; }
	}
}