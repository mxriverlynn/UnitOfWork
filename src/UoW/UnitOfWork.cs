using System;
using log4net;

namespace UoW
{
	public class UnitOfWork
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Variables and properties

		private static IUnitOfWorkConfiguration Configuration { get; set; }

		internal static IRepositoryFactory RepositoryFactory
		{
			get
			{
				CheckForUoWConfiguration();
				return Configuration.RepositoryFactory;
			}
		}

		internal static IUnitOfWorkStorage UnitOfWorkStorage
		{
			get
			{
				CheckForUoWConfiguration();
				return Configuration.UnitOfWorkStorage;
			}
		}

		#endregion

		public static void Start(Action uowAction)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			CheckForUoWConfiguration();

			IUnitOfWork uow = null;
			try
			{
				uow = GetCurrentUnitOfWork();
				if (log.IsDebugEnabled) log.Debug("Starting uow...");
				uow.Start();

				if (log.IsDebugEnabled) log.Debug("Executing uow...");
				uowAction();
			}
			catch (Exception except)
			{
				if (log.IsErrorEnabled) log.Error(except);
				throw;
			}
			finally
			{
				if (log.IsDebugEnabled) log.Debug("Cleaning up transaction...");
				Transaction.Cleanup();

				CleanupUoW(uow);
			}
		}

		public static void Start()
		{
			CheckForUoWConfiguration();

			try
			{
				IUnitOfWork uow = GetCurrentUnitOfWork();
				if (log.IsDebugEnabled) log.Debug("Starting uow...");
				uow.Start();

				if (log.IsDebugEnabled) log.Debug("Executing uow...");
			}
			catch (Exception except)
			{
				if (log.IsErrorEnabled) log.Error(except);
				throw;
			}
		}

		public static void Stop()
		{
			if (Configuration.UnitOfWorkStorage.HasUnitOfWork)
			{
				IUnitOfWork uow = GetCurrentUnitOfWork();
				Transaction.Cleanup();
				CleanupUoW(uow);			
			}
		}

		public static IUnitOfWork GetCurrentUnitOfWork()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			CheckForUoWConfiguration();

			IUnitOfWork uow;
			if (!Configuration.UnitOfWorkStorage.HasUnitOfWork)
			{
				if (log.IsDebugEnabled) log.Debug("UoWStorage has no uow, creating uow...");
				uow = Configuration.UnitOfWorkFactory.CreateUnitOfWork();

				if (log.IsDebugEnabled) log.Debug("Storing uow in UoWStorage...");
				Configuration.UnitOfWorkStorage.Store(uow);
			}
			else
			{
				if (log.IsDebugEnabled) log.Debug("Getting uow from UoWStorage...");
				uow = Configuration.UnitOfWorkStorage.Retrieve();
			}

			if (log.IsDebugEnabled) log.Debug("Returning with uow: " + uow);
			return uow;
		}

		public static ITransactionManager GetCurrentTransactionManager()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			CheckForUoWConfiguration();

			ITransactionManager transactionManager;
			if (!Configuration.UnitOfWorkStorage.HasTransactionManager)
			{
				if (log.IsDebugEnabled) log.Debug("UoWStorage has no transaction manager, creating transaction manager...");
				transactionManager = Configuration.UnitOfWorkFactory.CreateTransactionManager();

				if (log.IsDebugEnabled) log.Debug("Storing transaction manager in UoWStorage...");
				Configuration.UnitOfWorkStorage.StoreTransactionManager(transactionManager);
			}
			else
			{
				if (log.IsDebugEnabled) log.Debug("Getting transaction manager from UoWStorage...");
				transactionManager = Configuration.UnitOfWorkStorage.RetrieveTransactionManager();
			}

			if (log.IsDebugEnabled) log.Debug("Returning with transaction manager: " + transactionManager);
			return transactionManager;
		}

		private static void CleanupUoW(IUnitOfWork uow)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			if (log.IsDebugEnabled) log.Debug("Shutting down uow...");
			uow.Shutdown(UnitOfWorkStorage);

			if (log.IsDebugEnabled) log.Debug("Done with uow shutdown, clearing UoWStorage...");
			Configuration.UnitOfWorkStorage.Clear();

			if (log.IsDebugEnabled) log.Debug("Done with cleanup");
		}

		private static void CheckForUoWConfiguration()
		{
			if (Configuration == null)
				throw new NoUnitOfWorkConfigurationException();
		}

		public static void Configure(IUnitOfWorkConfiguration configuration)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(configuration);

			Configuration = configuration;
		}

	}
}