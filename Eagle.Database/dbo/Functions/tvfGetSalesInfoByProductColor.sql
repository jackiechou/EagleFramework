-- SQL create table-valued function like a parametrized view

CREATE FUNCTION [dbo].[tvfGetSalesInfoByProductColor]

              (@Color NVARCHAR(16))

RETURNS @retSalesInformation TABLE(-- Columns returned by the function

                                   ProductName NVARCHAR(50)    NULL,

                                   NonDiscountSales MONEY,

                                   DiscountSales    MONEY,

                                   Color            NVARCHAR(16)    NULL)

AS

  BEGIN

    INSERT @retSalesInformation

    SELECT   p.Name,

             SUM(OrderQty * UnitPrice),

             SUM((OrderQty * UnitPrice) * UnitPriceDiscount),

             @Color

    FROM     Production.Product p

             INNER JOIN Sales.SalesOrderDetail sod

               ON p.ProductID = sod.ProductID

    WHERE    Color = @Color

              OR @Color IS NULL

    GROUP BY p.Name

    

    RETURN;

  END;

 



