using System;
using log4net;
using StructureMap;

namespace UoW
{
	public class StructureMapRepositoryFactory : IRepositoryFactory
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public tRepo GetRepository<tRepo>()
		{
			if (log.IsInfoEnabled)
				log.Info(Consts.ENTERED);

			tRepo currentRepo = default(tRepo);

			try
			{
				currentRepo = ObjectFactory.GetInstance<tRepo>();
			}
			catch (Exception ex)
			{
				if(log.IsDebugEnabled)
					log.Debug(ex);

				throw;
			}

			return currentRepo;
		}

	}
}
