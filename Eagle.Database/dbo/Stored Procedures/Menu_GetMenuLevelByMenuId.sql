create procedure [dbo].[Menu_GetMenuLevelByMenuId](@MenuId int, @Depth int output)
as
begin
	with cte as (
		select MenuId, ParentId, MenuName, Depth from [dbo].[Menu] where MenuId = @MenuId
		union all
		select    C.MenuId, C.ParentId, C.MenuName, C.Depth
		from    [dbo].[Menu] as C
			inner join cte as P -- here it joins against itself
					on C.ParentId = P.MenuId    
					where C.MenuId =@MenuId
	)
	select @Depth = Depth from cte
	return @Depth
end



