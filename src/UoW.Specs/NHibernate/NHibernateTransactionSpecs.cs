using System.Collections.Generic;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using UoW.NHibernate;
using UoW.Specs.Model;

namespace UoW.Specs.NHibernate
{

	[TestFixture]
	[Concern("NHibernate Transaction")]
	public class When_executing_a_unit_of_work_with_an_NHibernate_Transaction : NHibernateUoWSpec
	{

		private bool wasExecuted;

		protected override void Context()
		{
			base.Context();

			NHibernateConfig config = new NHibernateConfig(_repositoryFactory, _uowStorage);
            UnitOfWork.Configure(config);
			UnitOfWork.Start(() =>
			{
				Transaction.Begin();
				NHibernateConfig.GenerateSchema();
				Transaction.Commit();
				wasExecuted = true;
			});
		}

		[Test]
		[Observation]
		public void Should_execute_the_transaction()
		{
			wasExecuted.ShouldEqual(true);
		}

	}

	[TestFixture]
	[Concern("NHibernate Transaction")]
	public class When_rolling_back_an_NHibernate_Transaction : NHibernateUoWSpec
	{

		private MockFooRepo fooRepo;
		private Foo foo;
		
		protected override void Context()
		{
			base.Context();

			//DepCon.ClearRegistrations();
			fooRepo = new MockFooRepo();
			//DepCon.RegisterInstance<IFooRepository>(fooRepo);
			ObjectFactory.Initialize(
				factory => factory
					.ForRequestedType<IFooRepository>().TheDefault.IsThis(fooRepo));

			IDictionary<string, string> properties = new Dictionary<string, string>
			{
			    {"connection.driver_class", "NHibernate.Driver.SQLite20Driver"},
			    {"dialect", "NHibernate.Dialect.SQLiteDialect"},
			    {"connection.provider", "NHibernate.Connection.DriverConnectionProvider"},
			    {
			        "connection.connection_string",
			        @"Data Source=FooDb.s3db"
			        },
			    {"connection.release_mode", "on_close"},
				{"proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle"}
			};

            NHibernateConfig config = new NHibernateConfig(properties, _repositoryFactory, _uowStorage, typeof(Foo).Assembly );
			UnitOfWork.Configure(config);
			UnitOfWork.Start(() =>
			{
				Transaction.Begin();
				Repository<IFooRepository>.Do.Something();
				Transaction.Rollback();
			});
			UnitOfWork.Start(() =>
     		{
				foo = fooRepo.CheckForIt();
     		});
		}

		private class MockFooRepo : NHibernateRepository, IFooRepository
		{
			private Foo _foo;

			public void Something()
			{
				_foo = new Foo();
				Session.Save(_foo);
			}

			public Foo CheckForIt()
			{
				return Session.Get<Foo>(_foo.Id);
			}

		}

		[Test]
		[Observation]
		public void Should_not_commit_the_changes()
		{
			foo.ShouldBeNull();
		}

	}

}
