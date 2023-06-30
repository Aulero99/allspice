CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) default charset utf8 COMMENT '';

CREATE TABLE
recipes(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  title VARCHAR(255) NOT NULL,
  instructions VARCHAR(3000) NOT NULL,
  img VARCHAR(255) NOT NULL,
  category VARCHAR(255) NOT NULL DEFAULT "Misc.",
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) default charset utf8 COMMENT '';

CREATE TABLE
ingredients(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(255) NOT NULL DEFAULT "Forgot set a name.",
  quantity VARCHAR(255) NOT NULL DEFAULT "1",
  recipeId INT NOT NULL,
  FOREIGN KEY (recipeId) REFERENCES recipes(id) ON DELETE CASCADE
) default charset utf8 COMMENT '';

CREATE TABLE
favoriteRecipes(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  accountId VARCHAR(255) NOT NULL,
  recipeId INT NOT NULL,
  FOREIGN KEY (accountId) REFERENCES accounts(id) ON DELETE CASCADE,
  FOREIGN KEY (recipeId) REFERENCES recipes(id) ON DELETE CASCADE
) default charset utf8 COMMENT '';

INSERT INTO
    recipes(
        title,
        instructions,
        img,
        category,
        creatorId
    )
VALUES (
        "Cheetos",
        "Open a bag of Cheetos",
        "https://i5.walmartimages.com/asr/067d572c-5628-4a26-9cb3-6930cd5433c4_2.f8a5108df6bd7ef4ac6ef5b2bbb2a7ba.jpeg",
        "Snacks",
        "646d28b8f893d37fb764429b"
    );

  SELECT * FROM recipes JOIN accounts ON creatorId;

  SELECT
  fav.*,
  rec.*,
  act.*
  FROM favoriteRecipes fav
  JOIN recipes rec ON fav.recipeId = rec.id
  JOIN accounts act ON fav.accountId = act.id
  WHERE fav.accountId = "646d28b8f893d37fb764429b"
  ;

  SELECT
  fav.*,
  rec.*,
  act.*
  FROM favoriteRecipes fav
  JOIN recipes rec ON fav.recipeId = rec.id
  JOIN accounts act ON fav.accountId = act.id
  WHERE fav.id = 1;