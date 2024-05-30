IF EXISTS (SELECT * FROM [AspNetUsers] WHERE [Email] = 'miguel.a.medina454@gmail.com')
BEGIN
    -- Perform actions if the user exists
    -- For example, you can delete the user or update related tables
    -- Here, we are deleting the user from the [AspNetUsers] table
    DELETE FROM [AspNetUsers] WHERE [Email] = 'miguel.a.medina454@gmail.com';
END;
IF EXISTS (SELECT * FROM [AspNetUsers] WHERE [Email] = 'dvlz.applez@gmail.com')
BEGIN
    -- Perform actions if the user exists
    -- For example, you can delete the user or update related tables
    -- Here, we are deleting the user from the [AspNetUsers] table
    DELETE FROM [AspNetUsers] WHERE [Email] = 'dvlz.applez@gmail.com';
END;