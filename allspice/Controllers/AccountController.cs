namespace allspice.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  private readonly AccountService _accountService;
  private readonly Auth0Provider _auth0Provider;
  private readonly FavoriteRecipesService _favoriteRecipesService;

    public AccountController(AccountService accountService, Auth0Provider auth0Provider, FavoriteRecipesService favoriteRecipesService = null)
    {
        _accountService = accountService;
        _auth0Provider = auth0Provider;
        _favoriteRecipesService = favoriteRecipesService;
    }

    [HttpGet]
  [Authorize]
  public async Task<ActionResult<Account>> Get()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      return Ok(_accountService.GetOrCreateProfile(userInfo));
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

[HttpGet("favorites")]
[Authorize]
public async Task<ActionResult<List<FavoriteRecipe>>> GetMyFavoriteRecipes()
{
  try
      {
        Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
        List<FavoriteRecipe> myFavoriteRecipes = _favoriteRecipesService.GetAccountFavoriteRecipes(userInfo.Id);
        return myFavoriteRecipes;
      }
  catch (Exception e)
      {
        return BadRequest(e.Message);
      }
}

}
