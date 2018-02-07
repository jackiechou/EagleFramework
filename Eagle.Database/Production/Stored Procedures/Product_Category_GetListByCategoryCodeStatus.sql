CREATE PROCEDURE [Production].[Product_Category_GetListByCategoryCodeStatus] (@languageCode varchar(5), @vendorId int, @categoryCode varchar(10), @status int=NULL)
AS
BEGIN
		WITH Descendants AS (
				SELECT  p.CategoryId, p.CategoryName, p.CategoryCode, p.LanguageCode, p.ParentId, 1 AS Depth, p.Status, p.VendorId
				FROM [Production].[Product_Category] p  
				WHERE CategoryCode = @categoryCode
			 UNION ALL
				SELECT  p.CategoryId, p.CategoryName, p.CategoryCode, p.LanguageCode, p.ParentId, H.Depth+1, p.Status, p.VendorId
				FROM [Production].[Product_Category] p INNER JOIN Descendants  H ON H.ParentId=p.CategoryId  
			)  
			SELECT CategoryId, CategoryCode, REPLICATE('- ', Depth) + CategoryName AS CategoryName, ParentId, Depth, Status, VendorId
			FROM Descendants d			
			WHERE d.VendorId = @vendorId and (@languageCode ='' or d.LanguageCode = @languageCode) and (@status ='' or d.Status = @status)
			ORDER BY d.Depth ASC
END


