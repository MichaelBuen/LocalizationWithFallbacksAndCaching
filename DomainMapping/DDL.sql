create table Person
(
	PersonId int identity(1,1) primary key,
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null
);


insert into Person(FirstName, LastName) values('John', 'Lennon');
insert into Person(FirstName, LastName) values('Paul', 'McCartney');
insert into Person(FirstName, LastName) values('ZZZ','Sleepy')


create table "Order"
(
	OrderId int identity(1,1) not null,
	MadeByPersonId int not null references Person(PersonId),
	OrderDate datetime not null
);


insert into "Order"(MadeByPersonId, OrderDate) values(1, '2000-1-1');
insert into "Order"(MadeByPersonId, OrderDate) values(2, '2000-1-2');
insert into "Order"(MadeByPersonId, OrderDate) values(3, '2000-1-3');



