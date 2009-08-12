using System;
using log4net;

namespace UoW
{

	public class UnitOfWorkConfigurationBase : IUnitOfWorkConfiguration
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Variables and properties

		private IUnitOfWorkFactory _unitOfWorkFactory;
		public IUnitOfWorkFactory UnitOfWorkFactory
		{
			get { return _unitOfWorkFactory; }
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "This property cannot be set to null");
				}
				_unitOfWorkFactory = value;
			}
		}

		private IUnitOfWorkStorage _unitOfWorkStorage;
		public IUnitOfWorkStorage UnitOfWorkStorage
		{
			get { return _unitOfWorkStorage; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "This property cannot be set to null");
				}
				_unitOfWorkStorage = value;
			}
		}

		private IRepositoryFactory _repositoryFactory;
		public IRepositoryFactory RepositoryFactory
		{
			get { return _repositoryFactory; }
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "This property cannot be set to null");
				}
				_repositoryFactory = value;
			}
		}

		#endregion

		public UnitOfWorkConfigurationBase(IUnitOfWorkFactory uowFactory, IRepositoryFactory repositoryFactory, IUnitOfWorkStorage storage)
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(new object[] { uowFactory, repositoryFactory, storage } );

			UnitOfWorkFactory = uowFactory;
			RepositoryFactory = repositoryFactory;
			UnitOfWorkStorage = storage;

			if (log.IsDebugEnabled) log.Debug("Done creating UoWConfiguration");
		}
	}

}