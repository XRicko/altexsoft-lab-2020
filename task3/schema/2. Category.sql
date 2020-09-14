CREATE TABLE RecipeBook.dbo.Category
(
	CategoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CategoryName NVARCHAR(75) NOT NULL UNIQUE,
	ParentId INT FOREIGN KEY REFERENCES RecipeBook.dbo.Category(CategoryId)
);