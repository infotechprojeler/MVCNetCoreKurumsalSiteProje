using Data;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Service
{
    public class Service<T> : IService<T> where T : class // Buraya gönderilecek olan T bir class olmalıdır
        , IEntity // gönderilecek olan bu class IEntity den miras almış olmalıdır
        , new() // ve (Product product = new Product() şeklinde ) new lenebilir bir tip olmalıdır 
    {
        private readonly DatabaseContext _context; // tabloları tutan contextimiz
        private readonly DbSet<T> _dbSet; // veritabanı işlemlerini yürüteceğimiz dbset
        public Service(DatabaseContext context)
        {
            _context = context; // yukardaki boş contexti doldurduk
            _dbSet = _context.Set<T>(); // yukardaki boş db seti hangi class gönderilmişse onun için çalışmak üzere ayarladık.
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToList();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
