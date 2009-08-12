using log4net;

namespace UoW.NHibernate
{
	public class NHibernateUoWFactory : IUnitOfWorkFactory
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public IUnitOfWork CreateUnitOfWork()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			return new NHibernateUoW();
		}

		public ITransactionManager CreateTransactionManager()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			return new NHibernateTransMan(NHibernateUoW.CurrentSession);
		}
	}
}