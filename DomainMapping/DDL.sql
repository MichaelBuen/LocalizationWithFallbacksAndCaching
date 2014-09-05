create table Person
(
	PersonId int identity(1,1) primary key,
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null
);