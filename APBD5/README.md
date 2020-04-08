### Procedure:

Add a new stored procedure to your database. The procedure should expect parameters
for Studies and Semester. Our endpoint will execute the procedure to update all
Students assigned to a given Semester and Studies. Firstly, we must find a proper
record in Enrollment table (given Studies and Semester increased by 1). If such record
doesn’t exists we must add a new one. To complete the process update IdEnrollment
value for all promoted students.

```
CREATE PROCEDURE promoteStudentsByStudyAndSemester @Studies nvarchar(30), @Semester int
AS
    --- DECLARE @Studies NVARCHAR(30) = 'Computer Science'; ---
    --- DECLARE @Semester INT = 4; -- 
    
    DECLARE @oldIdEnrollment INT = null;
    DECLARE @newIdEnrollment INT = null;
    DECLARE @idStudy INT = null;
    
    SELECT @idStudy = S.IdStudy FROM Studies S WHERE Name = @Studies;
    
    SELECT @oldIdEnrollment = E.IdEnrollment
    FROM Enrollment E
    WHERE Semester = @Semester AND IdStudy = @idStudy;
    
    SELECT @newIdEnrollment = E.IdEnrollment 
    FROM Enrollment E
    WHERE Semester = 1+@Semester AND IdStudy = @idStudy;
    
    IF @newIdEnrollment IS NULL
    BEGIN
        SELECT @newIdEnrollment = MAX(IdEnrollment)+1 FROM Enrollment;
    
        INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate)
        VALUES (@newIdEnrollment, 1+@Semester, @idStudy, GETDATE())
    END

    UPDATE Student SET IdEnrollment = @newIdEnrollment WHERE IdEnrollment = @oldIdEnrollment;
GO;
```