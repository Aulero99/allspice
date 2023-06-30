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
            rec.*,
            ctr.*
            FROM recipes rec
            JOIN accounts ctr ON rec.creatorId = ctr.id
            WHERE rec.id = LAST_INSERT_ID();
            ";

            Recipe recipe = _db.Query<Recipe, Account, Recipe>(sql, (recipe, creator) =>{
                recipe.Creator = creator;
                return recipe;
            }, recipeData).FirstOrDefault();
            return recipe;
        }

        internal int DeleteRecipe(int recipeId)
        {
            string sql = @"
            DELETE FROM recipes
            WHERE id = @recipeId
            LIMIT 1
            ;";
            int rows = _db.Execute(sql, new { recipeId });
            return rows;
        }

        internal void EditRecipe(Recipe repoRecipe)
        {
            string sql = @" 
            UPDATE recipes SET
            title = @title,
            instructions = @instructions,
            img = @img,
            category = @category
            WHERE  id = @id;";

            _db.Execute(sql, repoRecipe);
        }

        internal List<Recipe> GetAllRecipes()
        {
            string sql = @"
            SELECT
            rec.*,
            ctr.*
            FROM recipes rec
            JOIN accounts ctr ON rec.creatorId = ctr.id;";
            List<Recipe> recipes = _db.Query<Recipe, Account, Recipe>(sql,(recipe, creator) =>
            {
                recipe.Creator = creator;
                return recipe;
            }).ToList();
            return recipes;
        }

        internal Recipe GetRecipeById(int recipeId)
        {
            string sql = @"
            SELECT
            rec.*,
            ctr.*
            FROM
            recipes rec
            JOIN accounts ctr ON rec.creatorId = ctr.id
            WHERE
            rec.id = @recipeId;";
            Recipe recipe = _db.Query<Recipe, Account, Recipe>(sql, (rec,ctr) =>{
                rec.Creator = ctr;
                return rec;
            }, new { recipeId }).FirstOrDefault();
            return recipe;
        }
    }
}