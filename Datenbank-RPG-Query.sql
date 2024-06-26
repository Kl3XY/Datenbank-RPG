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
	id int identity(0, 1) primary key,
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
	gold int default 0,
	classId int
);

alter table player add constraint classRestraint foreign key (classId) references class(id);

CREATE INDEX playerName
ON player(name); 

CREATE INDEX playerID
ON player(id); 

create table enemyType (
	id int identity(1, 1) primary key,
	name varchar(64),
	attackDelay int default 10,
	attack int default 20
);

create table enemy (
	id int identity(0, 1) primary key,
	name varchar(64),
	life int default 100,
	maxLife int default 100,
	defense int default 20,
	enemyTypeId int foreign key references enemyType(id)
);

create table battle (
	playerid int foreign key references player(id),
	enemyid int foreign key references enemy(id)
);

create table player_graveyard (
	playerId int foreign key references player(id),
	enemyId int foreign key references enemy(id),
);

create table enemy_graveyard (
	playerId int foreign key references player(id),
	enemyId int foreign key references enemy(id), 
);


create table inventory (
	itemId int ,
	amount int
);

alter table inventory add constraint itemRestraint foreign key (itemId) references item(id);

insert into  itemType(name) values
('Health Potion'),
('Revive Item');

insert into  item(name, itemType, itemPower, gold) values
('Minor Health Potion', 1, 15, 25),
('Health Potion', 1, 60, 125),
('Major Health Potion', 1, 120, 400),
('Revive Halo', 2, 120, 75);

insert into  class(name, attackDelay, attack) values
('Undefined', 0, 0),
('Warrior', 200, 125),
('Mage', 450, 300),
('Thief', 100, 50),
('Demon Hunter', 250, 80);

insert into player(name, life, maxlife, defense, classId, gold) values
('KENT KARSON', 50, 50, 20, 1, 20);

insert into enemyType(name, attackDelay, attack) values
('Small Foe', 500, 60);

insert into enemy(name, life, maxLife, defense, enemyTypeId) values
('Goblin', 20, 20, 5, 1);

go 

create procedure add_player @name varchar(64), @life int, @defense int, @classId int
as
	insert into player(name, life, maxlife, defense, classId) values
	(@name, @life, @life, @defense, @classId);
go

create procedure list_all_items
as
	select item.id, item.name, itemType, iT.name as itemname, itemPower, gold, 1 as amount from item
	INNER JOIN itemType as iT on itemType = iT.id
go

create procedure list_item @id int
as
	select item.id, item.name, itemType, iT.name as itemname, itemPower, gold, 1 as amount from item
	INNER JOIN itemType as iT on itemType = iT.id
	where @id = item.id
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

create procedure displayPlayers @id int
as
	IF @id != -1 
	BEGIN
		select player.id, player.name, life, maxlife, defense, gold, classId, cl.attack, cl.attackDelay, cl.name as className from player
		INNER JOIN class as cl ON player.classId = cl.id 
		where player.id = @id
	END
	ELSE
	BEGIN
		select player.id, player.name, life, maxlife, defense, gold, classId, cl.attack, cl.attackDelay, cl.name as className from player
		INNER JOIN class as cl ON player.classId = cl.id;
	END
go

create procedure displayEnemies @id int
as
	IF @id != -1 
	BEGIN
		select enemy.id, enemy.name, enemy.life, enemy.maxLife, enemy.defense, e.name as type, e.attack, e.attackDelay from enemy 
		inner join enemyType as e on e.id = enemy.enemyTypeId
		where enemy.id = @id
	END
	ELSE
	BEGIN
		select enemy.id, enemy.name, enemy.life, enemy.maxLife, enemy.defense, e.name as type, e.attack, e.attackDelay from enemy 
		inner join enemyType as e on e.id = enemy.enemyTypeId
	END
go

create procedure setPlayerHealth @playerid int, @health int
as
	update player
	set life = @health
	where id = @playerid;
go

create procedure setEnemyHealth @enemyid int, @health int
as
	update enemy
	set life = @health
	where id = @enemyid;
go

create procedure playerDead @playerId int, @enemyid int
as
	insert into player_graveyard values
	(@playerId, @enemyid);
go

create procedure enemyDead @playerId int, @enemyid int
as
	insert into enemy_graveyard values
	(@playerId, @enemyid);
go

create procedure useItem @itemId int
as
	update inventory
	set amount = amount - 1
	where itemId = @itemId;

	delete from inventory where amount = 0;
go

create procedure removeItemFromInventory @itemId int
as
	delete from inventory where @itemId = itemId
go

create procedure updateAmountItem @itemId int, @amount int
as
	update inventory
	set amount = @amount
	where itemId = @itemId;

	delete from inventory where amount = 0;
go

create procedure searchPlayer @search varchar(64)
as
	select player.id, player.name, life, maxlife, defense, gold, classId, cl.attack, cl.attackDelay, cl.name as className from player
	INNER JOIN class as cl ON player.classId = cl.id
	where player.name like @search + '%';
go

create procedure searchItem @search varchar(64)
as
	select item.id, item.name, itemType, iT.name as itemname, itemPower, gold, 1 as amount from item
	INNER JOIN itemType as iT on itemType = iT.id
	where item.name like @search + '%';
go

create procedure searchInventory @search varchar(64)
as
	select it.id, it.name, itName.name as itemname, it.itemPower, amount, it.gold from inventory
	INNER JOIN item as it on inventory.itemId = it.id
	INNER JOIN itemType as itName on it.itemType = itName.id
	where it.name like @search + '%';
