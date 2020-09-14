CREATE TABLE RecipeBook.dbo.Recipe
(
	RecipeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RecipeName NVARCHAR(75) NOT NULL UNIQUE,
	Description NVARCHAR(255) NOT NULL,
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES RecipeBook.dbo.Category(CategoryId),
	Instruction TEXT NOT NULL,
	DurationInMinutes DECIMAL NOT NULL
);