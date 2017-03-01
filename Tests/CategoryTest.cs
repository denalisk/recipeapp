using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;


namespace RecipeApp
{
  public class CategoryTest: IDisposable
  {
    public CategoryTest()
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
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void OverrideBool_SameCategory_ReturnsEqual()
    {
      //Arrange, Act
      Category categoryOne = new Category ("Peasant");
      Category categoryTwo = new Category ("Peasant");

      //Assert
      Assert.Equal(categoryTwo, categoryOne);
    }

    [Fact]
    public void Save_OneCategory_CategorySavedToDatabase()
    {
      //Arrange
      Category testCategory = new Category ("Peasant");

      //Act
      testCategory.Save();
      List<Category> output = Category.GetAll();
      List<Category> verify = new List<Category>{testCategory};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Save_OneCategory_CategorySavedWithCorrectID()
    {
      //Arrange
      Category testCategory = new Category ("Peasant");
      testCategory.Save();
      Category savedCategory = Category.GetAll()[0];

      //Act
      int output = savedCategory.GetId();
      int verify = testCategory.GetId();

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void SaveGetAll_ManyCategories_ReturnListOfCategories()
    {
      //Arrange
      Category categoryOne = new Category ("Peasant");
      categoryOne.Save();
      Category categoryTwo = new Category ("Delicious");
      categoryTwo.Save();

      //Act
      List<Category> output = Category.GetAll();
      List<Category> verify = new List<Category>{categoryOne, categoryTwo};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Find_OneCategoryId_ReturnCategoryFromDatabase()
    {
      //Arrange
      Category testCategory = new Category ("Peasant");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void SearchName_Name_ReturnCategoryFromDatabase()
    {
      //Arrange
      Category testCategory = new Category ("Peasant");
      testCategory.Save();

      //Act
      List<Category> output = Category.SearchName("Peasant");
      List<Category> verify = new List<Category>{testCategory};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void AddRecipe_OneCategory_RecipeAddedToJoinTable()
    {
      //Arrange
      Category testCategory = new Category ("Peasant");
      testCategory.Save();
      Recipe testRecipe = new Recipe("Pot Pie", "Microwave it");
      testRecipe.Save();
      testCategory.AddRecipe(testRecipe);

      //Act
      List<Recipe> output = testCategory.GetRecipesByCategory();
      List<Recipe> verify = new List<Recipe>{testRecipe};

      //Assert
      Assert.Equal(verify, output);
    }

    [Fact]
    public void Category_Delete_RemoveObjectFromDatabase()
    {
      Category testCategory = new Category ("Peasant");
      testCategory.Save();

      testCategory.Delete();

      Assert.Equal(0, Category.GetAll().Count);
    }

    [Fact]
    public void Category_Update_UpdateDatabaseAndLocalObject()
    {
      Category testCategory = new Category ("Peasant");
      testCategory.Save();

      testCategory.Update("Ultra Poor");
      Category expectedCategory = new Category("Ultra Poor", testCategory.GetId());

      Assert.Equal(expectedCategory, Category.Find(testCategory.GetId()));
    }






































  }
}
