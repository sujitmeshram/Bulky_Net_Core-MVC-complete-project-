using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BulkyBook.DataAccess.Repository
{

    //Implementing IRepository here.
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        //71.2.43 Since, we are playing with generics, we dont know the class, so adding DbSet for that.
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;

            //71.3.03 assigning dbSet based on the generic class
            this.dbSet = _db.Set<T>();
            //foe ex. above is equivalent to _db.Categories == dbSet

            //100.1.44 this is for including the category data into the Product.
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
            
        }

        public void Add(T entity)
        {

            //adding the entity here
            dbSet.Add(entity);
        }


        //to get the single T return
        //includeProperties for including the other proeprties, refer 100.4.25

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked) {
                 query= dbSet;
                
            }
            else {
                 query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties)) {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        //to get all the T return
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, 
            string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null) {
                query = query.Where(filter);
            }

            //for including other properties here.
			if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
