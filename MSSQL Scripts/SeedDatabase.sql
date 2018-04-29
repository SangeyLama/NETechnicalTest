Use NeasTest

INSERT INTO Salespersons(name)
Values ('Jens Jensen'), ('John Smith'), ('Alice Johnson'), ('Mary Smith'), ('Lisa Stanley'), ('Michael Fox'), ('Sam Rockwell'), ('Samuel Jackson'), ('Jack Nicholson')

GO

INSERT INTO Districts(name,primary_salesperson_id)
Values ('North Denmark', 1), ('South Denmark', 2), ('West Denmark', 3), ('East Denmark', 4), ('Central Denmark', 5)

GO
INSERT INTO District_Salesperson_Junction(district_id, salesperson_id)
Values (1,2), (1,3), (2,5), (2,6), (3,4), (4,7), (5,8), (5,9)

INSERT INTO Stores(name, district_id)
Values ('Rema 1000', 1), ('Netto', 1), ('Fakta', 1), ('Ikea', 2), ('Bilka', 2), ('Jysk', 2), ('Salling', 3), ('Fotex', 3), ('McDonalds', 4),('Burger King', 4),
('Jensen Bofhus', 4), ('Sunset', 5), ('Bike Shop', 5), ('Aldi', 5), ('Spar', 5)