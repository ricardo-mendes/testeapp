/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


:r .\Inserts\User.sql
:r .\Inserts\Client.sql 
:r .\Inserts\PetLover.sql 
:r .\Inserts\Pet.sql
:r .\Inserts\Employee.sql
:r .\Inserts\ScheduleItem.sql
:r .\Inserts\ScheduleItemClient.sql
:r .\Inserts\ScheduleItemEmployee.sql