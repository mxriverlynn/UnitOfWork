using System;
using NUnit.Framework;
using SpecUnit;

namespace UoW.Specs
{
	public abstract class ExecuteAUoWContext : BaseMockUoWContext
	{
		protected bool unitOfWorkExecuted;		
	}

	[TestFixture]
	[Concern("Execute UoW")]
	public class When_executing_a_unit_of_work: ExecuteAUoWContext
	{

		protected override void Context()
		{
			UnitOfWork.Start();
			UnitOfWork.Stop();
		}

		[Test]
		[Observation]
		public void Should_start_the_unit_of_work()
		{
			mockUoW.WasStarted.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_stop_the_unit_of_work()
		{
			mockUoW.WasStopped.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_retrieve_the_UoW_from_the_UoWFactory()
		{
			mockUoWFactory.UoWRetrieved.ShouldEqual(true);
		}

	}

	[TestFixture]
	[Concern("Execute UoW")]
	public class When_executing_a_unit_of_work_with_the_shortcut_lambda : ExecuteAUoWContext
	{

		protected override void Context()
		{
			UnitOfWork.Start(() =>
			{
				unitOfWorkExecuted = true;
			});
		}

		[Test]
		[Observation]
		public void Should_start_the_unit_of_work()
		{
			mockUoW.WasStarted.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_stop_the_unit_of_work()
		{
			mockUoW.WasStopped.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_execute_the_unit_of_work()
		{
			unitOfWorkExecuted.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_retrieve_the_UoW_from_the_UoWFactory()
		{
			mockUoWFactory.UoWRetrieved.ShouldEqual(true);
		}

	}

	[TestFixture]
	[Concern("Execute UoW")]
	public class When_an_exception_is_thrown_from_a_unit_of_work : ExecuteAUoWContext
	{
		
		private Exception caughtException;
        
		protected override void Context()
		{
			try
			{
				UnitOfWork.Start(() =>
             	{
             		throw new Exception("Test");
             	});
			}
			catch(Exception ex)
			{
				caughtException = ex;
			}
		}

		[Test]
		[Observation]
		public void Should_shut_down_unit_of_work()
		{
			mockUoW.WasStopped.ShouldEqual(true);
		}

		[Test]
		[Observation]
		public void Should_rethrow_exception()
		{
			caughtException.ShouldNotBeNull();
		}

	}

	[TestFixture]
	[Concern("Execute UoW")]
	public class When_executing_a_unit_of_work_without_configuring_UnitOfWork : ContextSpecification
	{
		private NoUnitOfWorkConfigurationException caughtException;

		protected override void Context()
		{
			try
			{
			    UnitOfWork.Configure(null);
				UnitOfWork.Start(() => { });
			}
			catch(NoUnitOfWorkConfigurationException ex)
			{
				caughtException = ex;
			}
		}

		[Test]
		[Observation]
		public void Should_throw_a_NoUnitOfWorkConfigurationException()
		{
			caughtException.ShouldNotBeNull();
		}

	}


}
