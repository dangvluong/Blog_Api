using WebApp.Models;

namespace WebApp.Helper
{
    public static class CategoryHelper
    {

        public static List<Category> CreateTreeLevelCategory(List<Category> source)
        {
            Dictionary<int, Category> dictCategory = new Dictionary<int, Category>();
            foreach (Category category in source)
            {
                dictCategory[category.Id] = category;
            }
            List<Category> listCategory = new List<Category>();

            foreach (Category category in source)
            {
                if (category.ParentCategoryId == null)
                    listCategory.Add(category);
                else
                {
                    if (dictCategory.ContainsKey(category.ParentCategoryId.Value))
                    {
                        if (dictCategory[category.ParentCategoryId.Value].ChildCategories == null)
                            dictCategory[category.ParentCategoryId.Value].ChildCategories = new List<Category>();
                        dictCategory[category.ParentCategoryId.Value].ChildCategories.Add(category);
                    }
                }
            }

            return listCategory;
        }

        public static void CreateSelectListCategory(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                category.Name = prefix + category.Name;
                des.Add(category);
                if (category.ChildCategories?.Count > 0)
                {
                    CreateSelectListCategory(category.ChildCategories, des, level + 1);
                }
            }
        }
    }
}
