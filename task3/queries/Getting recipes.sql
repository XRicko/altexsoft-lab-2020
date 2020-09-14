WITH q AS (
SELECT CategoryId, CategoryName, ParentId, CategoryId AS main_id 
FROM RecipeBook.dbo.Category
UNION ALL
SELECT c.CategoryId, c.CategoryName, c.ParentId, q.main_id AS main_id 
FROM RecipeBook.dbo.Category c
INNER JOIN q ON c.ParentId = q.CategoryId)

SELECT r.RecipeName, c.CategoryName, i.IngredientName, ri.Amount
FROM RecipeBook.dbo.Recipe r
INNER JOIN RecipeBook.dbo.RecipeIngredient ri ON r.RecipeId = ri.RecipeId
INNER JOIN RecipeBook.dbo.Category c ON r.CategoryId = c.CategoryId
INNER JOIN RecipeBook.dbo.Ingredient i ON ri.IngredientId = i.IngredientId
WHERE r.CategoryId IN (SELECT TOP 3 q.CategoryId FROM q WHERE main_id = 3)