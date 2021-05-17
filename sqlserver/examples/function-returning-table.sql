drop function if exists FilteringByAddressType

GO
CREATE FUNCTION FilteringByAddressType(@typeId smallint)
RETURNS @location TABLE(
	City nvarchar(30) not null,
	TypeName nvarchar(30) not null
)
AS
BEGIN
	IF @typeId IS NOT NULL
		BEGIN
			IF EXISTS(select 1 from Person.AddressType where AddressTypeID = @typeId)
				INSERT INTO @location
					select adr.City, adrType.[Name] from Person.BusinessEntityAddress as bea
					INNER JOIN Person.Address as adr ON bea.AddressID = adr.AddressID
					INNER JOIN Person.AddressType as adrType ON bea.AddressTypeID = adrType.AddressTypeID
					where bea.AddressTypeID = @typeId
		END

		RETURN;
END;

GO

select * from FilteringByAddressType(2);