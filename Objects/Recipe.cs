using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeApp
{
  public class Recipe
  {
    private int _id;
    private string _name;
    private string _instruction;

    public Recipe(string recipeName, string instruction, int recipeId = 0)
    {
      _id = recipeId;
      _name = recipeName;
      _instruction = instruction;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetInstruction()
    {
      return _instruction;
    }

    public static void DeleteAll()
    {
      DB.DeleteAll("recipes");
    }

    public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        string instruction = rdr.GetString(2);
        Recipe newRecipe = new Recipe(recipeName, instruction, recipeId);
        allRecipes.Add(newRecipe);
      }

      DB.CloseSqlConnection(conn, rdr);

      return allRecipes;
    }

    public override bool Equals(System.Object randomRecipe)
    {
      if(!(randomRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) randomRecipe;
        bool idEquality = (this.GetId() == newRecipe.GetId());
        bool nameEquality = (this.GetName() == newRecipe.GetName());
        bool instructionEquality = (this.GetInstruction() == newRecipe.GetInstruction());
        return (idEquality && nameEquality && instructionEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes (name, instruction) OUTPUT INSERTED.id VALUES (@RecipeName, @Instruction) ;", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeName", this.GetName()));
      cmd.Parameters.Add(new SqlParameter("@Instruction", this.GetInstruction()));
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

      SqlCommand cmd = new SqlCommand("DELETE FROM recipes WHERE id = @TargetId; DELETE FROM recipes_categories WHERE recipe_id = @TargetId; DELETE FROM recipes_ingredients WHERE recipe_id = @TargetId;", conn);
      cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

      cmd.ExecuteNonQuery();
      DB.CloseSqlConnection(conn);
    }

    public static Recipe Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", id.ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      int recipeId = 0;
      string recipeName = null;
      string recipeInstruction = null;

      while (rdr.Read())
      {
        recipeId = rdr.GetInt32(0);
        recipeName = rdr.GetString(1);
        recipeInstruction = rdr.GetString(2);
      }

      Recipe foundRecipe = new Recipe(recipeName, recipeInstruction, recipeId);

      DB.CloseSqlConnection(conn, rdr);

      return foundRecipe;
    }

    public static List<Recipe> SearchName(string recipeName)
    {
      List<Recipe> foundRecipes = new List<Recipe>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE name LIKE @RecipeName", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeName", "%" + recipeName + "%"));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        string recipeInstruction = rdr.GetString(2);
        Recipe foundRecipe = new Recipe(newName, recipeInstruction, recipeId);
        foundRecipes.Add(foundRecipe);
      }

      DB.CloseSqlConnection(conn, rdr);
      return foundRecipes;
    }

    public void AddCategory(Category newCategory)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_categories (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@CategoryId", newCategory.GetId().ToString()));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }


    public List<Category> GetCategory()
    {
      List<Category> allCategories = new List<Category>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT categories.* FROM recipes JOIN recipes_categories ON (recipes.id = recipes_categories.recipe_id) JOIN categories ON (categories.id = recipes_categories.category_id) WHERE recipes.id = @RecipeId;", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId().ToString()));
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

    public void AddIngredient(Ingredient newIngredient, string amount)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_ingredients (recipe_id, ingredient_id, amount) VALUES (@RecipeId, @IngredientId,@Amount);", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@IngredientId", newIngredient.GetId().ToString()));
      cmd.Parameters.Add(new SqlParameter("@Amount", amount));
      cmd.ExecuteNonQuery();

      DB.CloseSqlConnection(conn);
    }


    public List<Ingredient> GetIngredient()
    {
      List<Ingredient> allIngredients = new List<Ingredient>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT ingredients.*, recipes_ingredients.amount FROM recipes JOIN recipes_ingredients ON (recipes.id = recipes_ingredients.recipe_id) JOIN ingredients ON (ingredients.id = recipes_ingredients.ingredient_id) WHERE recipes.id = @RecipeId;", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId().ToString()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int Id = rdr.GetInt32(0);
        string ingredientName = rdr.GetString(1);
        string ingredientAmount = rdr.GetString(2);
        Ingredient newIngredient = new Ingredient(ingredientName, Id, ingredientAmount);
        allIngredients.Add(newIngredient);
      }

      DB.CloseSqlConnection(conn, rdr);
      return allIngredients;
    }
  }
}
