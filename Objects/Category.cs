using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeApp
{
  public class Category
  {
    private int _id;
    private string _name;

    public Category(string categoryName, int categoryId = 0)
    {
      _id = categoryId;
      _name = categoryName;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public static void DeleteAll()
    {
      DB.DeleteAll("categories");
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allCategories;
    }

    public override bool Equals(System.Object randomCategory)
    {
      if(!(randomCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) randomCategory;
        bool idEquality = (this.GetId() == newCategory.GetId());
        bool nameEquality = (this.GetName() == newCategory.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@CategoryName) ;", conn);
      cmd.Parameters.Add(new SqlParameter("@CategoryName", this.GetName()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      DB.CloseSqlConnection(conn, rdr);
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @TargetId; DELETE FROM recipes_categories WHERE category_id = @TargetId; DELETE FROM ingredients_categories WHERE category_id = @TargetId;", conn);
      cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

      cmd.ExecuteNonQuery();
      DB.CloseSqlConnection(conn);
    }


    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId;", conn);
      cmd.Parameters.Add(new SqlParameter("@CategoryId", id.ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int categoryId = 0;
      string categoryName = null;

      while (rdr.Read())
      {
        categoryId = rdr.GetInt32(0);
        categoryName = rdr.GetString(1);
      }

      Category foundCategory = new Category(categoryName, categoryId);

      DB.CloseSqlConnection(conn, rdr);
      return foundCategory;
    }

    public static List<Category> SearchName(string categoryName)
    {
      List<Category> foundCategories = new List<Category>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE name LIKE @CategoryName", conn);
      cmd.Parameters.Add(new SqlParameter("@CategoryName", "%" + categoryName + "%"));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        Category foundCategory = new Category(newName, categoryId);
        foundCategories.Add(foundCategory);
      }

      DB.CloseSqlConnection(conn, rdr);
      return foundCategories;
    }

    public void AddRecipe(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_categories (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", newRecipe.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@CategoryId", this.GetId().ToString()));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }

    public List<Recipe> GetRecipesByCategory()
    {
      List<Recipe> allRecipes = new List<Recipe>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM categories JOIN recipes_categories ON (categories.id = recipes_categories.category_id) JOIN recipes ON (recipes.id = recipes_categories.recipe_id) WHERE categories.id = @CategoryId;", conn);
      cmd.Parameters.Add(new SqlParameter("@CategoryId", this.GetId().ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        string recipeInstructions = rdr.GetString(2);
        Recipe newRecipe = new Recipe(recipeName, recipeInstructions, newId);
        allRecipes.Add(newRecipe);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allRecipes;
    }
  }
}
