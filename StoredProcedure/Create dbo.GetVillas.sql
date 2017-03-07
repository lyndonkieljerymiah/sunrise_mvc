USE [SunrisePortalWebDb]
GO

/****** Object: SqlProcedure [dbo].[GetVillas] Script Date: 2/23/2017 9:43:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].GetVillas
AS
BEGIN
	SELECT 
	v.Id,
	v.VillaNo,
	v.DateStamp,
	v.ElecNo,
	v.WaterNo,
	v.QtelNo,
	v.Status StatusCode,
	(SELECT s.Description FROM main.Selection s WHERE s.Code = v.Status) StatusDescription,
	v.Type,
	v.Capacity,
	v.Description,
	v.RatePerMonth,
	v.ProfileIndex,
	gal.Blob_Blob
	FROM vm.Villa v
	LEFT JOIN 
		(
		SELECT 
			gal.Id,
			gal.VillaId,
			gal.Blob_FileName,
			gal.Blob_MimeType,gal.Blob_Size, 
			gal.Blob_Blob 
		FROM vm.VillaGallery gal) gal ON (gal.Id = v.ProfileIndex)
END
