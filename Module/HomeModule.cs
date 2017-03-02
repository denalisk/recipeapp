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
        return View["recipes.cshtml", ModelMaker()];
      };

      Get["/categories"] = _ => {
        return View["categories.cshtml", ModelMaker()];
      };

      Post["/categories"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["categories.cshtml", ModelMaker()];
      };

      Get["/ingredients"] = _ => {
        return View["ingredients.cshtml", ModelMaker()];
      };

      Post["/ingredients"] = _ => {
        Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"]);
        newIngredient.Save();
        return View["ingredients.cshtml", ModelMaker()];
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
