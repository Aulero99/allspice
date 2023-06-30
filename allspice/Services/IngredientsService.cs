namespace allspice.Services
{
    public class IngredientsService
    {
        private readonly IngredientsRepository _repo;
        private readonly RecipeService _recipeService;

        public IngredientsService(IngredientsRepository repo, RecipeService recipeService)
        {
            _repo = repo;
            _recipeService = recipeService;
        }

        internal Ingredient CreateIngredient(Ingredient ingredientData, string userId)
        {
           Recipe recipe = _recipeService.GetRecipeById(ingredientData.RecipeId);
           if(recipe.CreatorId != userId) throw new Exception($"Unauthorized: User {userId} is not authorized to add this ingredient.");
           Ingredient ingredient = _repo.CreateIngredient(ingredientData);
           return ingredient;
        }

        internal void DeleteIngredient(int ingredientId, string userId)
        {
            Ingredient ingredient = GetIngredientById(ingredientId);
            Recipe recipe = _recipeService.GetRecipeById(ingredient.RecipeId);
            if(recipe.CreatorId != userId) throw new Exception($"Unauthorized: User {userId} is not authorized to add this ingredient.");
            int rows = _repo.DeleteIngredient(ingredientId);
            if ( rows > 1 ) throw new Exception("Error: More than 1 row deleted.");
        }

        private Ingredient GetIngredientById(int ingredientId)
        {
            Ingredient ingredient = _repo.GetIngredientById(ingredientId);
            return ingredient;
        }

        internal List<Ingredient> GetIngredientsByRecipeId(int recipeId)
        {
            List<Ingredient> ingredients = _repo.GetIngredientsByRecipeId(recipeId);
            if (ingredients == null) throw new Exception($"Error: Recipe id:{recipeId} does not contain any ingredients.");
            return ingredients;
        }
    }
}