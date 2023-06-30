namespace allspice.Services;
public class FavoriteRecipesService
{
    private readonly FavoriteRecipesRepository _repo;

    public FavoriteRecipesService(FavoriteRecipesRepository repo)
    {
        _repo = repo;
    }

    internal Favorite CreateFavorite(Favorite favoriteData)
    {
        Favorite favorite = _repo.CreateFavorite(favoriteData);
        return favorite;
    }

    internal void DeleteFavorite(int favoriteId, string accountId)
    {
        Favorite favorite = GetFavoriteById(favoriteId);
        if ( favorite.AccountId != accountId ) throw new Exception("Unauthorized: Cannot delete this favorite.");
        int rows = _repo.DeleteFavorite(favoriteId);
        if ( rows > 1 ) throw new Exception("Error: More than 1 row deleted.");
    }

    private Favorite GetFavoriteById(int favoriteId)
    {
        Favorite favorite = _repo.GetFavoriteById(favoriteId);
        if (favorite == null) throw new Exception($"No favorite id:{favoriteId} found.");
        return favorite;
    }

    internal List<FavoriteRecipe> GetAccountFavoriteRecipes(string accountId)
    {
        List<FavoriteRecipe> favoriteRecipes = _repo.GetAccountFavoriteRecipes(accountId);
        return favoriteRecipes;
    }
}
