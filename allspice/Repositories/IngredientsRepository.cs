namespace allspice.Repositories
{
    public class IngredientsRepository
    {
        private readonly IDbConnection _db;

        public IngredientsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Ingredient CreateIngredient(Ingredient ingredientData)
        {
            string sql = @"
            INSERT INTO ingredients
            (quantity, name, recipeId)
            VALUES
            (@quantity, @name, @recipeId);

            SELECT
            i.*,
            r.*
            FROM ingredients i
            JOIN recipes r ON i.recipeId = r.id
            WHERE i.id = LAST_INSERT_ID();";

            Ingredient ingredient = _db.Query<Ingredient, Recipe, Ingredient>(sql, (i, r)=>{
                i.Recipe = r;
                return i;
            }, ingredientData).FirstOrDefault();
            return ingredient;
        }

        internal int DeleteIngredient(int ingredientId)
        {
            string sql = @"
            DELETE FROM ingredients
            WHERE id = @ingredientId
            LIMIT 1
            ;";
            int rows = _db.Execute(sql, new { ingredientId });
            return rows;
        }

        internal Ingredient GetIngredientById(int ingredientId)
        {
            string sql = @"
            SELECT
            i.*,
            r.*
            FROM ingredients i
            JOIN recipes r ON i.recipeId = r.id
            WHERE i.Id = @ingredientId;";

            Ingredient ingredient = _db.Query<Ingredient, Recipe, Ingredient>(sql,(i, r)=>{
                i.Recipe = r;
                return i; 
            }, new { ingredientId }).FirstOrDefault();
            return ingredient;
        }

        internal List<Ingredient> GetIngredientsByRecipeId(int recipeId)
        {
            string sql = @"
            SELECT
            i.*,
            r.*
            FROM ingredients i
            JOIN recipes r ON i.recipeId = r.id
            WHERE i.recipeId = @recipeId;";

            List<Ingredient> ingredients = _db.Query<Ingredient, Recipe, Ingredient>(sql,(i, r)=>{
                i.Recipe = r;
                return i; 
            }, new { recipeId }).ToList();
            return ingredients;
        }
    }
}