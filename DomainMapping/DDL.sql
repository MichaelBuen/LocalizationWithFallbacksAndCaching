/*
drop table "Order"; 
drop table Person;
*/

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
	OrderDate datetime not null,
	Comment nvarchar(4000) not null
);


insert into "Order"(MadeByPersonId, OrderDate, Comment) values(1, '2000-1-1', 'You');
insert into "Order"(MadeByPersonId, OrderDate, Comment) values(2, '2000-1-2', 'Say');
insert into "Order"(MadeByPersonId, OrderDate, Comment) values(3, '2000-1-3', 'Yes');



