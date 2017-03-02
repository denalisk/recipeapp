var ingredientCounter = 1;

var newIngredientDiv = function(targetElement, parentElement) {
  ingredientCounter++;
  $("#ingredient-counter").attr("value", ingredientCounter);
  var newElementId = "ingredient-" + ingredientCounter.toString();
  var newElement = $(targetElement).clone().attr('id', newElementId);
  $(parentElement).append(newElement);
  $("#" + newElementId + " .ingredient-name-label").attr("for", newElementId);
  $("#" + newElementId + " .ingredient-name").attr("name", newElementId).val("");
  $("#" + newElementId + " .ingredient-amount-label").attr("for", "ingredient-amount-" + ingredientCounter.toString());
  $("#" + newElementId + " .ingredient-amount").attr("name", "ingredient-amount-" + ingredientCounter.toString()).val("");
}

$(document).ready(function(){
  $("form").submit(function(event){
    $("input").each(function() {
      if ($(this).val() === 0 || $(this).val() === "") {
        event.preventDefault();
        $(this).parent().append($("#invalid").clone().removeClass("hidden-div"));
        return false;
      }
    });
  });
  $("select").material_select();
  $("#new-ingredient-btn").click(function(){
    newIngredientDiv("#ingredient-1", "#ingredients-div");
  });
})
