using NHibernate;

namespace UoW.NHibernate
{

	public abstract class NHibernateRepository
	{

		protected static internal ISession Session
		{
			get
			{
				return NHibernateUoW.CurrentSession;
			}
		}

	}

}
