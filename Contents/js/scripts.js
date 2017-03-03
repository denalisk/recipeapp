var ingredientCounter;
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
  ingredientCounter = $("#ingredient-counter").val();
  $("form.entry").submit(function(event){
    // $("input").defaultValue = "";
    $(this).children("input").each(function() {
      if ($(this).val() === 0 || $(this).val() === "") {
        event.preventDefault();
        $(".invalid-entry").show();
        return false;
      }
    });
  });
  $("#new-ingredient-btn").click(function(){
    newIngredientDiv("#ingredient-1", "#ingredients-div");
  });
  if ($("body").has(".rating").length === 1) {
    var oldRating = "#star" + $("#old-rating").val().toString();
    $(oldRating).attr("checked", true);
  }
  // Materialize Initializations
  $("select").material_select();
  $('.collapsible').collapsible();
})

// if (!($.contains($(this).parent(), ".invalid-entry"))) {
//   $(this).parent().append($(".invalid-entry").clone().removeClass("hidden"))
// }
