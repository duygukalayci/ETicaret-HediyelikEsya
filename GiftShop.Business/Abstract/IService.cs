using System.Linq.Expressions;

namespace GiftShop.Business.Abstract
{
    public interface IService<T> where T : class
    {
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T,bool>>expression);
        IQueryable<T> GetAllQueryable();
        T Get(Expression<Func<T, bool>> expression);
        T Find(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int saveChanges();

        //
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
    }
}
