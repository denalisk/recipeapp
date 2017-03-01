using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeApp
{
  public class Ingredient
  {
    private int _id;
    private string _name;

    public Ingredient(string ingredientName, int ingredientId = 0)
    {
      _id = ingredientId;
      _name = ingredientName;
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
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
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

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

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
        return (idEquality && nameEquality);
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

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
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

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

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

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

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

      if (conn != null)
      {
        conn.Close();
      }
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

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allRecipes;
    }
  }
}
