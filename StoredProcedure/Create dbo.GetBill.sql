USE [SunrisePortalWebDb]
GO

/****** Object: SqlProcedure [dbo].[GetBill] Script Date: 2/23/2017 9:41:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].GetBill
	@BillId nvarchar(150)
AS
BEGIN
	SELECT
		eo.*
	FROM (
		SELECT 
			b.Id,
			b.Code BillCode,
			b.DateStamp,
			b.ContractId,
			c.Code ContractCode,
			(SELECT s.Description  FROM main.Selection s WHERE s.Code = c.Code) ContractStatusDescription,
			c.Amount_Amount ContractAmount,
			b.Status_Code StatusCode,
			(SELECT s.Description  FROM main.Selection s WHERE s.Code = b.Status_Code) StatusDescription
		FROM stm.Bill b		
		INNER JOIN stm.Contract c ON (c.Id = b.Id)
		INNER JOIN (
			SELECT 
				v.Id,
				v.VillaNo,
				v.ElecNo
			FROM vm.Villa v) villa 
			ON (villa.Id = c.VillaId)
		INNER JOIN (
			SELECT 
				ten.Id,
				ten.Name,
				CONCAT(ten.Address_Address1,' ',ten.Address_Address2,' ',ten.Address_City,' ',ten.Address_PostalCode) Address,
				ind.Birthday,
				ind.QatarId,
				ind.Company,
				com.CrNo,
				com.ValidityDate,
				com.BusinessType,
				com.Representative
			FROM tm.Tenant ten 
			LEFT JOIN tm.Individual ind ON(ind.TenantId = ten.Id)
			LEFT JOIN tm.Company com ON (com.TenantId = ten.Id)) cus 
		ON (cus.Id = c.TenantId)) eo
	WHERE eo.Id = @BillId
END
