using System;
using log4net.Config;
using NUnit.Framework;
using SpecUnit;
using UoW;
using UoW.NHibernate;
using UoW.Specs.Mocks;

namespace UoW.Specs
{

	public abstract class BaseContext : ContextSpecification
	{

		protected static bool IsInitialized { get; private set; }
		private static readonly object _lock = new object();

		[TestFixtureSetUp]
		public void TestFixtureSetUp2()
		{
			lock (_lock)
			{
				if (!IsInitialized)
				{
					XmlConfigurator.Configure();
					IsInitialized = true;
				}
			}
		}
	}

	public abstract class BaseMockUoWContext : BaseContext
	{

		protected MockUnitOfWork mockUoW;
		protected MockUoWFactory mockUoWFactory;
		protected IUnitOfWorkStorage uowStorage;

		protected static bool IsInitialized { get; private set; }
		private static readonly object _lock = new object();

		[TestFixtureSetUp]
		public void TestFixtureSetUp2()
		{
			lock (_lock)
			{
				if (!IsInitialized)
				{
					XmlConfigurator.Configure();
					IsInitialized = true;
				}
			}
		}

		protected override void SharedContext()
		{
			mockUoW = new MockUnitOfWork();
			mockUoWFactory = new MockUoWFactory(mockUoW);
			IRepositoryFactory repositoryFactory = new DepConRepositoryFactory();
			uowStorage = new ThreadStaticUnitOfWorkStorage();

			UnitOfWork.Configure(new UnitOfWorkConfigurationBase(mockUoWFactory, repositoryFactory, uowStorage));
		}

	}

	public abstract class NHibernateUoWSpec : BaseContext
	{
		protected IRepositoryFactory _repositoryFactory;
		protected IUnitOfWorkStorage _uowStorage;

		protected override void Context()
		{
			base.Context();

			_repositoryFactory = new DepConRepositoryFactory();
			_uowStorage = new ThreadStaticUnitOfWorkStorage();
		}
	}

	public abstract class With_Valid_NHibernateConfig : NHibernateUoWSpec
	{
		protected NHibernateConfig _config;

		protected override void Context()
		{
			base.Context();

			_config = new NHibernateConfig(_repositoryFactory, _uowStorage);
			UnitOfWork.Configure(_config);
		}
	}



}