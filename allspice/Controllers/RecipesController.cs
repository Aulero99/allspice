namespace allspice.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RecipesController : Controller
{
    private readonly RecipeService _recipeService;
    private readonly IngredientsService _ingredientsService;
    private readonly Auth0Provider _auth;

    public RecipesController(RecipeService recipeService, Auth0Provider auth, IngredientsService ingredientsService = null)
    {
        _recipeService = recipeService;
        _auth = auth;
        _ingredientsService = ingredientsService;
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

    [HttpGet("{recipeId}")]
    public ActionResult<Recipe> GetRecipeById(int recipeId)
    {
        try
            {
                Recipe recipe = _recipeService.GetRecipeById(recipeId);
                return Ok(recipe);
            }
        catch (Exception e)
            {
              return BadRequest(e.Message);
            }
    }

    [HttpGet("{recipeId}/ingredients")]
    public ActionResult<List<Ingredient>> GetIngredientsByRecipeId(int recipeId)
    {
        try
            {
             List<Ingredient> ingredients = _ingredientsService.GetIngredientsByRecipeId(recipeId);
             return Ok(ingredients);  
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

    [HttpPut("{recipeId}")]
    [Authorize]
    public async Task<ActionResult<Recipe>> EditRecipe(int recipeId, [FromBody] Recipe recipeData)
    {
        try
            {
                Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
                recipeData.CreatorId = userInfo.Id;
                recipeData.Id = recipeId;
                Recipe recipe = _recipeService.EditRecipe(recipeData);
                return new ActionResult<Recipe>(Ok(recipe));

            }
        catch (Exception e)
            {
              return BadRequest(e.Message);
            }
    }

    [HttpDelete("{recipeId}")]
    [Authorize]
    public async Task<ActionResult<string>> DeleteRecipe(int recipeId)
    {
        try
            {
                Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
                _recipeService.DeleteRecipe(recipeId, userInfo.Id);
                return Ok($"Recipe Id:{recipeId} was deleted.");
            }
        catch (Exception e)
            {
              return BadRequest(e.Message);
            }
    } 

}
