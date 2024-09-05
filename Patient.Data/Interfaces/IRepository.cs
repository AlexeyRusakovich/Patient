namespace Patient.Data.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetById(Guid id);

        Task<bool> SaveChanges();

        Task Create(T entity);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
