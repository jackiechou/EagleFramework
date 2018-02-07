CREATE PROCEDURE [dbo].[Menu_GetParentNodesOfSelectedNodeByMenuCode]
(@MenuCode uniqueidentifier)
AS
BEGIN
	WITH Hierarchy(PageId, TypeId, MenuId, MenuCode, ParentId, Depth, Lineage, ListOrder, MenuName, 
                         MenuTitle, MenuAlias,  Description, Target, IconFile, IconClass, Color, CssClass, Status, 
                         CreatedDate, LastModifiedDate, CreatedByUserId, LastModifiedByUserId, PagePath, PageUrl, IsExtenalLink)
	AS
	(
		SELECT        m.PageId, m.TypeId, m.MenuId, m.MenuCode, m.ParentId, m.Depth, m.Lineage, m.ListOrder, m.MenuName, 
                         m.MenuTitle, m.MenuAlias, m.Description, m.Target, m.IconFile,m.IconClass, m.Color, m.CssClass, m.Status, 
                         m.CreatedDate, m.LastModifiedDate, m.CreatedByUserId, m.LastModifiedByUserId, p.PagePath, p.PageUrl, p.IsExtenalLink
		FROM            [dbo].[Menu] as m INNER JOIN
                         [dbo].[Page] as p ON m.PageId = p.PageId WHERE m.MenuCode = @MenuCode 	
		
		UNION ALL

		SELECT        m.PageId, m.TypeId, m.MenuId, m.MenuCode, m.ParentId, m.Depth, m.Lineage, m.ListOrder, m.MenuName, 
                         m.MenuTitle, m.MenuAlias, m.Description, m.Target, m.IconFile,m.IconClass, m.Color, m.CssClass, m.Status, 
                         m.CreatedDate, m.LastModifiedDate, m.CreatedByUserId, m.LastModifiedByUserId, p.PagePath, p.PageUrl, p.IsExtenalLink
		FROM            [dbo].[Menu] as m INNER JOIN
                         [dbo].[Page] as p ON m.PageId = p.PageId
		INNER JOIN Hierarchy ON m.MenuId = Hierarchy.ParentId 
	)

	SELECT        PageId, TypeId, MenuId, MenuCode, ParentId, Depth, Lineage, ListOrder, MenuName, 
                         MenuTitle, MenuAlias,  Description, Target, IconFile,IconClass, Color, CssClass, Status, 
                         CreatedDate, LastModifiedDate, CreatedByUserId, LastModifiedByUserId, PagePath, PageUrl, IsExtenalLink
	FROM Hierarchy ORDER BY ListOrder ASC
END




