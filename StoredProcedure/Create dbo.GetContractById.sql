USE [SunrisePortalWebDb]
GO

/****** Object: SqlProcedure [dbo].[GetContractById] Script Date: 2/23/2017 9:42:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].GetContractById
	@ContractId nvarchar(150)
AS
BEGIN
	SELECT 
		c.Id,
		c.Code,
		c.DateCreated,
		c.ContractType_Code ContractTypeCode,
		(SELECT s.Description FROM main.Selection s WHERE s.Code = c.ContractType_Code) ContractTypeDescription,
		c.Period_Start,
		c.Period_End,
		c.Amount_Amount Amount,
		c.Status_Code ContractStatus,
		(SELECT s.Description FROM main.Selection s WHERE s.Code = c.Status_Code) ContractStatusDescription,
		c.VillaId,
		c.UserId,
		c.IsReversed,
		c.IsTerminated,
		v.VillaNo,
		v.ElecNo,
		v.WaterNo,
		v.QtelNo,
		v.RatePerMonth,
		v.Type RentalType,
		(SELECT s.Description FROM main.Selection s WHERE s.Code = v.Type) RentalTypeDescription,
		v.Status VillaStatus,
		(SELECT s.Description FROM main.Selection s WHERE s.Code = v.Status) VillaStatusDescription,
		cus.Code TenantCode,
		cus.Name,
		cus.Address,
		cus.TelNo,
		cus.MobileNo,
		cus.EmailAddress
	FROM stm.Contract c
	INNER JOIN vm.Villa v ON v.Id = c.VillaId
	INNER JOIN (SELECT 
				ten.Id,
				ten.Code,
				ten.Name,
				CONCAT(ten.Address_Address1,' ',ten.Address_Address2,' ',ten.Address_City,' ',ten.Address_PostalCode) Address,
				ten.TelNo,
				ten.MobileNo,
				ten.EmailAddress,
				ind.Birthday,
				ind.QatarId,
				ind.Company,
				com.CrNo,
				com.ValidityDate,
				com.BusinessType,
				com.Representative
			FROM tm.Tenant ten 
			LEFT JOIN tm.Individual ind ON(ind.TenantId = ten.Id)
			LEFT JOIN tm.Company com ON (com.TenantId = ten.Id)) cus ON cus.Id = c.TenantId
	WHERE c.Id = @ContractId
END
