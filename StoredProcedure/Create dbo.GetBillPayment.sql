USE [SunrisePortalWebDb]
GO

/****** Object: SqlProcedure [dbo].[GetBillPayment] Script Date: 2/23/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetBillPayment]
	@BillId nvarchar(150)
AS
BEGIN
	SELECT
		p.Id,
		p.BillId,
		p.PaymentDate,
		p.PaymentType_Code PaymentTypeCode,
		(SELECT 
			s.Description 
		FROM main.Selection s 
		WHERE s.Code = p.PaymentType_Code) PaymentTypeDescription,

		p.PaymentMode_Code PaymentModeCode,
		(SELECT 
			s.Description 
		FROM main.Selection s 
		WHERE s.Code = p.PaymentMode_Code) PaymentTypeDescription,
		p.ChequeNo,
		p.Bank_Code BankCode,
		(SELECT 
			s.Description 
		FROM main.Selection s 
		WHERE s.Code = p.Bank_Code) BankDescription,
		p.Period_Start PeriodStart,
		p.Period_End PeriodEnd,
		p.Amount,
		p.Status_Code StatusCode,
		(SELECT 
			s.Description 
		FROM main.Selection s 
		WHERE s.Code = p.Status_Code) StatusDescription,
		p.StatusDate
	FROM stm.Payment p
	WHERE p.BillId = @BillId
END
