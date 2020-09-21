WITH Categories_CTE AS (
SELECT CategoryId, CategoryName, ParentId, CategoryId AS main_id 
FROM Category
UNION ALL
SELECT c.CategoryId, c.CategoryName, c.ParentId, Categories_CTE.main_id AS main_id 
FROM Category c
INNER JOIN Categories_CTE ON c.ParentId = Categories_CTE.CategoryId)

SELECT r.RecipeName, c.CategoryName, c.CategoryId, i.IngredientName, ri.Amount
FROM Recipe r
INNER JOIN RecipeIngredient ri ON r.RecipeId = ri.RecipeId
INNER JOIN Category c ON r.CategoryId = c.CategoryId
INNER JOIN Ingredient i ON ri.IngredientId = i.IngredientId
WHERE r.RecipeId IN (SELECT TOP 3 r1.RecipeId FROM Recipe r1 INNER JOIN Categories_CTE cte ON r1.CategoryId = cte.CategoryId AND cte.main_id = 3)