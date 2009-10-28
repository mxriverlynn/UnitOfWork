using NUnit.Framework;
using SpecUnit;
using StructureMap;
using UoW.NHibernate;
using UoW.Specs.Model;

namespace UoW.Specs.NHibernate
{
	public class MockFooRepo : NHibernateRepository, IFooRepository
	{
		public void Something()
		{
			Foo foo = new Foo();
			Session.Save(foo);
		}
	}

	[TestFixture]
	[Concern("NHibernateUoW")]
	public class When_shutting_down_NHibernateUoW : With_Valid_NHibernateConfig
	{
		protected Container container;
		protected override void Context()
		{
			base.Context();

			UnitOfWork.Start(() =>
			{
				Transaction.Begin();
				NHibernateConfig.GenerateSchema();

				ObjectFactory.Initialize(
				factory => factory
				    .ForRequestedType<IFooRepository>()
				    .TheDefaultIsConcreteType<MockFooRepo>());

				Repository<IFooRepository>.Do.Something();

				Assert.IsTrue(_uowStorage.HasTransactionManager);

				Transaction.Commit();
			});

		}

		[Test]
		[Observation]
		public void Should_clear_transaction_manager_from_storage()
		{
			Assert.IsFalse(_uowStorage.HasTransactionManager);
		}

	}
}
