-- Temporary variables
DECLARE @MaxGenerations INT = 100;
DECLARE @InitialYear INT = 1340;
DECLARE @YearsBetweenGenerations INT = 25;

-- List of sample names (more can be added for larger datasets)
DECLARE @MaleNames TABLE (Name NVARCHAR(50));
DECLARE @FemaleNames TABLE (Name NVARCHAR(50));
DECLARE @Surnames TABLE (Surname NVARCHAR(50));

INSERT INTO @MaleNames (Name) VALUES ('John'), ('William'), ('James'), ('Robert'), ('Charles'), ('George'), ('Joseph');
INSERT INTO @FemaleNames (Name) VALUES ('Mary'), ('Elizabeth'), ('Sarah'), ('Margaret'), ('Anna'), ('Emma'), ('Martha');
INSERT INTO @Surnames (Surname) VALUES ('Smith'), ('Johnson'), ('Williams'), ('Brown'), ('Jones'), ('Miller'), ('Davis');

-- Disable foreign key constraints for insertion
ALTER TABLE [dbo].[People] NOCHECK CONSTRAINT ALL;

-- Clear existing data (optional)
TRUNCATE TABLE [dbo].[People];

-- Initialize base parents
DECLARE @InitialFather NVARCHAR(50) = 'Adam';
DECLARE @InitialMother NVARCHAR(50) = 'Eve';
DECLARE @InitialSurname NVARCHAR(50) = 'Ancestor';

INSERT INTO [dbo].[People] (Name, SurName, BirthDate, IdentityNumber, FatherId, MotherId)
VALUES (@InitialFather, @InitialSurname, DATEFROMPARTS(@InitialYear, 1, 1), 'A00001', NULL, NULL),
       (@InitialMother, @InitialSurname, DATEFROMPARTS(@InitialYear + 1, 1, 1), 'B00001', NULL, NULL);

-- Retrieve IDs of base parents
DECLARE @FatherId INT = SCOPE_IDENTITY() - 1; -- Adam
DECLARE @MotherId INT = SCOPE_IDENTITY();     -- Eve

-- Loop through generations
DECLARE @Generation INT = 1;
WHILE @Generation <= @MaxGenerations
BEGIN
    -- Create a realistic name combination for each generation
    DECLARE @CurrentFatherName NVARCHAR(50);
    DECLARE @CurrentMotherName NVARCHAR(50);
    DECLARE @CurrentSurname NVARCHAR(50);

    SELECT TOP 1 @CurrentFatherName = Name FROM @MaleNames ORDER BY NEWID();
    SELECT TOP 1 @CurrentMotherName = Name FROM @FemaleNames ORDER BY NEWID();
    SELECT TOP 1 @CurrentSurname = Surname FROM @Surnames ORDER BY NEWID();

    -- Create new children with unique identity numbers
    INSERT INTO [dbo].[People] (Name, SurName, BirthDate, IdentityNumber, FatherId, MotherId)
    VALUES
        (@CurrentFatherName, @CurrentSurname, DATEFROMPARTS(@InitialYear + @Generation * @YearsBetweenGenerations, 1, 1), CONCAT('A', FORMAT(@Generation, 'D5')), @FatherId, @MotherId),
        (@CurrentMotherName, @CurrentSurname, DATEFROMPARTS(@InitialYear + @Generation * @YearsBetweenGenerations + 1, 1, 1), CONCAT('B', FORMAT(@Generation, 'D5')), NULL, NULL);

    -- Update parent IDs for the next generation
    SET @FatherId = SCOPE_IDENTITY() - 1;
    SET @MotherId = SCOPE_IDENTITY();

    -- Increment the generation counter
    SET @Generation = @Generation + 1;
END;

-- Re-enable foreign key constraints
ALTER TABLE [dbo].[People] WITH CHECK CHECK CONSTRAINT ALL;
