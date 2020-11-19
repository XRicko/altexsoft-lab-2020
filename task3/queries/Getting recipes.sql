WITH Categories_CTE AS (
SELECT Id, Name, ParentId, Id AS main_id 
FROM Category
UNION ALL
SELECT c.Id, c.Name, c.ParentId, Categories_CTE.main_id AS main_id 
FROM Category c
INNER JOIN Categories_CTE ON c.ParentId = Categories_CTE.Id)

SELECT r.Name, c.Name, c.Id, i.Name, ri.Amount
FROM Recipe r
INNER JOIN RecipeIngredient ri ON r.Id = ri.RecipeId
INNER JOIN Category c ON r.CategoryId = c.Id
INNER JOIN Ingredient i ON ri.IngredientId = i.Id
WHERE r.Id IN (SELECT TOP 3 r1.Id FROM Recipe r1 INNER JOIN Categories_CTE cte ON r1.CategoryId = cte.Id AND cte.main_id = 3)