## Architectural Design

1. **Folder Naming Convention:**
   - Main project 
     - The main project name will be CamelCase, following the conventions we use in the previous semester. (BitBracket)
     - No symbols or spaces, using underscores (_) if spaces are absolutely neccesary. 
   - NUnit Test Project Folder 'BitBracket_NUnit_Tests'.
     - Tests will be lowercase.
     - NUnit tests will be named by what they are trying to test, such as 'added_registered_user_returns_registered_user'. Don't think we need to include '_Tests' appened at the end as it is already in the testing folder.
     - No symbols or spaces, using underscores (_) if spaces are absolutely neccesary for naming the specific tests.
     - We should place NUnit tests and mock tests seperate to keep testing sections organized and seperate, so that our main testing file isn't cluttered.
     - 'Mock_Tests' will be used for mock tests.
   - Jest JavaScript Testing Folder 'BitBracket_JavaScript_Tests'
     - No symbols or spaces, using underscores (_) if spaces are absolutely neccesary for naming the specific tests.
     - If we need to have seperate files to avoid cluttering, we will do so by naming the files what they are testing, such as mock tests if we use something like that for testing javascript.
   - BDD Testing Folder 'BitBracket_BDD_Tests'
     - Using Cucumber and Gherkin for BDD testing.
     - Since the point of BDD testing is to convey what is needed to be accomplished in simple english, we will write our tests in regular english.
     - Feature files will be named by what they are aiming to test, such as 'user_registration.feature'. 
     
2. **.NET Core Version**
   - We will be using .Net Core 8.0.101
   
3. **CSS**
   - We will be using normal CSS for our project, along with bootstrap 5.3.2
   - We are also considering using the library 'semantic UI' for our project, but we will decide on that later once we have a better idea of how exactly we are doing the bracket front end section of our project, as this library would be just to help with that. 
        - Semantic UI is a library that is used to help with the front end of a project, and is used to help with the theming of a project. It can also help with streamlining css and making it easier to use, but we will keep using css how we have been so far.
   - Theming is important for our project, but first we will focus on ensuring that our color choices are accessible to color blind peoples, as this is something that start.gg and challonge have been criticised for. 
   
4. **JavaScript**
   - We will be using just regular Javascript for our project. 
   - We will be using Jest for our testing.
   - We don't believe we will need to use JQuery.
   
5. **Git Branches and Naming Conventions**
   - We will be using the following conventions for our branches
        - (sprint number)-(feature abbreviation) 
            - 's1-ac' for sprint 1, account creation
            - We will double check with the team to make sure that we are not having conflicting branch names through discord.
            
6. **Database Design and Naming Conventions**
   - We will be using the naming conventions provided in the document given to us.
        - Example: 
            CREATE TABLE [Character] (
            [ID] int PRIMARY KEY IDENTITY(1, 1), 
            [Name] nvarchar(50) NOT NULL,
            [Created] datetime NOT NULL,
            [Level] int NOT NULL,
            [Health] int NOT NULL,
            [CharacterClassID] int NOT NULL,
			);
            ALTER TABLE [Character] ADD CONSTRAINT [Fk Character Class ID] FOREIGN KEY ([CharacterClassID]) REFERENCES [CharacterCl  ass]([ID]) ON DELETE NO ACTION ON UPDATE;
        - ID will always be capatilized, regardless of where it is in the name. 
        - Foregin Key declarations go in an alter table, but use a named CONSTRAINT
        - Must use semi colons, not GO
        
        - Populating Database:
            INSERT INTO [Character] (Created, Level, Health, CharacterClassID, Name) Values
            ('2021-12-03', 3, 39, 2, 'example name');
        - Must have semicolon at the end, not GO
        - We will avoid keywords used in SQL as names for tables and items, like USERS, ORDERS, etc. Comprehensive list [Here](https://dev.mysql.com/doc/refman/8.0/en/keywords.html#:~:text=Keywords%20are%20words%20that%20have,names%20of%20built%2Din%20functions)
        
7. **Eager vs Lazy Loading**
   - Due to knowing that we will have many relationships between users to tournaments, user to user, and potentially from organizer to tournament/user, we think using eager loading would be the best way to go about this.
