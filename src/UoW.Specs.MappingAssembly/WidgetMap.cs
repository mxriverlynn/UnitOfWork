using FluentNHibernate.Mapping;
using UoW.Specs.Model;

namespace UoW.Specs.MappingAssembly
{
	public class WidgetMap: ClassMap<Widget>
	{
	
		public WidgetMap()
		{
			Id(w => w.Id);
			Map(w => w.SomeValue);
			WithTable("MyTable");
			HasManyToMany<Foo>(x => x.Id).AsSet().WithTableName("Foo_User");
		}
	
	}
}
