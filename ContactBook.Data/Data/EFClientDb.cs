using System;
using System.Linq;
using System.Data.Entity;
using ContactBook.Core.Models;
using ContactBook.Core.Repository;
using System.Data.Entity.Validation;


namespace ContactBook.Data.Data
{
    public class EFClientDb : DbContext, IGenericRepo
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Country> Countries { get; set; }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        public IQueryable<T> QueryIncluding<T>(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T GetById<T>(int id) where T : class
        {
            return Set<T>().Find(id);
        }

        public void Add<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Entry(entity).State = System.Data.Entity.EntityState.Modified;
            
        }

        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public void Delete<T>(int id) where T : class
        {
            var entity = Set<T>().Find(id);
            Set<T>().Remove(entity);
        }

        public bool Save()
        {
            try
            {
                return this.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine("============E R R O R ======================");
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine("Property: {0} Error: {1}",
                          validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                //System.InvalidOperationException

                Console.WriteLine("============E R R O R ======================");
                return false;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}
