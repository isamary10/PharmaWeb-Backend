using Microsoft.EntityFrameworkCore;
using PharmaWeb.Persistencia;

namespace PharmaWeb.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PharmaWebContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(PharmaWebContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }

            // Usar a expressão para encontrar a chave primária da entidade
            // dessa forma o método fica genérico podendo ser usando por outros controllers
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id"));
            if (keyProperty != null)
            {
                return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, keyProperty.Name) == id);
            }

            return null;
        }
        public async Task AddAsync(T entity) 
        { 
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id"));

            if (keyProperty == null)
                throw new Exception("Primary key property not found.");

            int id = (int)keyProperty.GetValue(entity);

            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
                throw new Exception("Entity not found.");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("Id"));

            if (keyProperty == null)
                throw new Exception("Primary key property not found.");

            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new Exception($"Entity with ID {id} not found.");

            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error saving changes to the database.", ex);
            }
        }
    }
}
