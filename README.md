# Recipe Database
#### _Site to view ingredients_

#### By _**Sam Kirsch && Minh Phuong**_

## Description

This website will allow the user to store and manage recipes and their associated ingredients

***

## Setup/Installation Requirements

#### Create Databases
* In `SQLCMD`:  
        `> CREATE DATABASE recipe`  
        `> GO`  
        `> USE recipe`  
        `> GO`  
        `> CREATE TABLE recipes (id INT IDENTITY(1,1), name VARCHAR(255), instruction VARCHAR(3500))`
        `> GO`  
        `> CREATE TABLE ingredients (id INT IDENTITY(1,1), name VARCHAR(255));`
        `> GO`  
        `> CREATE TABLE categories (id INT IDENTITY(1,1), name VARCHAR(255))`  
        `> GO`  
        `> CREATE TABLE recipes_ingredients (id INT IDENTITY(1,1), recipe_id INT, ingredient_id INT, amount VARCHAR(500))`  
        `> GO`  
        `> CREATE TABLE recipes_categories (id INT IDENTITY(1,1), recipe_id INT, category_id INT)`  
        `> GO`  
        `> CREATE TABLE ingredients_categories (id INT IDENTITY(1,1), ingredient_id INT, category_id INT);`  
        `> GO`

* Requires DNU, DNX, MSSQL, and Mono
* Clone to local machine
* Use command "dnu restore" in command prompt/shell
* Use command "dnx kestrel" to start server
* Navigate to http://localhost:5004 in web browser of choice

***

## Specifications

### Recipe Class
================  

**The DeleteAll method for the Recipe class will delete all rows from the recipes table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Recipe class will return an empty list if there are no entries in the Recipe table.**
* Example Input: N/A
* Example Output: empty list

**The Equals method for the Recipe class will return true if the Recipe in local memory matches the Recipe pulled from the database.**
* Example Input:  
        > Local: "Pot Pie", id is 1, instructions: "Microwave the package"
        > Database: "Pot Pie", id is 1, instructions: "Microwave the package"
* Example Output: `true`

**The Save method for the Recipe class will save new recipes to the database.**
* Example Input:  
\> New recipe: "Pot Pie"
* Example Output: no return value

**The Save method for the Recipe class will assign an id to each new instance of the Recipe class.**
* Example Input:  
\> New recipe: "Pot Pie", `local id: 0`  
* Example Output:  
\> "Pot Pie", `database-assigned id`  

**The GetAll method for the Recipe class will return all recipe entries in the database in the form of a list.**
* Example Input:  
        > "Pot Pie", id is 10  
        > "Instant Ramen", id is 11  
* Example Output: `{Pot Pie object}, {Instant Ramen object}`

**The Find method for the Recipe class will return the Recipe as defined in the database.**
* Example Input: "Pot Pie"
* Example Output: "Pot Pie", `database-assigned id`

**The Update method for the Recipe class will return the Recipe with the new recipe name and instructions.**
* Example Input: "Pot Pie", id is 1, instructions: "Microwave the package"
* Example Output: "Pot Pie", id is 1, instructions: "Microwave the package"

**The DeleteThis method for the Recipe class will return a list of Recipes without the deleted Ingredient.**
* Example Input: "Pot Pie"
* Example Output: "Instant Ramen"

**The Search method for the Recipe class will return a list of Recipes with matched name or date of enrollment.**
* Example Input: Search "Pot Pie"
* Example Output: "Pot Pie", id is 1, instructions: "Microwave the package"

### Ingredient class
================

**The DeleteAll method for the Ingredient class will delete all rows from the ingredients table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Ingredient class will return an empty list if there are no entries in the Ingredient table.**
* Example Input: N/A, automatically loads on home page
* Example Output: empty list

**The Equals method for the Ingredient class will return true if the Ingredient in local memory matches the Ingredient pulled from the database.**
* Example Input:  
        > Local: "Chicken", id is 10
        > Database: "Chicken", id is 10
* Example Output: `true`

**The Save method for the Ingredient class will save new Ingredients to the database.**
* Example Input:  
\> New ingredient: "Chicken"
* Example Output: no return value

**The Save method for the Ingredient class will assign an id to each new instance of the Ingredient class.**
* Example Input:  
\> New ingredient: "chicken", `local id: 0`  
* Example Output:  
\> "Chicken",`database-assigned id`  

**The GetAll method for the Ingredient class will return all ingredient entries in the database in the form of a list.**
* Example Input:  
        > "Chicken", id is 10  
        > "Beef", id is 11  
* Example Output: `{Chicken object}, {Beef object}`

**The Find method for the Ingredient class will return the Ingredient as defined in the database.**
* Example Input: "Chicken",
* Example Output: "Chicken", `database-assigned id`

**The Update method for the Ingredient class will return the Ingredient with the new ingredient name.**
* Example Input: "Chicken", id is 10
* Example Output: "Chicken", id is 10

**The DeleteThis method for the Ingredient class will return a list of Ingredients without the deleted Ingredient.**
* Example Input: "Chicken"
* Example Output: "Beef"

**The Search method for the Ingredient class will return a list of Ingredients with matched ingredient name or ingredient number.**
* Example Input: Search "Chic"
* Example Output: "Chicken", id is 10, "Chickpea", id is 13

