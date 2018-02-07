
create PROCEDURE [dbo].[Menu_GetList]
(@applicationId uniqueidentifier, @typeId int)
AS
BEGIN
	 SELECT CASE Depth
			 WHEN 0 THEN MenuName
			 WHEN 1 THEN '|__ '+ MenuName
			 WHEN 2 THEN '|__ __ '+ MenuName
			 WHEN 3 THEN '|__ __ __ ' + MenuName
			 WHEN 4 THEN '|__ __ __ __ ' +MenuName
			 WHEN 5 THEN '|__ __ __ __ __ __ ' +MenuName
			 WHEN 6 THEN '|__ __ __ __ __ __ __ ' +MenuName
			 WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +MenuName
			 WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +MenuName
			 WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +MenuName
			 WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +MenuName                         
			 END AS MenuName
		   ,CASE Depth
			 WHEN 0 THEN MenuTitle
			 WHEN 1 THEN '|__ '+ MenuTitle
			 WHEN 2 THEN '|__ __ '+ MenuTitle
			 WHEN 3 THEN '|__ __ __ ' + MenuTitle
			 WHEN 4 THEN '|__ __ __ __ ' +MenuTitle
			 WHEN 5 THEN '|__ __ __ __ __ __ ' +MenuTitle
			 WHEN 6 THEN '|__ __ __ __ __ __ __ ' +MenuTitle
			 WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +MenuTitle
			 WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +MenuTitle
			 WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +MenuTitle
			 WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +MenuTitle                         
			 END AS MenuTitle,   
		 [ApplicationId]
      ,[PositionId]
      ,[PageId]
      ,[MenuId]
      ,[TypeId]
      ,[MenuCode]
      ,[MenuName]
      ,[MenuTitle]
      ,[MenuAlias]
      ,[ParentId]     
      ,[Lineage]
      ,[ListOrder]
      ,[HasChild]
      ,[Description]
      ,[Target]
      ,[IconClass]
      ,[IconFile]
      ,[Color]
      ,[CssClass]
      ,[IsSecured]
      ,[Status]
      ,[CreatedDate]
      ,[CreatedByUserId]
      ,[LastModifiedDate]
      ,[LastModifiedByUserId]
  FROM [dbo].[Menu]
  WHERE ApplicationId = @applicationId and TypeId = @typeId
	  Order By [Lineage] ASC, [PageId] ASC
END

	 

	 
	 


