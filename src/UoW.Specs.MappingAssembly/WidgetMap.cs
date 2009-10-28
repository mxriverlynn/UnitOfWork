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
			Table("MyTable");
			HasManyToMany<Foo>(x => x.Id).AsSet().Table("Foo_User");
		}
	
	}
}
