using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Repository
{
	public interface IUnitOfWork:IDisposable
	{
		int SaveChanges(bool ensureAutoHistory = false);
		IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
		
	}
}
