using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace UoW.NHibernate
{

	public class NHibernateConfig : UnitOfWorkConfigurationBase
	{

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static Configuration _config;
		private static ISessionFactory _sessionFactory;

		#region Constructors

		public NHibernateConfig(IRepositoryFactory repositoryFactory, IUnitOfWorkStorage storage) : base(new NHibernateUoWFactory(), repositoryFactory, storage)
	    {
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			
			_config = new Configuration();
	        _config.Configure();

	        ConfigureUnitOfWork();
	    }

	    public NHibernateConfig(string configFile, IRepositoryFactory repositoryFactory, IUnitOfWorkStorage storage) : base(new NHibernateUoWFactory(), repositoryFactory, storage)
	    {
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(configFile);
			
			_config = new Configuration();
	        _config.Configure(configFile);

	        ConfigureUnitOfWork();
	    }

		public NHibernateConfig(Stream configData, IRepositoryFactory repositoryFactory, IUnitOfWorkStorage storage) : base(new NHibernateUoWFactory(), repositoryFactory, storage)
	    {
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			if (log.IsDebugEnabled) log.Debug(configData);
			
			_config = new Configuration();

	        XmlTextReader reader = new XmlTextReader(configData);
	        _config.Configure(reader);

	        ConfigureUnitOfWork();

	        reader.Close();
	    }

	    public NHibernateConfig(IDictionary<string, string> properties, IRepositoryFactory repositoryFactory, IUnitOfWorkStorage storage, params Assembly[] assemblies) : base(new NHibernateUoWFactory(), repositoryFactory, storage)
	    {
	        _config = new Configuration { Properties = properties };

	        if (assemblies != null)
	        {
	            foreach (Assembly assembly in assemblies)
	            {
	                _config = _config.AddAssembly(assembly);
	            }
	        }
	        ConfigureUnitOfWork();
	    }

		public NHibernateConfig(Func<Configuration> configurationAction, IRepositoryFactory repositoryFactory, IUnitOfWorkStorage storage) : base(new NHibernateUoWFactory(), repositoryFactory, storage)
		{
			_config = configurationAction();
			ConfigureUnitOfWork();
		}

		#endregion

		#region NHibernate specific methods

		internal static ISession GetSession()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			ISession session = _sessionFactory.OpenSession();
			session.FlushMode = FlushMode.Auto;

			if (log.IsDebugEnabled) log.Debug(session);			
			return session;
		}

		public static void GenerateSchema()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);

			SchemaExport exporter = new SchemaExport(_config);
			exporter.Execute(false, true, false, false, NHibernateUoW.CurrentSession.Connection, null);
		}

		#endregion

		#region Helper Methods

		private void ConfigureUnitOfWork()
		{
			if (log.IsInfoEnabled) log.Info(Consts.ENTERED);
			
			_sessionFactory = _config.BuildSessionFactory();

			if (log.IsDebugEnabled) log.Debug(_sessionFactory);
		}

		#endregion

	}
}