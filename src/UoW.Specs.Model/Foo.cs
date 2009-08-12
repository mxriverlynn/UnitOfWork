namespace UoW.Specs.Model
{
	public class Foo
	{
		private int _id;
		private string _bar;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Bar
		{
			get { return _bar; }
			set { _bar = value; }
		}
	}
}