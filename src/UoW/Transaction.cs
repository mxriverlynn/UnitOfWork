using log4net;

namespace UoW
{
	public class Transaction
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		internal static ITransactionManager TransactionManager
		{
			get { return UnitOfWork.GetCurrentTransactionManager(); }
		}

		public static void Begin()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			if (TransactionManager == null)
			{
				if (log.IsErrorEnabled) log.Error("Cannot begin a transaction with a null TransactionManger, returning");
				return;
			}

			if (log.IsDebugEnabled) log.Debug("Starting transaction");
			TransactionManager.Begin();
		}

		public static void Commit()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			if (TransactionManager == null)
			{
				if (log.IsErrorEnabled) log.Error("Cannot commit a transaction with a null TransactionManger, returning");
				return;
			}

			if (! TransactionManager.IsInTransaction)
			{
				if (log.IsErrorEnabled) log.Error("Cannot commit without starting a transaction, returning");
				return;
			}

			if (log.IsDebugEnabled) log.Debug("Committing...");
			TransactionManager.Commit();
			if (log.IsDebugEnabled) log.Debug("Done committing");
		}

		public static void Rollback()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			if (TransactionManager == null)
			{
				if (log.IsDebugEnabled) log.Debug("TransactionManger is null, returning");
				return;
			}

			if (!TransactionManager.IsInTransaction)
			{
				if (log.IsDebugEnabled) log.Debug("No transaction started, returning");
				return;
			}

			if (log.IsDebugEnabled) log.Debug("Rolling back...");
			TransactionManager.Rollback();
			if (log.IsDebugEnabled) log.Debug("Done rolling back");
		}

		public static void Cleanup()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			Rollback();
		}

	}

}