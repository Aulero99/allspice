namespace allspice.Services;

public class RecipeService
{
    private readonly RecipeRepository _repo;

    public RecipeService(RecipeRepository repo)
    {
        _repo = repo;
    }

    internal Recipe CreateRecipe(Recipe recipeData)
    {
        Recipe recipe = _repo.CreateRecipe(recipeData);
        return recipe;
    }

    internal void DeleteRecipe(int recipeId, string accountId)
    {
        Recipe repoRecipe = GetRecipeById(recipeId);
        if ( repoRecipe.CreatorId != accountId ) throw new Exception("Unauthorized: Cannot delete this recipe.");
        int rows = _repo.DeleteRecipe(recipeId);
        if ( rows > 1 ) throw new Exception("Error: More than 1 row deleted.");

    }

    internal Recipe EditRecipe(Recipe recipeData)
    {
        Recipe repoRecipe = GetRecipeById(recipeData.Id);
        if ( repoRecipe.CreatorId != recipeData.CreatorId ) throw new Exception("Unauthorized: Cannot edit this recipe.");
        
        repoRecipe.Title = recipeData.Title != null ? recipeData.Title : repoRecipe.Title;
        repoRecipe.Instructions = recipeData.Instructions != null ? recipeData.Instructions : repoRecipe.Instructions;
        repoRecipe.Img = recipeData.Img != null ? recipeData.Img : repoRecipe.Img;
        repoRecipe.Category = recipeData.Category != null ? recipeData.Category : repoRecipe.Category;

        _repo.EditRecipe(repoRecipe);
        return repoRecipe;

    }

    internal List<Recipe> GetAllRecipes()
    {
        List<Recipe> recipes = _repo.GetAllRecipes();
        return recipes;
    }

    internal Recipe GetRecipeById(int recipeId)
    {
        Recipe recipe = _repo.GetRecipeById(recipeId);
        if (recipe == null) throw new Exception($"No recipe id:{recipeId} found.");
        return recipe;
    }
}
