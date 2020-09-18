WITH Categories_CTE AS (
SELECT CategoryId, CategoryName, ParentId, CategoryId AS main_id 
FROM Category
UNION ALL
SELECT c.CategoryId, c.CategoryName, c.ParentId, Categories_CTE.main_id AS main_id 
FROM Category c
INNER JOIN Categories_CTE ON c.ParentId = Categories_CTE.CategoryId)

SELECT r.RecipeName, c.CategoryName, i.IngredientName, ri.Amount
FROM Recipe r
INNER JOIN RecipeIngredient ri ON r.RecipeId = ri.RecipeId
INNER JOIN Category c ON r.CategoryId = c.CategoryId AND r.CategoryId IN (SELECT Categories_CTE.CategoryId FROM Categories_CTE WHERE main_id = 3)
INNER JOIN Ingredient i ON ri.IngredientId = i.IngredientId
WHERE r.RecipeName IN (SELECT TOP 3 r1.RecipeName FROM Recipe r1 WHERE r1.CategoryId IN (SELECT Categories_CTE.CategoryId FROM Categories_CTE WHERE main_id = 3))
