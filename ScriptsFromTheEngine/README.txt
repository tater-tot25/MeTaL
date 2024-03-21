CREATE TABLE Pet(
	name VARCHAR(40),
	species ENUM('Dog','Cat','Mouse','Other'),
	color SET('Black', 'White', 'Brown', 'Orange', 'Other'),
);

INSERT INTO Pet (name, species, color)
VALUES('Milo', 'Dog', 'Black, White, Brown');

INSERT INTO Pet (name, species, color)
VALUES('Otis', 'Cat', 'orange, white');







CREATE TABLE Employee(
	EmployeeInfo JSON
);

INSERT INTO Employee
VALUES (JSON_OBJECT('name', 'John', 'salary', 1, 'department', 'sales', 'active', True));

SELECT JSON_EXTRACT (EmpoyeeInfo, '$.name') AS names
FROM Employee;