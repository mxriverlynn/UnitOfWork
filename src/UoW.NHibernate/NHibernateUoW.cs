using System;
using log4net;
using NHibernate;

namespace UoW.NHibernate
{
	public class NHibernateUoW: IUnitOfWork
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Variables and properties

		private ISession NHibernateSession { get; set; }

		internal static ISession CurrentSession
		{
			get
			{
				if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

				NHibernateUoW uow = UnitOfWork.GetCurrentUnitOfWork() as NHibernateUoW;

				if (uow == null || uow.NHibernateSession == null)
					throw new NoUnitOfWorkException();

				if (log.IsDebugEnabled) log.Debug(uow.NHibernateSession);
				return uow.NHibernateSession; 
			}
		}

		#endregion

		public void Start()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			NHibernateSession = NHibernateConfig.GetSession();

			if (log.IsDebugEnabled) log.Debug(NHibernateSession);
		}

		public void Shutdown(IUnitOfWorkStorage storage)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			if( null == storage )
				throw new ArgumentNullException("storage", "Cannot clear TransactionManager without access to UoW storage");

			ITransactionManager transMan = storage.RetrieveTransactionManager();
			if( null != transMan )
			{
				if( transMan.IsInTransaction )
				{
					if (log.IsErrorEnabled) log.Error("Shutting down with an open transaction!  The transaction is being rolled back");
					transMan.Rollback();					
				}

				if (log.IsDebugEnabled) log.Debug("Clearing transaction manager...");
				storage.ClearTransactionManager();
			}

			if (NHibernateSession != null && NHibernateSession.IsOpen)
			{
				if (log.IsDebugEnabled) log.Debug("Closing session...");
				NHibernateSession.Close();

				if (log.IsDebugEnabled) log.Debug("Disposing session...");
				NHibernateSession.Dispose();

				if (log.IsDebugEnabled) log.Debug("Done");
			}
		}

	}
}