CREATE TABLE Districts (
	id int IDENTITY(1,1) PRIMARY KEY,
	name nvarchar(50) NOT NULL,
	primary_salesperson_id int NOT NULL,

);