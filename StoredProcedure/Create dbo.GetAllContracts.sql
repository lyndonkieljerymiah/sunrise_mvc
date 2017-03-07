USE [SunrisePortalWebDb]
GO

/****** Object: SqlProcedure [dbo].[GetAllContracts] Script Date: 2/23/2017 9:40:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllContracts]
AS
BEGIN
	SELECT 
		c.Id,
		c.Code,
		c.DateCreated,
		c.Period_Start PeriodStart,
		c.Period_End PeriodEnd,
		ten.Name,
		c.Status_Code StatusCode,
		(SELECT s.Description FROM main.Selection s WHERE s.Code = c.Status_Code) StatusDescription
	FROM stm.Contract c
	INNER JOIN vm.Villa v ON v.Id = c.VillaId
	INNER JOIN (SELECT ten.Id,ten.Name FROM tm.Tenant ten) ten ON ten.Id = c.TenantId
END