go

create procedure searchEnemy @search varchar(64)
as
	select enemy.id, enemy.name, enemy.life, enemy.maxLife, enemy.defense, e.name as type, e.attack, e.attackDelay from enemy 
	inner join enemyType as e on e.id = enemy.enemyTypeId
	where enemy.name like @search + '%';
go


create procedure sortInventoryByAmount
as
	select it.id, it.name, amount from inventory
	INNER JOIN item as it on inventory.itemId = it.id
	INNER JOIN itemType as itName on it.itemType = itName.id
	order by amount DESC
go

create procedure sortPlayerByGold
as
	select player.id, player.name, life, maxlife, defense, gold, classId, cl.attack, cl.attackDelay, cl.name as className from player
	INNER JOIN class as cl ON player.classId = cl.id
	order by gold DESC
go

create procedure displayPlayerGraveyard @id int
as
	select plr.name as 'Hero', enm.name as 'Has been slain by', COUNT(*) as amount from player_graveyard
	inner join player as plr on id = playerId
	inner join enemy as enm on enm.id = enemyId
	where playerId = @id
	GROUP BY plr.name, enm.name
	
go

create procedure displayEnemyGraveyard @id int
as
	select enm.name as 'Enemy', plr.name as 'Has been slain by', COUNT(*) as 'Amount of times Slain' from enemy_graveyard
	inner join player as plr on id = playerId
	inner join enemy as enm on enm.id = enemyId
	where playerId = @id
	group by enm.name, plr.name;
go

create procedure displayPlayerGraveyard_enemyID @id int
as
	select plr.name as 'Hero', enm.name as 'Has been slain by', COUNT(*) as amount from player_graveyard
	inner join player as plr on id = playerId
	inner join enemy as enm on enm.id = enemyId
	where enemyId = @id
	GROUP BY plr.name, enm.name;
	
go

create procedure displayEnemyGraveyard_enemyID @id int
as
	select enm.name as 'Enemy', plr.name as 'Has been slain by', COUNT(*) as 'Amount of times Slain' from enemy_graveyard
	inner join player as plr on id = playerId
	inner join enemy as enm on enm.id = enemyId
	where enemyId = @id
	group by enm.name, plr.name;
go


create procedure mostKilledEnemy
as
	select enm.name as 'Enemy', COUNT(*) as 'Amount of times Slain' from enemy_graveyard
	inner join enemy as enm on enm.id = enemyId
	group by enm.name
	order by COUNT(*) DESC;
go

create procedure mostKilledPlayer
as
	select plr.name as 'Player', COUNT(*) as 'Amount of times Slain' from player_graveyard
	inner join player as plr on plr.id = enemyId
	group by plr.name
	order by COUNT(*) DESC;
go

create procedure giveGold @id int, @amount int
as
	update player
	set gold = gold + @amount
	where @id = id;
go

create procedure addEnemy @name varchar(64), @life int, @defense int, @enemyType int
as
	insert into enemy(name, life, maxLife, defense, enemyTypeId) values
	(@name, @life, @life, @defense, @enemyType);
go

create procedure updateEnemy @id int, @name varchar(64), @life int, @maxlife int, @defense int, @enemyType int
as
	update enemy
	set 
	name = @name,
	life = @life,
	maxlife = @maxlife,
	defense = @defense,
	enemyTypeId = @enemyType
	where id = @id;
go

create procedure updatePlayer @id int, @name varchar(64), @life int, @maxlife int, @defense int, @classId int
as
	update player
	set 
	name = @name,
	life = @life,
	maxlife = @maxlife,
	defense = @defense,
	classId = @classId
	where id = @id;
go

create procedure updateItem @id int, @name varchar(64), @itemPower int, @itemType int, @Gold int
as
	update Item
	set 
	name = @name,
	itemPower = @itemPower,
	itemType = @itemType,
	gold = @Gold
	where id = @id;
go

create procedure removeEnemy @id int
as
	Delete from enemy where @id = id;
go

create procedure removePlayer @id int
as
	Delete from player where @id = id;
go

create procedure deleteClass @id int
as
	update player
	set classId = 0
	where classId = @id
	Delete from class where @id = id;
go

create procedure createItem @name varchar(64), @itemtype int, @itemPower int, @gold int
as
	insert into  item(name, itemType, itemPower, gold) values
	(@name, @itemtype, @itemPower, @gold);
go

create procedure deleteItem @id int
as
	Delete from inventory where @id = itemId;
	Delete from item where @id = id;
go

exec displayEnemies @id = 0

exec displayInventory

exec playerDead @playerId = 1, @enemyId = 0;
exec playerDead @playerId = 1, @enemyId = 0;
exec playerDead @playerId = 1, @enemyId = 0;
exec playerDead @playerId = 1, @enemyId = 0;
exec playerDead @playerId = 1, @enemyId = 0;
exec playerDead @playerId = 1, @enemyId = 0;
exec playerDead @playerId = 1, @enemyId = 0;

exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;
exec enemyDead @playerId = 1, @enemyId = 0;


exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;
exec addItem @itemId = 1;

exec displayEnemyGraveyard_enemyID @id = 0;


exec displayEnemies @id = -1; 

insert into player select player.name, player.life, player.maxlife, player.defense, player.gold, player.classId from player cross join player as plr