### Recipe && Ingredient classes
===========================

**The GetIngredient method for the Recipe class will return the list of ingredients associated with that recipe.**
* Example Input: "Pot Pie"
* Example Output:  "Chicken", "Chickpea"

**The GetRecipe method for the Ingredient class will return the list of recipes associated with that ingredient.**
* Example Input: "Chicken"
* Example Output: "Pot Pie", "Instant Ramen"

**The Delete method for the Recipe class will delete the entry that connects the ingredient id with the recipe**
* Example Input: Delete "Pot Pie" from Recipe List
* Example Output: List of all ingredients excluding the combination of "Pot Pie" and its ingredients; leaving the ingredients table intact

### Category class
===========================

**The DeleteAll method for the Category class will delete all rows from the Category table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Category class will return an empty list if there are no entries in the Category table.**
* Example Input: N/A, automatically loads on home page
* Example Output: empty list

**The Equals method for the Category class will return true if the Category in local memory matches the Category pulled from the database.**
* Example Input:  
        > Local: "Peasant", id is 10  
        > Database: "Peasant", id is 10  
* Example Output: `true`

**The Save method for the Category class will save new Categories to the database.**
* Example Input:  
\> New Category: "Peasant"
* Example Output: no return value

**The Save method for the Category class will assign an id to each new instance of the Ingredient class.**
* Example Input:  
\> New Category: "Peasant", `local id: 0`  
* Example Output:  
\> "Peasant",`database-assigned id`  


**The GetCategory method for the Category class will return the list of categories.**
* Example Input:  
        > "Peasant", id is 10  
        > "History", id is 11  
* Example Output: `{Peasant object}, {Noodles object}`

**The GetRecipeCategories method for the Recipe class will return the list of category associated with that recipe.**
* Example Input: "Pot Pie"
* Example Output: "Peasant"

**The GetRecipes method for the Ingredient class will return the list of recipes associated with that ingredient.**
* Example Input: "Chicken"
* Example Output: "Pot Pie", "Instant Ramen"

**The Delete method for the Category class will delete the entry that connects the recipe id with the category**
* Example Input: Delete "Peasant" from categories
* Example Output: List of all recipes and their categories excluding the combination of "Peasant" and "Pot Pie"

### User Interface
===================  

**The user can add a new Recipe using the "Add Recipe" form.**
* Example Input: New Recipe: "Pot Pie", id is 1, instructions: "Microwave the package";
* Example Output: All Recipes Page: "Pot Pie, Instant Ramen, Beef Noodles"

**The user can add a new Ingredient using the "Add ingredient" form.**
* Example Input: New Ingredient: "Chicken", id is 10;
* Example Output: All Ingredients: "Chicken", "Noodles", "Salt"

**The user can click on any ingredient in the ingredients list to view the ingredient's details**
* Example Input: *click* "Chicken"
* Example Output: "Chicken", List of ingredient tags

**The user can click on any recipe to view a list of all ingredients in that the recipe and it's tags.**
* Example Input: *click* "Pot Pie"
* Example Output: "Pot Pie", list of ingredients in pot pie (eg Chicken), List of categories (Peasant)

**The user can edit a ingredient's ingredient name on the ingredient's page.**
* Example Input:  
    \> *click* "Chicken"  
    \> New ingredient name: "Chicken Breast"  
* Example Output: "Chicken Breast"

**The user can delete a ingredient using a link on the ingredient's page which will lead to a confirmation page.**
* Example Input:  
    \> *click* "Chicken"  
    \> *delete clicky*  
    \> *confirmation clicky*
* Example Output: Return to Search Ingredient Page

**The user can edit a recipe's name and instructions on the recipe's page.**
* Example Input:  
    \> *click* "Pot Pie"  
    \> New instructions: "Throw in pot and eat raw"  
* Example Output: "Chicken", "Throw in pot and eat raw"

**The user can delete a recipe using a link on the recipe's page which will lead to a confirmation page.**
* Example Input:  
    \> *click* "Pot Pie"  
    \> *delete clicky*  
    \> *confirmation clicky*
* Example Output: Return to Search Recipe Page

**The user can search using recipe name or category with the search form.**
* Example Input: "chic", "Peasant"
* Example Output: "Pot Pie"

**The user can search using ingredient name for an ingredient using the search form.**
* Example Input: "Chick"
* Example Output: "Chicken"

**The user can add a category to a recipe using selection form.**
* Example Input: "Pot Pie", "Delicious"
* Example Output: "Joe, "Delicious", "Peasant"

**The user can add a category to a ingredient using a selection form.**
* Example Input: "Chicken", "Meat"
* Example Output: "Chicken", "Meat"


***

## Support and contact details

Please contact Sam Kirsch at samdkirsch@gmail.com or Minh Phuong mphuong@kent.edu with any questions, concerns, or suggestions.

***

## Technologies Used

This web application uses:
* Nancy
* Mono
* DNVM
* C#
* Razor
* MSSQL & SSMS

***

### License

*This project is licensed under the MIT license.*

Copyright (c) 2017 **_Sam Kirsch && Minh Phuong_**
