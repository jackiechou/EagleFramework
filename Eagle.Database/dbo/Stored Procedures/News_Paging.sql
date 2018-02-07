create PROCEDURE [dbo].[News_Paging]
@current_page int,
@record_per_page int,
@page_size int
AS
BEGIN  
    BEGIN
		WITH s AS
		(
			SELECT ROW_NUMBER() 
				OVER(ORDER BY NewsId,Headline) AS RowNum, 
				[NewsId],[CategoryId],[Title],[Headline],[Alias],[Summary],[FrontImage],[MainImage],[MainText],[ListOrder],[Tags],[Source],[TotalViews],[PostedDate],[CreatedDate],[Status],[CreatedByUserId],[LastModifiedByUserId],[Ip],[LastUpdatedIp]
			  FROM [dbo].[News] as x
		)
		Select * From s 
		Where RowNum Between 
			(@current_page - 1)*@record_per_page+1 
				AND @current_page*@record_per_page
    END
    
    -- Tính tổng số bản ghi
    DECLARE @total int
    SELECT @total=Count(*) FROM [dbo].[News]
    
    EXEC [dbo].[spPagingBySQL] @total, @current_page,@page_size, 10
END


