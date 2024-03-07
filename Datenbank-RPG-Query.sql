USE master;
DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('game')
EXEC(@kill);
GO

if (db_id('game') is not null)
	drop database game

go

create database game

go

use game;

go

create table class (
	id int identity(1, 1) primary key,
	name varchar(64),
	attackDelay int default 100,
	attack int default 20,
);

create table itemType (
	id int identity(1, 1) primary key,
	name varchar(64),
);

create table item (
	id int identity(1, 1) primary key,
	name varchar(64),
	itemType int foreign key references itemType(id),
	itemPower int default 200,
	gold int
);

create table player (
	id int identity(1, 1) primary key,
	name varchar(64),
	life int default 100,
	maxlife int default 100,
	defense int default 20,
	gold int default 1,
	classId int foreign key references class(id)
);

create table enemy (
	id int identity(1, 1) primary key,
	name varchar(64),
	life int default 100,
	defense int default 20,
	attackDelay int default 10,
	attack int default 20
);

create table player_graveyard (
	playerId int foreign key references class(id),
	enemyId int foreign key references enemy(id),
);

create table enemy_graveyard (
	playerId int foreign key references class(id),
	enemyId int foreign key references enemy(id), 
);


create table inventory (
	itemId int foreign key references item(id),
	amount int
);

insert into  itemType(name) values
('Health Potion'),
('Revive Item');

insert into  item(name, itemType, itemPower, gold) values
('Minor Health Potion', 1, 15, 25),
('Health Potion', 1, 60, 125),
('Major Health Potion', 1, 120, 400),
('Revive Halo', 2, 120, 75);

insert into  class(name, attackDelay, attack) values
('Warrior', 2000, 125),
('Mage', 450, 40),
('Thief', 100, 2),
('Demon Hunter', 250, 30);

insert into player(name, life, defense, classId, gold) values
('KENT KARSON', 100, 125, 1, 125);

insert into enemy(name, life, defense, attackDelay, attack) values
('Goblin', 20, 5, 500, 5),
('Iron Goblin', 40, 15, 1500, 45);


go 

create procedure add_player @name varchar(64), @life int, @defense int, @classId int
as
	insert into player(name, life, maxlife, defense, classId) values
	(@name, @life, @life, @defense, @classId);
go

create procedure list_all_items
as
	select item.id, item.name, itemType, iT.name as itemTypeName, itemPower, gold from item
	INNER JOIN itemType as iT on itemType = iT.id
go

create procedure buyItem @playerID int, @itemId int, @gold int
as
	update player
	set gold = @gold
	where id = @playerID;
go

create procedure addItem @itemId int
as
	IF (select itemId from inventory where itemId = @itemId) IS NOT NULL
	BEGIN
		update inventory
		set amount = amount + 1
		where itemId = @itemId;
	END
	ELSE
	BEGIN
		insert into inventory values
		(@itemId, 1)
	END
go

create procedure displayInventory
as
	select it.id, it.name, itName.name as itemname, it.itemPower, amount, it.gold from inventory
	INNER JOIN item as it on inventory.itemId = it.id
	INNER JOIN itemType as itName on it.itemType = itName.id
go
