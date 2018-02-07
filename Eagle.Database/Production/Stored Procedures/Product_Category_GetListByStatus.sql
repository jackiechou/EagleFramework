CREATE PROCEDURE [Production].[Product_Category_GetListByStatus]( @languageCode varchar(5), @vendorId int, @status int = NULL)
AS
BEGIN
	SELECT [CategoryId]
			,[CategoryCode]
			,[CategoryAlias]		
			,[CategoryLink]							 
			,ISNULL(ParentId, 0) ParentId
			,CASE  Depth 
				WHEN 0 THEN CategoryName
				WHEN 1 THEN '|__ '+ CategoryName
				WHEN 2 THEN '|__ __ '+ CategoryName
				WHEN 3 THEN '|__ __ __ ' + CategoryName
				WHEN 4 THEN '|__ __ __ __ ' +CategoryName
				WHEN 5 THEN '|__ __ __ __ __ __ ' +CategoryName
				WHEN 6 THEN '|__ __ __ __ __ __ __ ' +CategoryName
				WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +CategoryName
				WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +CategoryName
				WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +CategoryName
				WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +CategoryName                         
				END AS CategoryName  
			,[Depth]
			,[Lineage]
			,[ViewOrder]
			,[VendorId]
			,[Icon]		 
			,[Description]
			,[Status]
			,[CreatedDate]
			,[LastModifiedDate]
	FROM [Production].[Product_Category]	
	WHERE VendorId = @vendorId and (@status is null or [Status]=@status)
	order by [LastModifiedDate] desc, [ViewOrder] desc
END


