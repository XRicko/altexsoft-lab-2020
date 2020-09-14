CREATE TABLE RecipeBook.dbo.RecipeIngredient
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RecipeId INT NOT NULL FOREIGN KEY REFERENCES RecipeBook.dbo.Recipe(RecipeId),
	IngredientId INT NOT NULL FOREIGN KEY REFERENCES RecipeBook.dbo.Ingredient(IngredientId),
	Amount NVARCHAR(50) NOT NULL
);