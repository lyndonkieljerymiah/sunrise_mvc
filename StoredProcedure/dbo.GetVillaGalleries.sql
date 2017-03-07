

CREATE PROCEDURE [dbo].GetVillaGalleries
	@VillaId nvarchar(150)
AS
BEGIN
	SELECT 
		vg.Id,
		vg.VillaId,
		vg.Blob_Blob Blob,
		vg.Blob_FileFormat FileFormat,
		vg.Blob_FileName [FileName],
		vg.Blob_MimeType MimeType,
		vg.Blob_Size Size 
	FROM vm.VillaGallery vg
	WHERE vg.VillaId = @VillaId
END