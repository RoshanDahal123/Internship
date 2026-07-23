create database library;

use library;
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Department VARCHAR(100),
    Salary DECIMAL(10,2)
);

INSERT INTO Employees (Name, Department, Salary)
VALUES('John Doe','IT', 55000.00);




Insert into Employees (Name, Department,Salary)
Values
 ('Jane Smith', 'HR', 60000.00),
    ('Mike Ross', 'Finance', 58000.00);

select * from Employees where Department='IT';

select * from Employees Order by Salary Desc;

select top 5 * from employees order by salary desc;

Update Employees 
set Salary = 65000.00
Where Id =1 ;

Update Employees 
set Name='Roshan Dahal', Department='Engineerring', Salary= 70000
where Id = 1;

select * from Employees where Id=1

Delete from Employees where Id =1;

select * from Employees;

Drop table Employees;
Drop  Database library;

