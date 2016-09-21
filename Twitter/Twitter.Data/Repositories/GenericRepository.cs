using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Data.Repositories
{
    using System.Data.Entity;

    using Twitter.Data.Contracts;

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private DbContext context;

        private IDbSet<T> set;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        protected IDbSet<T> Set
        {
            get
            {
                return this.set;
            }

            set
            {
                this.set = value;
            }
        }

        protected DbContext Context
        {
            get
            {
                return this.context;
            }

            set
            {
                this.context = value;
            }
        }

        public IQueryable<T> All()
        {
            return this.set.AsQueryable();
        }

        public T Find(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
