create PROCEDURE [dbo].[NewsCategory_spPagingTable] (
	@PageNumber int,
	@PageSize int
)

AS
	DECLARE @Start int, @End int
	BEGIN TRANSACTION GetDataSet
	SET @Start = (((@PageNumber - 1) * @PageSize) + 1)
	IF @@ERROR <> 0
		GOTO ErrorHandler
	SET @End = (@Start + @PageSize - 1)
	IF @@ERROR <> 0
		GOTO ErrorHandler
	CREATE TABLE #TemporaryTable (
		Row int IDENTITY(1,1) PRIMARY KEY,
		CategoryId int, CategoryName nvarchar(100)
	)
	IF @@ERROR <> 0
			GOTO ErrorHandler
	INSERT INTO #TemporaryTable
		SELECT CategoryId, CategoryName	FROM [dbo].[NewsCategory] 
	IF @@ERROR <> 0
		GOTO ErrorHandler
	SELECT CategoryId, CategoryName  
		FROM #TemporaryTable
		WHERE (Row >= @Start) AND (Row <= @End)
	IF @@ERROR <> 0
		GOTO ErrorHandler
	DROP TABLE #TemporaryTable
	COMMIT TRANSACTION GetDataSet
	RETURN 0
ErrorHandler:
ROLLBACK TRANSACTION GetDataSet
RETURN @@ERROR

-- Nếu bạn muốn hiển thị dữ liệu trang 1 và 20 bản ghi trên 1 trang
--EXEC spPhanTrang_Table (1,20)

