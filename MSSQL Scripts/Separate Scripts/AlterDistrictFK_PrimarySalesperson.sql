ALTER TABLE Districts
ADD CONSTRAINT FK_PrimarySalesperson
FOREIGN KEY (primary_salesperson_id) REFERENCES Salespersons(id);