USE [SunrisePortalWebDb]
GO

/****** Object: SqlProcedure [dbo].[GetBillReconcile] Script Date: 2/23/2017 9:42:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetBillReconcile]
	@BillId nvarchar(300)
AS

BEGIN	
	SELECT 
		rec.Id,
		rec.Date,
		rec.BillId,
		rec.PaymentType_Code PaymentTypeCode,
		(SELECT 
		s.Description
		FROM main.Selection s
		WHERE s.Code = rec.PaymentType_Code) PaymentTypeDescription,
		rec.Bank_Code BankCode,
		(SELECT 
		s.Description
		FROM main.Selection s
		WHERE s.Code = rec.Bank_Code) BankDescription,
		rec.ChequeNo,
		rec.ReferenceNo,
		rec.DishonouredAmount,
		rec.Amount,
		rec.Period_Start PeriodStart,
		rec.Period_End PeriodEnd,
		rec.Remarks
	FROM stm.Reconcile rec
	WHERE rec.BillId = @BillId
END
