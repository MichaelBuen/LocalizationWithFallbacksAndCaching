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






create table LanguageCulture
(
	-- https://dotnetfiddle.net/17Bkny
	LanguageCultureCode varchar(16) not null, -- longest is ca-ES-valencia	
	NativeName nvarchar(50), -- CultureInfo.GetCultures(CultureTypes.AllCultures).OrderByDescending(x => x.NativeName.Length).First().NativeName.Length.Dump();
	EnglishName varchar(50), -- CultureInfo.GetCultures(CultureTypes.AllCultures).OrderByDescending(x => x.EnglishName.Length).First().EnglishName.Length.Dump();

	NeutralLanguageCode varchar(16) null,

	constraint pk_LanguageCulture primary key (LanguageCultureCode),
	constraint fk_LanguageCulture_NeutralLanguageCode foreign key(NeutralLanguageCode) references LanguageCulture(LanguageCultureCode)
);




insert into LanguageCulture(LanguageCultureCode, NativeName, EnglishName, NeutralLanguageCode) values
('zh-Hans', N'中文(简体)', 'Chinese (Simplified)', null),
('zh-Hant', N'中文(繁體)', 'Chinese (Traditional)', null),

('zh-CN', N'中文(中华人民共和国)', 'Chinese (Simplified, China)', 'zh-Hans'),
('zh-SG', N'中文(新加坡)', 'Chinese (Simplified, Singapore)', 'zh-Hans'),
('zh-HK', N'中文(香港特別行政區)', 'Chinese (Traditional, Hong Kong SAR)', 'zh-Hant'),
('zh-TW', N'中文(台灣)', 'Chinese (Traditional, Taiwan)', 'zh-Hant'),

('en', N'English', 'English', null),

('en-PH', N'English (Philippines)', 'English (Philippines)', 'en'),
('en-US', N'English (United States)', 'English (United States)', 'en'),
('en-GB', N'English (United Kingdom) ', ' English (United Kingdom)', 'en');


