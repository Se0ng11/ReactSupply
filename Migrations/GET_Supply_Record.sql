USE [SupplyChain]
GO

/****** Object:  StoredProcedure [dbo].[GET_Supply_Record]    Script Date: 28/05/2018 14:33:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

      
CREATE PROCEDURE [dbo].[GET_Supply_Record]       
      
AS      
BEGIN      
 DECLARE @cols AS NVARCHAR(MAX),      
    @query  AS NVARCHAR(MAX)      
      
 SELECT @cols = STUFF((SELECT ',' + ValueName      
      FROM ConfigurationMain      
   WHERE IsVisible = 1  
      ORDER BY Position      
    FOR XML PATH(''), TYPE      
    ).value('.', 'NVARCHAR(MAX)')       
   ,1,1,'')      
      
 SET @query = N'SELECT(SELECT ' + @cols + N' from       
     (      
     select AxNumber, valuename, data      
      from supplyrecord WHERE [Status]=''Open''      
    ) x      
    pivot       
    (      
     max(data)      
     for valuename in (' + @cols + N')      
    ) p       
    Order By AX6SO      
    FOR JSON AUTO) AS JsonResult'      
 print @query    
 EXEC sp_executesql @query;      
END 
GO

