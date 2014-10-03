/*

drop table dbo.ThingTranslation;
drop table dbo.Thing;

drop table dbo."Order"; 
drop table dbo.Person;

drop table dbo.LanguageCulture;

*/

create table dbo.Person
(
	PersonId int identity(1,1) primary key,
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null
);


insert into Person(FirstName, LastName) values('John', 'Lennon');
insert into Person(FirstName, LastName) values('Paul', 'McCartney');
insert into Person(FirstName, LastName) values('ZZZ','Sleepy')


create table dbo."Order"
(
	OrderId int identity(1,1) not null,
	MadeByPersonId int not null references dbo.Person(PersonId),
	OrderDate datetime not null,
	Comment nvarchar(4000) not null
);


insert into dbo."Order"(MadeByPersonId, OrderDate, Comment) values(1, '2000-1-1', 'You');
insert into dbo."Order"(MadeByPersonId, OrderDate, Comment) values(2, '2000-1-2', 'Say');
insert into dbo."Order"(MadeByPersonId, OrderDate, Comment) values(3, '2000-1-3', 'Yes');






create table dbo.LanguageCulture
(
	-- https://dotnetfiddle.net/17Bkny
	LanguageCultureCode varchar(16) not null, -- longest is ca-ES-valencia	
	NativeName nvarchar(50), -- CultureInfo.GetCultures(CultureTypes.AllCultures).OrderByDescending(x => x.NativeName.Length).First().NativeName.Length.Dump();
	EnglishName varchar(50), -- CultureInfo.GetCultures(CultureTypes.AllCultures).OrderByDescending(x => x.EnglishName.Length).First().EnglishName.Length.Dump();

	NeutralLanguageCode varchar(16) null,

	constraint pk_LanguageCulture primary key (LanguageCultureCode),
	constraint fk_LanguageCulture_NeutralLanguageCode foreign key(NeutralLanguageCode) references dbo.LanguageCulture(LanguageCultureCode)
);




insert into dbo.LanguageCulture(LanguageCultureCode, NativeName, EnglishName, NeutralLanguageCode) values
('zh-Hans', N'中文(简体)', 'Chinese (Simplified)', null),
('zh-Hant', N'中文(繁體)', 'Chinese (Traditional)', null),

('zh-CN', N'中文(中华人民共和国)', 'Chinese (Simplified, China)', 'zh-Hans'),
('zh-SG', N'中文(新加坡)', 'Chinese (Simplified, Singapore)', 'zh-Hans'),
('zh-HK', N'中文(香港特別行政區)', 'Chinese (Traditional, Hong Kong SAR)', 'zh-Hant'),
('zh-TW', N'中文(台灣)', 'Chinese (Traditional, Taiwan)', 'zh-Hant'),

('en', N'English', 'English', null),

('en-PH', N'English (Philippines)', 'English (Philippines)', 'en'),
('en-US', N'English (United States)', 'English (United States)', 'en'),
('en-GB', N'English (United Kingdom) ', 'English (United Kingdom)', 'en');





create table dbo.Thing
(
	ThingId int identity (1,1) not null,

	YearInvented int not null,

	constraint pk_Thing primary key (ThingId)
);



create table dbo.ThingTranslation
(
	ThingTranslationId int not null identity(1,1),

	ThingId int not null,
	LanguageCultureCode varchar(16) not null,

	ThingName nvarchar(100) not null,
	ThingDescription nvarchar(100) not null,

	constraint pk_ThingTranslation primary key(ThingTranslationId),

	constraint uk_ThingTranslation unique(ThingId, LanguageCultureCode),

	constraint fk_ThingTranslation_Thing foreign key(ThingId) references dbo.Thing(ThingId),
	constraint fk_ThingTranslation_LanguageCultureCode foreign key(LanguageCultureCode) references dbo.LanguageCulture(LanguageCultureCode)

);


go




insert into dbo.Thing(YearInvented) values
(1980), -- elevator
(1990), -- guitar
(1800), -- erhu
(1900); -- qiu



insert into dbo.ThingTranslation(ThingId, LanguageCultureCode, ThingName, ThingDescription) values
(1, 'en-US', 'Elevator - Specific US', ''), -- specific
(1, 'en-GB', 'Lift - Specific GB', ''), -- specific
(1, 'en', 'Liftor - Neutral', ''), -- neutrals

(2, 'en', 'Guitar - Neutral', ''), -- neutral

(2, 'en-PH', 'Gitara - Specific', ''), -- neutral

(3, 'zh-CN', 'Erhu - Specific China', ''), -- specific
(3, 'zh-Hans', 'Erhu - Hans Neutral', ''), -- neutral
(3, 'zh-Hant', 'Erhu - Hant Neutral', ''), -- neutral
(3, 'zh-TW', 'Erhu - Specific Taiwan', ''), -- neutral

(4, 'zh-Hant', 'Qiu - Hant Neutral', ''); -- neutral


go




if exists(select 1 from information_schema.routines where specific_schema = 'dbo' and specific_name = 'GetThingTranslation' and routine_type = 'FUNCTION')
	drop function dbo.GetThingTranslation;
GO


create function dbo.GetThingTranslation(@LanguageCultureCode varchar(16))
returns table
as
	
return
    with a as
	(
		select 
			TheRank = 
			rank() over (partition by tl.ThingId
				order by
				case tl.LanguageCultureCode
				when @LanguageCultureCode then 1
				when lc.NeutralLanguageCode then 2
				when 'en' then 3
				else 4
				end,
				tl.ThingTranslationId -- if the order by LanguageCultureCode don't have any match (4), just pick whatever comes first 
				)	
			, tl.ThingId	
			, tl.ThingName
			, tl.ThingDescription
			, ActualLanguageCultureCode = tl.LanguageCultureCode 
			
		from dbo.ThingTranslation tl
		left join dbo.LanguageCulture lc on tl.LanguageCultureCode = lc.LanguageCultureCode
	)
	select
		-- composite key for ORM..
		a.ThingId, LanguageCultureCode = @LanguageCultureCode
		-- ..composite key
		
		, a.ThingName
		, a.ThingDescription
		
		, a.ActualLanguageCultureCode

		, a.TheRank
		
	from a
	where a.TheRank = 1;


go



select * from dbo.GetThingTranslation('en-US');
select * from dbo.GetThingTranslation('en-GB');
select * from dbo.GetThingTranslation('en-PH');



select * from dbo.GetThingTranslation('zh-CN');
select * from dbo.GetThingTranslation('zh-SG');

select * from dbo.GetThingTranslation('zh-TW');
select * from dbo.GetThingTranslation('zh-HK');




go