using System;
using log4net;
using NHibernate;

namespace UoW.NHibernate
{
	public class NHibernateTransMan: ITransactionManager
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Variables and properties

		private ISession _session;

		private ISession Session
		{
			get { return _session; }
			set
			{
				if( value == null )
				{
					throw new ArgumentNullException("value", "This property cannot be set to null");
				}
				_session = value;
			}
		}

		private ITransaction Transaction { get; set; }

		public bool IsInTransaction { get; private set; }

		#endregion

		public NHibernateTransMan(ISession session)
		{
			Session = session;
		}

		public void Begin()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			Transaction = Session.BeginTransaction();
			IsInTransaction = true;
		}

		public void Commit()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			Transaction.Commit();
			IsInTransaction = false;
		}

		public void Rollback()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			Transaction.Rollback();
			IsInTransaction = false;
		}
	}
}
