create PROC [dbo].[Menu_UpdateListOrder] 
(	
	@MenuId int,
	@ParentId int,
	@ListOrder int,	
	@o_return	int output
)					
AS 
BEGIN
IF (@MenuId < 0 or  @ParentId < 0)
	begin
		set @o_return = -1 -- thong tin khong day du  
	end
 ELSE
	BEGIN
		IF EXISTS (SELECT MenuId FROM Cms.Menu WHERE MenuId = @MenuId)       
		BEGIN   
			declare @_Level int	
			declare @_Lineage varchar(200)					
			declare @i int	
			
			select @_Lineage = Lineage, @_Level = Depth from [dbo].[Menu] where MenuId = @MenuId
			set @i = len(@_Lineage)
			select Lineage from [dbo].[Menu] where MenuId = @ParentId

			UPDATE [dbo].[Menu]
				SET [ParentId] = @ParentId
					,[Depth] = ( select [Depth] + 1 from [dbo].[Menu] where MenuId = @ParentId )
					,[Lineage] = ( select Lineage from [dbo].[Menu] where MenuId = @ParentId ) + convert(varchar(4), @MenuId) + ','				 
					,[ListOrder] =@ListOrder					
					,[LastModifiedDate] = GetDate()
				WHERE MenuId = @MenuId

				if ( @@rowcount = 1 )
				begin					
					declare @Level int	
					declare @Lineage varchar(200)
					
					select @Lineage = Lineage, @Level = [Depth] from [dbo].[Menu] where MenuId = @MenuId

					update [dbo].[Menu] 
						set Lineage = @Lineage + right(Lineage, len(Lineage) - @i)
							,[Depth] = Depth + @Level - @_Level
					where Lineage like @_Lineage + '%'

					set @o_return = 1 -- thanh cong
				  end
				else
					set @o_return = -1 -- loi update
		END
		ELSE
			SET @o_return=-3
	END
END





















