CREATE PROCEDURE sp_RecordFailedLogin
(
    @p_UserId INT
)
AS

    UPDATE Users

    SET
        FailedLoginAttempts =
            FailedLoginAttempts + 1,

        LockoutUntil =
            CASE
                WHEN FailedLoginAttempts + 1 >= 5
                THEN DATE_ADD(NOW(), DATEADD(minute, 15, GETDATE()))
                ELSE LockoutUntil
            END

    WHERE UserId = @p_UserId
