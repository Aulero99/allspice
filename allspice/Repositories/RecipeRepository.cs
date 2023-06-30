namespace allspice.Repositories
{
    public class RecipeRepository
    {
        private readonly IDbConnection _db;

        public RecipeRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Recipe CreateRecipe(Recipe recipeData)
        {
            string sql = @"
            INSERT INTO recipes
            (title, instructions, img, category, creatorId)
            VALUES 
            (@title, @instructions, @img, @category, @creatorId);

            SELECT
            r.*,
            c.*
            FROM recipes r
            JOIN accounts c ON r.creatorId = c.id
            WHERE r.id = LAST_INSERT_ID();
            ";

            Recipe recipe = _db.Query<Recipe, Account, Recipe>(sql, (recipe, creator) =>{
                recipe.Creator = creator;
                return recipe;
            }, recipeData).FirstOrDefault();
            return recipe;
        }

        internal List<Recipe> GetAllRecipes()
        {
            string sql = @"
            SELECT
            r.*,
            c.*
            FROM recipes r
            JOIN accounts c ON r.creatorId = c.id;";
            List<Recipe> recipes = _db.Query<Recipe, Account, Recipe>(sql,(recipe, creator) =>
            {
                recipe.Creator = creator;
                return recipe;
            }).ToList();
            return recipes;
        }
    }
}