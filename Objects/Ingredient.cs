using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeApp
{
  public class Ingredient
  {
    private int _id;
    private string _name;
    private string _amount;

    public Ingredient(string ingredientName, int ingredientId = 0, string ingredientAmount = null)
    {
      _id = ingredientId;
      _name = ingredientName;
      _amount = ingredientAmount;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetAmount()
    {
      return _amount;
    }

    public static void DeleteAll()
    {
      DB.DeleteAll("ingredients");
    }

    public static List<Ingredient> GetAll()
    {
      List<Ingredient> allIngredients = new List<Ingredient>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        string ingredientName = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
        allIngredients.Add(newIngredient);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allIngredients;
    }

    public override bool Equals(System.Object randomIngredient)
    {
      if(!(randomIngredient is Ingredient))
      {
        return false;
      }
      else
      {
        Ingredient newIngredient = (Ingredient) randomIngredient;
        bool idEquality = (this.GetId() == newIngredient.GetId());
        bool nameEquality = (this.GetName() == newIngredient.GetName());
        bool amountEquality = (this.GetAmount() == newIngredient.GetAmount());
        return (idEquality && nameEquality && amountEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (name) OUTPUT INSERTED.id VALUES (@IngredientName);", conn);
      cmd.Parameters.Add(new SqlParameter("@IngredientName", this.GetName()));
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

      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients WHERE id = @TargetId; DELETE FROM recipes_ingredients WHERE ingredient_id = @TargetId; DELETE FROM ingredients_categories WHERE ingredient_id = @TargetId;", conn);
      cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

      cmd.ExecuteNonQuery();
      DB.CloseSqlConnection(conn);
    }


    public static Ingredient Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @IngredientId;", conn);
      cmd.Parameters.Add(new SqlParameter("@IngredientId", id.ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int ingredientId = 0;
      string ingredientName = null;

      while (rdr.Read())
      {
        ingredientId = rdr.GetInt32(0);
        ingredientName = rdr.GetString(1);
      }

      Ingredient foundIngredient = new Ingredient(ingredientName, ingredientId);

      DB.CloseSqlConnection(conn, rdr);
      return foundIngredient;
    }

    public static List<Ingredient> SearchName(string ingredientName)
    {
      List<Ingredient> foundIngredients = new List<Ingredient>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE name LIKE @IngredientName", conn);
      cmd.Parameters.Add(new SqlParameter("@IngredientName", "%" + ingredientName + "%"));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        Ingredient foundIngredient = new Ingredient(newName, ingredientId);
        foundIngredients.Add(foundIngredient);
      }

      DB.CloseSqlConnection(conn, rdr);
      return foundIngredients;
    }

    public void AddRecipe(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_ingredients (recipe_id, ingredient_id) VALUES (@RecipeId, @IngredientId);", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", newRecipe.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@IngredientId", this.GetId().ToString()));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }

    public void AddCategory(Category newCategory)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO ingredients_categories (ingredient_id, category_id) VALUES (@IngredientId, @CategoryId);", conn);
      cmd.Parameters.Add(new SqlParameter("@IngredientId", this.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@CategoryId", newCategory.GetId().ToString()));
      cmd.ExecuteNonQuery();
      DB.CloseSqlConnection(conn);
    }

    public List<Category> GetCategory()
    {
      List<Category> allCategories = new List<Category>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT categories.* FROM ingredients JOIN ingredients_categories ON (ingredients.id = ingredients_categories.ingredient_id) JOIN categories ON (categories.id = ingredients_categories.category_id) WHERE ingredients.id = @IngredientId;", conn);
      cmd.Parameters.Add(new SqlParameter("@IngredientId", this.GetId().ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int Id = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, Id);
        allCategories.Add(newCategory);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allCategories;
    }

    public List<Recipe> GetRecipesByIngredient()
    {
      List<Recipe> allRecipes = new List<Recipe>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM ingredients JOIN recipes_ingredients ON (ingredients.id = recipes_ingredients.ingredient_id) JOIN recipes ON (recipes.id = recipes_ingredients.recipe_id) WHERE ingredients.id = @IngredientId;", conn);
      cmd.Parameters.Add(new SqlParameter("@IngredientId", this.GetId().ToString()));
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
