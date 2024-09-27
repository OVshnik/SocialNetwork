﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Repository
{
	public class Repository<T> : IRepository<T> where T : class 
	{
		protected DbContext _db;
		public DbSet<T> Set { get; private set; }
		public Repository(ApplicationDbContext db)
		{
			_db = db;
			var set=db.Set<T>();
			set.LoadAsync();
			Set = set;
		}
		public async Task Create(T item)
		{
			await Set.AddAsync(item);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(T item)
		{
			Set.Remove(item);
			await _db.SaveChangesAsync();
		}

		public async Task <T> Get(int id)
		{
			return await Set.FindAsync(id);
		}

		public IEnumerable<T> GetAll()
		{
			return Set;
		}

		public async Task Update(T item)
		{
			Set.Update(item);
			await _db.SaveChangesAsync();
		}
	}
}
