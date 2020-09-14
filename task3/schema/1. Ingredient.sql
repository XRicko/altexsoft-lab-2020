CREATE TABLE RecipeBook.dbo.Ingredient
(
	IngredientId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	IngredientName NVARCHAR(75) NOT NULL UNIQUE
);