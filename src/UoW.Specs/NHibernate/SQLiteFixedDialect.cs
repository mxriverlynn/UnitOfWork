using System.Data;
using NHibernate.Dialect;

namespace UoW.Specs.NHibernate
{
	public class SQLiteFixedDialect : SQLiteDialect
	{
		public SQLiteFixedDialect()
		{
			RegisterColumnType(DbType.DateTime, "TIMESTAMP DEFAULT CURRENT_TIMESTAMP");
			RegisterColumnType(DbType.Time, "TIMESTAMP DEFAULT CURRENT_TIMESTAMP");
		}

		public override bool SupportsIdentityColumns
		{
			get { return true; }
		}

	}
}