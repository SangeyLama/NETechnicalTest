

CREATE TABLE Stores (
	id int IDENTITY(1,1) PRIMARY KEY,
	name nvarchar(50) NOT NULL,
	district_id int 
	
	CONSTRAINT FK_DistrictStore FOREIGN KEY (district_id)
	REFERENCES Districts(id)
);