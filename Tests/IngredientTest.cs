using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;


namespace RecipeApp
{
  public class IngredientTest: IDisposable
  {
    public IngredientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Category.DeleteAll();
      Recipe.DeleteAll();
      Ingredient.DeleteAll();
    }

    [Fact]
    public void GetAll_DatabaseEmptyAtFirst_ZeroOutput()
    {
      //Arrange, Act
      int result = Ingredient.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OverrideBool_SameIngredient_ReturnsEqual()
    {
      //Arrange, Act
      Ingredient ingredientOne = new Ingredient ("Pepper");
      Ingredient ingredientTwo = new Ingredient ("Pepper");

      //Assert
      Assert.Equal(ingredientTwo, ingredientOne);
    }

    [Fact]
    public void Save_OneIngredient_IngredientSavedToDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient ("Pepper");

      //Act
      testIngredient.Save();
      List<Ingredient> output = Ingredient.GetAll();
      List<Ingredient> verify = new List<Ingredient>{testIngredient};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Save_OneIngredient_IngredientSavedWithCorrectID()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient ("Pepper");
      testIngredient.Save();
      Ingredient savedIngredient = Ingredient.GetAll()[0];

      //Act
      int output = savedIngredient.GetId();
      int verify = testIngredient.GetId();

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void SaveGetAll_ManyIngredients_ReturnListOfIngredients()
    {
      //Arrange
      Ingredient ingredientOne = new Ingredient ("Pepper");
      ingredientOne.Save();
      Ingredient ingredientTwo = new Ingredient ("Bacon");
      ingredientTwo.Save();

      //Act
      List<Ingredient> output = Ingredient.GetAll();
      List<Ingredient> verify = new List<Ingredient>{ingredientOne, ingredientTwo};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Find_OneIngredientId_ReturnIngredientFromDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient ("Pepper");
      testIngredient.Save();

      //Act
      Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());

      //Assert
      Assert.Equal(testIngredient, foundIngredient);
    }

    [Fact]
    public void SearchName_Name_ReturnIngredientFromDatabase()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient ("Pepper");
      testIngredient.Save();

      //Act
      List<Ingredient> output = Ingredient.SearchName("Pepper");
      List<Ingredient> verify = new List<Ingredient>{testIngredient};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void AddRecipe_OneIngredient_RecipeAssociatedWithIngredientAddedToJoinTable()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient ("Pepper");
      testIngredient.Save();
      Recipe testRecipe = new Recipe("Pot Pie", "Microwave it");
      testRecipe.Save();
      testIngredient.AddRecipe(testRecipe);

      //Act
      List<Recipe> output = testIngredient.GetRecipesByIngredient();
      List<Recipe> verify = new List<Recipe>{testRecipe};

      //Assert
      Assert.Equal(verify, output);
    }

  }
}