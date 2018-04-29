CREATE TABLE District_Salesperson_Junction(
	district_id int,
	salesperson_id int,
	CONSTRAINT PK_dist_salesp PRIMARY KEY (district_id, salesperson_id),
	CONSTRAINT FK_District
		FOREIGN KEY (district_id) REFERENCES Districts(id),
	CONSTRAINT FK_Salesperson
		FOREIGN KEY (salesperson_id) REFERENCES Salespersons(id)
)