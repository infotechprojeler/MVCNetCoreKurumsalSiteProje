using System.Linq.Expressions;

namespace Service
{
    public interface IService<T> // Buradaki <T> bu interface in generic yani genel olarak kullanılacağını ifade ediyor
    {
        #region Senkron Metotlar
        // Burada Service classında tüm classlarla ilgili ortak crud işlemlerini yapabilmemizi sağlayan metotların imzalarını belirliyoruz
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> expression); // x=>x. .. şeklinde sorgu yazabilmemizi sağlayan metot
        T Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int Save();
        #endregion

        #region Asenkron Metotlar
        // Entity framework deki asenkron metotları da repository pattern de kullanmak istersek bu şekilde kullanabiliriz. Aşağıdaki metot imzalarını servis  classında kullanmaılıyız
        Task<T> FindAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<T> GetIncludeAsync(Expression<Func<T, bool>> expression, string table);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<int> SaveAsync();
        #endregion
    }
}
