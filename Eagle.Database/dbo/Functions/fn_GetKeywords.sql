CREATE FUNCTION [dbo].[fn_GetKeywords]
(
	@NoiDung nvarchar(4000)
)
RETURNS NVarChar(4000) 
AS
BEGIN
	DECLARE @KetQua nvarchar(4000) SET @KetQua='Từ khóa: '
	-- Khai báo 1 bảng tạm
	DECLARE @_Table TABLE  (Noidung nvarchar(4000) null)
	INSERT INTO @_Table(Noidung) VALUES (@NoiDung)
	DECLARE @Count int
	--Dùng con trỏ (vòng lặp) để tìm kiếm từ khóa
	DECLARE @TuKhoa Nvarchar(250) 
	DECLARE TuKhoa_Cursor CURSOR FOR 
	----------------------------------------------------------	
	SELECT [Tags] FROM [Articles].[Articles]
	----------------------------------------------------------		
 	OPEN TuKhoa_Cursor 
	FETCH NEXT FROM TuKhoa_Cursor INTO @TuKhoa 
	WHILE @@FETCH_STATUS = 0 
	BEGIN 
	-- Nếu tìm thấy trong nội dung có từ khóa thì cộng thêm kết quả
	 SELECT @Count =Count(*) FROM @_Table 
		WHERE Noidung LIKE N'%'+@TuKhoa+'%'
	 IF @Count>0
		BEGIN
			SET @KetQua=@KetQua+@TuKhoa+', '
		END
	FETCH NEXT FROM TuKhoa_Cursor INTO @TuKhoa 
	END
	CLOSE TuKhoa_Cursor
	DEALLOCATE TuKhoa_Cursor

RETURN @KetQua
END

