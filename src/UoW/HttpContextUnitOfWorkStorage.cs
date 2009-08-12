using System;
using System.Web;
using log4net;

namespace UoW
{
	public class HttpContextUoWStorage : IUnitOfWorkStorage
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private const string TRANSACTION_MANAGER = ".TransactionManager";
		private string _keyName;

		public bool HasUnitOfWork
		{
			get
			{
				if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
				bool hasUoW = (HttpContext.Items[KeyName] != null);
				return hasUoW;
			}
		}

		public bool HasTransactionManager
		{
			get
			{
				if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
				bool hasTransMan = (HttpContext.Items[TransactionManagerKeyName] != null);
				return hasTransMan;
			}
		}

		private HttpContext HttpContext { get { return HttpContext.Current; } }

		private string KeyName
		{
			get { return _keyName; }
			set
			{
				if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
				if (log.IsDebugEnabled) log.Debug(value);

				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				_keyName = value;
			}
		}

		private string TransactionManagerKeyName
		{
			get { return KeyName + TRANSACTION_MANAGER; }
		}

		public HttpContextUoWStorage(string storageKeyName)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(storageKeyName);

			KeyName = storageKeyName;
		}

		public void Store(IUnitOfWork uow)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(uow);

			HttpContext.Items[KeyName] = uow;
		}

		public void StoreTransactionManager(ITransactionManager transactionManager)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(transactionManager);

			HttpContext.Items[TransactionManagerKeyName] = transactionManager;
		}

		public IUnitOfWork Retrieve()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			return HttpContext.Items[KeyName] as IUnitOfWork;
		}

		public ITransactionManager RetrieveTransactionManager()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			return HttpContext.Items[TransactionManagerKeyName] as ITransactionManager;
		}

		public void Clear()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			HttpContext.Items[KeyName] = null;
		}

		public void ClearTransactionManager()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			HttpContext.Items[TransactionManagerKeyName] = null;
		}

	}
}