namespace allspice.Repositories
{
    public class FavoriteRecipesRepository
    {
        private readonly IDbConnection _db;

        public FavoriteRecipesRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Favorite CreateFavorite(Favorite favoriteData)
        {
            string sql = @"
            INSERT INTO favoriteRecipes
            (accountId, recipeId)
            VALUES
            (@accountId, @recipeId);
            SELECT
            LAST_INSERT_ID()
            ;";

            int lastInsertId = _db.ExecuteScalar<int>(sql, favoriteData);
            favoriteData.Id = lastInsertId;
            return favoriteData;
        }

        internal int DeleteFavorite(int favoriteId)
        {
            string sql = @"
            DELETE FROM favoriteRecipes
            WHERE id = @favoriteId
            LIMIT 1
            ;";
            int rows = _db.Execute(sql, new { favoriteId });
            return rows;
        }

        internal List<FavoriteRecipe> GetAccountFavoriteRecipes(string accountId)
        {
            string sql = @"
            SELECT
            fav.*,
            rec.*,
            act.*
            FROM favoriteRecipes fav
            JOIN recipes rec ON fav.recipeId = rec.id
            JOIN accounts act ON fav.accountId = act.id
            WHERE fav.accountId = @accountId
            ;";
            List<FavoriteRecipe> favRecipes = _db.Query<Favorite, FavoriteRecipe, Account, FavoriteRecipe>(sql, (fav, rec, act) =>{
                rec.FavoriteId = fav.Id;
                rec.Creator = act;
                return rec;
            }, new { accountId }).ToList();
            return favRecipes; 
        }

        internal Favorite GetFavoriteById(int favoriteId)
        {
            // string sql = @"
            // SELECT
            // fav.*,
            // rec.*,
            // act.*
            // FROM favoriteRecipes fav
            // JOIN recipes rec ON fav.recipeId = rec.id
            // JOIN accounts act ON fav.accountId = act.id
            // WHERE fav.id = @favoriteId
            // ;";
            // Favorite favorite = _db.Query<Favorite, Account, Recipe, Favorite>(sql, (fav, act, rec)=>{
            //     fav.Account = act;
            //     fav.Recipe = rec;
            //     return fav;
            // }, new { favoriteId }).FirstOrDefault();
            // return favorite;

            string sql = @"
            SELECT *
            FROM favoriteRecipes
            WHERE id = @favoriteId
            ;";

            Favorite favorite = _db.Query<Favorite>(sql, new { favoriteId }).FirstOrDefault();
            return favorite;
        }
    }
}