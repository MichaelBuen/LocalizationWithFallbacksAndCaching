create table Person
(
	PersonId int identity(1,1) primary key,
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null
);


insert into Person(FirstName, LastName) values('John', 'Lennon');
insert into Person(FirstName, LastName) values('Paul', 'McCartney');