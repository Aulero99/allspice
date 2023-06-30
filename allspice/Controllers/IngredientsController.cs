namespace allspice.Controllers
{
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        private readonly IngredientsService _ingredientsService;
        private readonly Auth0Provider _auth;

        public IngredientsController(IngredientsService ingredientsService, Auth0Provider auth = null)
        {
            _ingredientsService = ingredientsService;
            _auth = auth;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Ingredient>> CreateIngredient([FromBody] Ingredient IngredientData)
        {
            try
                {
                    Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
                    Ingredient ingredient = _ingredientsService.CreateIngredient(IngredientData, userInfo.Id);
                    return new ActionResult<Ingredient>(Ok(ingredient));
                    
                }
            catch (Exception e)
                {
                  return BadRequest(e.Message);
                }
        }

        [HttpDelete("{ingredientId}")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteIngredient(int ingredientId)
        {
            try
                {
                    Account userInfo = await _auth.GetUserInfoAsync<Account>(HttpContext);
                    _ingredientsService.DeleteIngredient(ingredientId, userInfo.Id);
                    return Ok($"Ingredient Id:{ingredientId} was deleted.");

                }
            catch (Exception e)
                {
                  return BadRequest(e.Message);
                }
        }
    }
}