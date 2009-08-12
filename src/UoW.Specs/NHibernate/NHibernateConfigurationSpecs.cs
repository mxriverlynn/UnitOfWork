using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SpecUnit;
using UoW.NHibernate;
using UoW.Specs.Model;

namespace UoW.Specs.NHibernate
{

	[TestFixture]
	[Concern("Configuring NHibernate UnitOfWork")]
	public class When_configuring_NHibernateUoW_with_specified_config_file : NHibernateUoWSpec
	{
		private bool wasExecuted;

		protected override void Context()
		{
			base.Context();

		    NHibernateConfig config = new NHibernateConfig(@".\hibernate.cfg.xml", _uowStorage);
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

    [TestFixture]
	[Concern("Configuring NHibernate UnitOfWork")]
	public class When_configuring_NHibernateUoW_with_config_supplied_as_dictionary : NHibernateUoWSpec
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
         		{
         			"connection.connection_string",
         			@"Data Source=:memory:;Version=3;New=True;"
         			},
         		{"connection.release_mode", "on_close"}
         	};

			NHibernateConfig config = new NHibernateConfig(properties, _uowStorage, typeof(Foo).Assembly );
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

	[TestFixture]
	[Concern("Configuring NHibernate UnitOfWork")]
	public class When_configuring_NHibernateUoW_with_config_supplied_as_xml_text_reader : NHibernateUoWSpec
	{
		private bool wasExecuted;

		protected override void Context()
		{
			base.Context();

			FileStream fs = new FileStream(@".\hibernate.cfg.xml", FileMode.Open, FileAccess.Read);
			NHibernateConfig config = new NHibernateConfig(fs, _uowStorage);
		    UnitOfWork.Configure(config);
			fs.Dispose();

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