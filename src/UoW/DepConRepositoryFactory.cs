using log4net;
using MAT.DependencyInjection;

namespace UoW
{

	public class DepConRepositoryFactory: IRepositoryFactory
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public tRepo GetRepository<tRepo>()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			return DepCon.GetDependency<tRepo>();
		}

	}

}