using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Entities.Contexts
{
    public abstract class AbstractDbContext<T> : BaseDbContext where T : AbstractAuditable
    {

        public AbstractDbContext(IConfiguration configuration) : base(configuration)
        {
        }
        public AbstractDbContext() { }

        public abstract void addToEntites(T entity);
        public abstract void removeFromEntites(T entity);
        public abstract void updateEntity(T entity);

        //TODO: DELET?

    }
}
