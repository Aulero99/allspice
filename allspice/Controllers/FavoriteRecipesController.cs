namespace allspice.Controllers;
[Route("api/Favorites")]
    public class FavoriteRecipesController : Controller
    {
        private readonly FavoriteRecipesService _favoriteRecipesService;
        private readonly Auth0Provider _auth;

    public FavoriteRecipesController(FavoriteRecipesService favoriteRecipesService, Auth0Provider auth = null)
    {
        _favoriteRecipesService = favoriteRecipesService;
        _auth = auth;
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Favorite>> CreateFavorite([FromBody] Favorite favoriteData)
    {
        Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
        favoriteData.AccountId = userInfo.Id;
        Favorite favorite = _favoriteRecipesService.CreateFavorite(favoriteData);
        return new ActionResult<Favorite>(Ok(favorite)); 
    }

    [HttpDelete("{favoriteId}")]
    [Authorize]
    public async Task<ActionResult<string>> DeleteFavorite(int favoriteId)
    {
        try
            {
                Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
                _favoriteRecipesService.DeleteFavorite(favoriteId, userInfo.Id);
                return Ok($"Favorite id:{favoriteId} was deleted.");
            }
        catch (Exception e)
            {
              return BadRequest(e.Message);
            }
    }
}
