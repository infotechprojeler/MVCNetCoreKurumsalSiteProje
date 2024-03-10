using System.Linq.Expressions;

namespace Service
{
    public interface IService<T> // Buradaki <T> bu interface in generic yani genel olarak kullanılacağını ifade ediyor
    {
        // Burada Service classında tüm classlarla ilgili ortak crud işlemlerini yapabilmemizi sağlayan metotların imzalarını belirliyoruz
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> expression); // x=>x. .. şeklinde sorgu yazabilmemizi sağlayan metot
        T Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int Save();
    }
}
