WITH Recipes AS (
SELECT CategoryId, CategoryName, ParentId, CategoryId AS main_id 
FROM Category
UNION ALL
SELECT c.CategoryId, c.CategoryName, c.ParentId, Recipes.main_id AS main_id 
FROM Category c
INNER JOIN Recipes ON c.ParentId = Recipes.CategoryId)

SELECT r.RecipeName, c.CategoryName, i.IngredientName, ri.Amount
FROM Recipe r
INNER JOIN RecipeIngredient ri ON r.RecipeId = ri.RecipeId
INNER JOIN Category c ON r.CategoryId = c.CategoryId
INNER JOIN Ingredient i ON ri.IngredientId = i.IngredientId
WHERE r.CategoryId IN (SELECT TOP 3 Recipes.CategoryId FROM Recipes WHERE main_id = 3)