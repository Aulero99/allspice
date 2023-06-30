namespace allspice.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RecipesController : Controller
{
    private readonly RecipeService _recipeService;
    private readonly Auth0Provider _auth;

    public RecipesController(RecipeService recipeService, Auth0Provider auth)
    {
        _recipeService = recipeService;
        _auth = auth;
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetAllRecipes()
    {
        try
            {
                List<Recipe> recipes = _recipeService.GetAllRecipes();
                return Ok(recipes);
            }
        catch (Exception e)
            {
                return BadRequest(e.Message);
            }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipeData)
    {
        try
            {
                Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
                recipeData.CreatorId = userInfo.Id;
                Recipe recipe = _recipeService.CreateRecipe(recipeData);
                return new ActionResult<Recipe>(Ok(recipe));           
            }
        catch (Exception e)
            {
              return BadRequest(e.Message);
            }
    }
}
