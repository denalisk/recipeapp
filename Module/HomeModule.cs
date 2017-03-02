using Nancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace RecipeApp
{
  public class HomeModule: NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/recipes"] = _ => {
        return View["recipes.cshtml",ModelMaker()];
      };

      Post["/recipes"] = _ => {
        Recipe newRecipe = new Recipe(Request.Form["recipe-name"],Request.Form["instruction"],Request.Form["rating"]);
        newRecipe.Save();
        int ingredientCounter = int.Parse(Request.Form["ingredient-counter"]);
        for(int index = 1; index <= ingredientCounter; index++)
        {
          Ingredient newIngredient = new Ingredient(Request.Form["ingredient-" + index.ToString()]);
          newIngredient.Save();
          string newAmount = Request.Form["ingredient-amount-" + index.ToString()];
          newRecipe.AddIngredient(newIngredient, newAmount);
        }
        return View["recipes.cshtml", ModelMaker()];
      };

      Get["/recipes/{id}"] = parameters => {
        Dictionary<string, object> model = ModelMaker();
        model.Add("recipe", Recipe.Find(parameters.id));
        return View["recipe.cshtml", model];
      };

      Post["/recipes/{id}/add-category"] = parameters => {
        Recipe foundRecipe = Recipe.Find(parameters.id);
        Category foundCategory = Category.Find(Request.Form["new-category"]);
        foundRecipe.AddCategory(foundCategory);
        Dictionary<string, object> model = ModelMaker();
        model.Add("recipe", Recipe.Find(parameters.id));
        return View["recipe.cshtml", model];
      };

      Get["/categories"] = _ => {
        return View["categories.cshtml", ModelMaker()];
      };

      Post["/categories"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["categories.cshtml", ModelMaker()];
      };

      Get["/categories/{id}"] = parameters => {
        Category foundCategory = Category.Find(parameters.id);
        return View["category.cshtml", foundCategory];
      };


      Get["/ingredients"] = _ => {
        return View["ingredients.cshtml", ModelMaker()];
      };

      Post["/ingredients"] = _ => {
        Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"]);
        newIngredient.Save();
        return View["ingredients.cshtml", ModelMaker()];
      };

      Get["/ingredients/{id}"] = parameters => {
        Ingredient foundIngredient = Ingredient.Find(parameters.id);
        return View["ingredient.cshtml", foundIngredient];
      };



    }

    public static Dictionary<string, object> ModelMaker()
    {
      Dictionary<string, object> model = new Dictionary<string, object>
      {
        {"recipes", Recipe.GetAll()},
        {"categories", Category.GetAll()},
        {"ingredients", Ingredient.GetAll()}
      };
      return model;
    }
  }
}
