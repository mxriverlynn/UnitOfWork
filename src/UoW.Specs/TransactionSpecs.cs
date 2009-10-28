using System;
using System.Threading;
using NUnit.Framework;
using SpecUnit;
using UoW.Specs.Mocks;

namespace UoW.Specs
{

	public abstract class TransactionContext : BaseMockUoWContext
	{
		protected override void Context()
		{			
			base.Context();

			uowStorage.ClearTransactionManager();
		}
	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_committing_a_transaction : TransactionContext
	{

		protected override void Context()
		{
			base.Context();

			UnitOfWork.Start(() =>
			{
				Transaction.Begin();

				Transaction.Commit();
			});
		}

		[Test]
		[Observation]
		public void Should_begin_the_transaction()
		{
			MockTransactionManager mockTransactionManager = UnitOfWork.GetCurrentTransactionManager() as MockTransactionManager;
			mockTransactionManager.TransactionBegan.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_commit_the_transaction()
		{
			MockTransactionManager mockTransactionManager = UnitOfWork.GetCurrentTransactionManager() as MockTransactionManager;
			mockTransactionManager.TransactionCommitted.ShouldEqual(true);
		}
	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_rolling_back_a_transaction : TransactionContext
	{

		protected override void Context()
		{
			base.Context();

			UnitOfWork.Start(() =>
			{
				Transaction.Begin();
				Transaction.Rollback();
			});
		}

		[Test]
		[Observation]
		public void Should_rollback()
		{
			MockTransactionManager mockTransactionManager = UnitOfWork.GetCurrentTransactionManager() as MockTransactionManager;
			mockTransactionManager.TransactionRolledBack.ShouldEqual(true);
		}

	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_committing_a_transaction_without_beginning_a_transaction : TransactionContext
	{

		private Exception caughtException;

		protected override void Context()
		{
			base.Context();

			try
			{
				UnitOfWork.Start(Transaction.Commit);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
		}

		[Test]
		[Observation]
		public void Should_not_commit()
		{
			MockTransactionManager mockTransactionManager = UnitOfWork.GetCurrentTransactionManager() as MockTransactionManager;
			mockTransactionManager.TransactionCommitted.ShouldEqual(false);
		}

		[Test]
		[Observation]
		public void Should_not_throw_an_exception()
		{
			caughtException.ShouldBeNull();
		}

	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_rolling_back_a_transaction_without_beginning_a_transaction : TransactionContext
	{

		private Exception caughtException;

		protected override void Context()
		{
			base.Context();

			try
			{
				UnitOfWork.Start(Transaction.Rollback);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
		}

		[Test]
		[Observation]
		public void Should_not_rollback()
		{
			MockTransactionManager mockTransactionManager = UnitOfWork.GetCurrentTransactionManager() as MockTransactionManager;
			mockTransactionManager.TransactionRolledBack.ShouldEqual(false);
		}

		[Test]
		[Observation]
		public void Should_not_throw_an_exception()
		{
			caughtException.ShouldBeNull();
		}

	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_closing_a_unit_of_work_and_a_transaction_has_not_been_comitted : TransactionContext
	{

		private Exception caughtException;

		protected override void Context()
		{
			try
			{
				UnitOfWork.Start(Transaction.Begin);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
		}

		[Test]
		[Observation]
		public void Should_rollback()
		{
			MockTransactionManager mockTransactionManager = UnitOfWork.GetCurrentTransactionManager() as MockTransactionManager;
			mockTransactionManager.TransactionRolledBack.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_not_throw_an_exception()
		{
			caughtException.ShouldBeNull();
		}

	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_beginning_a_transaction_and_no_ITransactionManager_is_provided : TransactionContext
	{

		private Exception caughtException;

		protected override void Context()
		{
			base.Context();

			try
			{
				UnitOfWork.Start(Transaction.Begin);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
		}

		[Test]
		[Observation]
		public void Should_not_throw_a_TransactionException()
		{
			caughtException.ShouldBeNull();
		}

	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_starting_and_comitting_with_no_ITransactionmanager_provided : TransactionContext
	{

		protected MockTransactionManager mockTransactionManager;
		private new MockUnitOfWork mockUoW;
		private new MockUoWFactory mockUoWFactory;
		private Exception caughtException;

		protected override void Context()
		{
			base.Context();

			try
			{
				mockUoW = new MockUnitOfWork();
				mockUoWFactory = new MockUoWFactory(mockUoW);
				IRepositoryFactory repositoryFactory = new StructureMapRepositoryFactory();
				IUnitOfWorkStorage uowStorage = new ThreadStaticUnitOfWorkStorage();
				UnitOfWork.Configure(new UnitOfWorkConfigurationBase(mockUoWFactory, repositoryFactory, uowStorage));
				
				UnitOfWork.Start(() =>
				{
					Transaction.Begin();
					Transaction.Commit();
				});
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
		}


		[Test]
		[Observation]
		public void Should_succeed_without_errors()
		{
			caughtException.ShouldBeNull();
		}

	}

	[TestFixture]
	[Concern("Transaction")]
	public class When_starting_transactions_on_two_threads_concurrently : UoWRetrievalContext
	{

		protected override void Context()
		{
			base.Context();

			Thread run1 = new Thread(() => UnitOfWork.Start(()=>
        	{
        		Transaction.Begin();
        		Transaction.Commit();
        	}));

			Thread run2 = new Thread(() => UnitOfWork.Start(() =>
        	{
				Transaction.Begin();
        		Transaction.Commit();
        	}));

			run1.Start();
			run2.Start();

			run1.Join();
			run2.Join();
		}

		[Test]
		[Observation]
		public void Should_get_two_transactions()
		{
			mockUoWFactory.GeneratedTransactionManagers.Count.ShouldEqual(2);
		}

	}


}