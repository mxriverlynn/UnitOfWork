using System.Collections.Generic;
using NHibernate.Cfg;
using NUnit.Framework;
using SpecUnit;
using UoW.NHibernate;
using UoW.Specs.MappingAssembly;
using FluentNHibernate;

namespace UoW.Specs.NHibernate
{
	[TestFixture]
	[Concern("Configuring UoW With Fluent NHibernate")]
	public class When_configuring_UoW_with_Fluent_NHibernate : NHibernateUoWSpec
	{

		private bool wasExecuted;

		protected override void Context()
		{
			base.Context(); 

			IDictionary<string, string> properties = new Dictionary<string, string>
         	{
         		{"connection.driver_class", "NHibernate.Driver.SQLite20Driver"},
         		{"dialect", "NHibernate.Dialect.SQLiteDialect"},
         		{"connection.provider", "NHibernate.Connection.DriverConnectionProvider"},
         		{"connection.connection_string", @"Data Source=:memory:;Version=3;New=True;"},
         		{"connection.release_mode", "on_close"}
         	};

			NHibernateConfig config
				= new NHibernateConfig(() =>
				                       	{
				                       		var cfg = new Configuration().Configure()
												.SetProperties(properties);
											cfg.AddMappingsFromAssembly(typeof(WidgetMap).Assembly);
				                       		return cfg;
				                       	}, _repositoryFactory, _uowStorage);
				
			UnitOfWork.Configure(config);
			UnitOfWork.Start(() =>
			{
				wasExecuted = true;
			});
		}

		[Test]
		[Observation]
		public void Should_allow_UnitOfWork_to_be_executed()
		{
			wasExecuted.ShouldEqual(true);
		}


	}

}
