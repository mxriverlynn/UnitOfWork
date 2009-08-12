using System;
using log4net;

namespace UoW
{
	public class ThreadStaticUnitOfWorkStorage : IUnitOfWorkStorage
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Vars

		[ThreadStatic]
		private static IUnitOfWork _currentUnitOfWork;
		private static readonly object _unitOfWorkLock = new object();

		[ThreadStatic]
		private static ITransactionManager _currentTransactionManager;
		private static readonly object _transactionManagerLock = new object();

		#endregion

		#region IUnitOfWorkStorage Members

		public bool HasUnitOfWork 
		{
			get
			{
				if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

				bool hasUnitOfWork;
				lock(_unitOfWorkLock)
				{
					hasUnitOfWork = (_currentUnitOfWork != null);
				}
				return hasUnitOfWork;
			}
		}
		
		public void Store(IUnitOfWork uow)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			lock (_unitOfWorkLock)
			{
				if (_currentUnitOfWork == null)
					_currentUnitOfWork = uow;
			}
		}

		public void Clear()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			
			lock (_unitOfWorkLock)
			{
				_currentUnitOfWork = null;
			}
		}
							
		public IUnitOfWork Retrieve()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			IUnitOfWork uow;
			lock(_currentUnitOfWork)
			{
				uow = _currentUnitOfWork;
			}
			return uow;
		}

		public bool HasTransactionManager
		{
			get
			{
				if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
				
				bool hasTransactionManager;
				lock (_transactionManagerLock)
				{
					hasTransactionManager = (_currentTransactionManager != null);
				}
				return hasTransactionManager;
			}
		}

		public void StoreTransactionManager(ITransactionManager transactionManager)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			lock (_transactionManagerLock)
			{
				if (_currentTransactionManager == null)
					_currentTransactionManager = transactionManager;
			}
		}

		public ITransactionManager RetrieveTransactionManager()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			ITransactionManager transactionManager;
			lock(_transactionManagerLock)
			{
				transactionManager = _currentTransactionManager;
			}
			return transactionManager;
		}

		public void ClearTransactionManager()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			lock (_transactionManagerLock)
			{
				_currentTransactionManager = null;
			}
		}

		#endregion

	}

}
