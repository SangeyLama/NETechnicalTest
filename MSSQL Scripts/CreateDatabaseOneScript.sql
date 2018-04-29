CREATE DATABASE NeasTest;
GO

Use NeasTest
CREATE TABLE Salespersons (
	id int IDENTITY(1,1) PRIMARY KEY,
	name nvarchar(50) NOT NULL,
);
GO

CREATE TABLE Districts (
	id int IDENTITY(1,1) PRIMARY KEY,
	name nvarchar(50) NOT NULL,
	primary_salesperson_id int NOT NULL,
);
GO

CREATE TABLE District_Salesperson_Junction(
	district_id int,
	salesperson_id int,
	CONSTRAINT PK_dist_salesp PRIMARY KEY (district_id, salesperson_id),
	CONSTRAINT FK_District
		FOREIGN KEY (district_id) REFERENCES Districts(id),
	CONSTRAINT FK_Salesperson
		FOREIGN KEY (salesperson_id) REFERENCES Salespersons(id)
);
GO

CREATE TABLE Stores (
	id int IDENTITY(1,1) PRIMARY KEY,
	name nvarchar(50) NOT NULL,
	district_id int 
	CONSTRAINT FK_DistrictStore FOREIGN KEY (district_id)
	REFERENCES Districts(id)
);
GO

ALTER TABLE Districts
ADD CONSTRAINT FK_PrimarySalesperson
FOREIGN KEY (primary_salesperson_id) REFERENCES Salespersons(id);
GO
