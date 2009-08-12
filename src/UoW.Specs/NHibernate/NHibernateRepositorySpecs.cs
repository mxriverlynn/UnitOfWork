using NUnit.Framework;
using SpecUnit;
using UoW.NHibernate;
using UoW.Specs.Model;

namespace UoW.Specs.NHibernate
{


	[TestFixture]
	[Concern("NHibernateRepository")]
	public class When_executing_an_nhibernate_repository_method : With_Valid_NHibernateConfig
	{

		private MockFooRepo foo;

		protected override void Context()
		{
			base.Context();

			foo = new MockFooRepo();

			UnitOfWork.Start(() =>
			{
				Transaction.Begin();
				NHibernateConfig.GenerateSchema();
				foo.Something();
				Transaction.Commit();
			});
		}

		private class MockFooRepo: NHibernateRepository, IFooRepository
		{
			public bool SomethingWasCalled;

			public void Something()
			{
				Foo foo = new Foo();
				Session.Save(foo);
				SomethingWasCalled = (foo.Id != 0);
			}
		}

		[Test]
		[Observation]
		public void Should_provide_the_NHibernate_Session_to_the_repository()
		{
			foo.SomethingWasCalled.ShouldEqual(true);
		}

	}

	[TestFixture]
	[Concern("NHibernateRepository")]
	public class When_executing_an_NHibernateRepository_method_without_a_UnitOfWork : With_Valid_NHibernateConfig
	{
		private MockFooRepo fooRepo;
		private NoUnitOfWorkException caughtException;

		protected override void Context()
		{
			base.Context();

			try
			{
                UnitOfWork.Configure(_config);
				fooRepo = new MockFooRepo();
				fooRepo.Something();
			}
			catch(NoUnitOfWorkException ex)
			{
				caughtException = ex;
			}
		}

		private class MockFooRepo : NHibernateRepository, IFooRepository
		{
			private Foo _foo;

			public void Something()
			{
				_foo = new Foo();
				Session.Save(_foo);
			}

		}

		[Test]
		[Observation]
		public void Should_throw_a_NoUnitOfWorkException()
		{
			caughtException.ShouldNotBeNull();
		}

	}



}
