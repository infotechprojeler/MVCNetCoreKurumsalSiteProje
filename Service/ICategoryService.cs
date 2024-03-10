using Entities;

namespace Service
{
    public interface ICategoryService
    {
        // Burada CategoryService classında kategorilerle ilgili tüm crud işlemlerini yapabilmemizi sağlayan metotların imzalarını belirliyoruz
        List<Category> GetCategories();
        Category GetCategory(int id);
        Category GetCategoryByPosts(int id);
        Category GetCategoryByProducts(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
        int Save();
    }
}
