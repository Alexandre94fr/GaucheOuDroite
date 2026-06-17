-- SQLite

/*
    DOCUMENTATION:
    
    This file is used to run queries to the data base (SQLiteTrainingDataBase.sqlite)
    You can run specific commands (query) without having to uncomment it.
    Just select the query and do Right click -> Run selected query
*/


PRAGMA foreign_keys = ON;


-- Training code for SQLite:

DROP TABLE IF EXISTS Recipes;
DROP TABLE IF EXISTS Categories;


-- Creating a Table:
CREATE TABLE IF NOT EXISTS Categories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title VARCHAR(50) NOT NULL,
    Description VARCHAR(255)
);


CREATE TABLE IF NOT EXISTS Recipes(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title VARCHAR(50) NOT NULL,
    Slug VARCHAR(100) NOT NULL UNIQUE,
    IsOnline BOOLEAN NOT NULL,
    DurationInMinutes TINYINT NOT NULL,
    CategoryId INTEGER,
    Content TEXT NOT NULL,

    FOREIGN KEY(CategoryId) REFERENCES Categories(Id)
);


-- Creating, modifying, deleting a Column of a Table:
/*
ALTER TABLE Recipes RENAME COLUMN Url TO Slug;
ALTER TABLE Recipes DROP Title;
ALTER TABLE Recipes ADD Title VARCHAR(50);
*/


-- Adding data to the data base
INSERT INTO Categories (
    Title,
    Description
)
VALUES 
(
    'Plat',
    'Quelque chose qu''on mange durant le repas (sauf à la fin)'
),
(
    'Déssert',
    'Quelque chose qu''on mange à la fin du repas.'
);


INSERT INTO Recipes (
    Title,
    Slug,
    IsOnline,
    DurationInMinutes,
    CategoryId,
    Content
)
VALUES 
(
    'Saucisse',
    'saucisse',
    TRUE,
    3,
    1,
    'AAAAA'
),
(
    'Fraises',
    'fraises',
    TRUE,
    10,
    2,
    'BBBBB'
),
(
    'Pain',
    'pain',
    TRUE,
    15,
    NULL,
    'CCCCC'
);


-- Getting some data depending on the CategoryId
SELECT * 
FROM Recipes
JOIN Categories ON Recipes.CategoryId = Categories.Id;

SELECT R.Id, R.Title, C.Title AS Category
FROM Recipes R
JOIN Categories C ON R.CategoryId = C.Id;

SELECT R.Id, R.Title, C.Title AS Category
FROM Recipes R
LEFT JOIN Categories C ON R.CategoryId = C.Id;


-- Getting all the data in the Recipes table
SELECT * FROM Recipes;

--DELETE FROM Recipes WHERE Title = 'Brocoli'