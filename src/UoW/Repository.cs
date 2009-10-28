namespace UoW
{

	public class Repository<tRepo>
	{

		public static tRepo Do
		{
			get { return UnitOfWork.RepositoryFactory.GetRepository<tRepo>(); }
		}
	
	}
}