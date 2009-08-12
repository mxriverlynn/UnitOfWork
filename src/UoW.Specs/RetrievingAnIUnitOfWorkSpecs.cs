using System.Threading;
using NUnit.Framework;
using SpecUnit;
using UoW;
using UoW.Specs.Mocks;

namespace UoW.Specs
{

	public abstract class UoWRetrievalContext : BaseMockUoWContext { }

	[TestFixture]
	[Concern("IUnitOfWork Retrieval")]
	public class When_retrieving_an_IUnitOfWork_in_nested_calls : UoWRetrievalContext
	{

		protected override void Context()
		{
			UnitOfWork.Start(() => UnitOfWork.Start(() => {}));
		}

		[Test]
		[Observation]
		public void Should_request_one_IUnitOfWork_from_the_IUnitOfWorkFactory()
		{
			mockUoWFactory.CreateUoWCalled.ShouldEqual(1);
		}

	}

	[TestFixture]
	[Concern("IUnitOfWork Retrieval")]
	public class When_retrieving_an_IUnitOfWork_on_two_threads : UoWRetrievalContext
	{

		protected override void Context()
		{
			Thread run1 = new Thread(() => UnitOfWork.Start(()=>
        	{
        	}));

			Thread run2 = new Thread(() => UnitOfWork.Start(() =>
        	{
        	}));

			run1.Start();
			run2.Start();

			run1.Join();
			run2.Join();
		}

		[Test]
		[Observation]
		public void Should_request_two_IUnitOfWork_from_the_IUnitOfWorkFactory()
		{
			mockUoWFactory.CreateUoWCalled.ShouldEqual(2);
		}
	}

	[TestFixture]
	[Concern("IUnitOfWork Retrieval")]
	public class When_starting_multiple_units_of_work_sequentially : UoWRetrievalContext
	{

		protected override void Context()
		{
			UnitOfWork.Start(() =>
         	{
         		
         	});

			UnitOfWork.Start(() =>
         	{
         		
         	});
		}

		[Test]
		[Observation]
		public void Should_request_two_IUnitOfWork_from_the_IUnitOfWorkFactory()
		{
			mockUoWFactory.CreateUoWCalled.ShouldEqual(2);
		}

	}

}