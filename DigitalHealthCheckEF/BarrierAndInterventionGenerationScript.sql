CREATE TABLE TempBarriers
(
	Id int NOT NULL IDENTITY,
	Category varchar(1000) NOT NULL,
	[Order] int NOT NULL,
	[Text] varchar(1000) NOT NULL,
	CONSTRAINT AK_TransactionID UNIQUE(Category,[Text])
)

CREATE TABLE TempInterventions
(
	Id int NOT NULL IDENTITY,
	Category varchar(1000) NOT NULL,
	[Text] varchar(1000) NOT NULL,
	[Url] varchar(1000) NOT NULL,
	[LinkTitle] varchar(1000) NOT NULL,
	[LinkDescription] varchar(1000) NOT NULL,
	[BarrierId] int NULL
)

/* INSERT BARRIER INSERTS HERE */

/* INSERT INTERVENTION INSERTS HERE */

SELECT CONCAT('new Barrier { Id = ',Id,', Category = "',Category,'", Order = ',[Order],', Text = "',[Text],'"},') val
FROM TempBarriers

SELECT CASE
	WHEN BarrierId IS NULL THEN CONCAT('new { Id = ',Id,', Category = "',Category,'", Text = "',[Text],'", Url = "',[Url],'", LinkTitle = "',LinkTitle,'", LinkDescription = "',LinkDescription,'"},')
	ELSE CONCAT('new { Id = ',Id,', Category = "',Category,'", Text = "',[Text],'", Url = "',[Url],'", LinkTitle = "',LinkTitle,'", LinkDescription = "',LinkDescription,'", BarrierId = ', BarrierId, '},')
END FROM TempInterventions

DROP TABLE TempBarriers
DROP TABLE TempInterventions