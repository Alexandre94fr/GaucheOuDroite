-- SQLite

/*
    DOCUMENTATION:
    
    This file is used to run queries to the data base (DataBase.sqlite)
    You can run specific commands (query) without having to uncomment it.
    Just select the query and do Right click -> Run selected query
*/


-- FOR DEVELOPMENT AND DEBUGGING:
-- DELETE FROM Users WHERE Username = 'Alexandre';


PRAGMA foreign_keys = ON;


-- Code for GaucheOuDroite project:

-- You should prefer dropping the tables that are the last created.
DROP TABLE IF EXISTS LevelResponseTimeSteps;
DROP TABLE IF EXISTS UserProgressions;
DROP TABLE IF EXISTS Levels;
DROP TABLE IF EXISTS Users;


CREATE TABLE IF NOT EXISTS Users(
   Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,

   Username VARCHAR(50) UNIQUE NOT NULL,
   PasswordHash VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS Levels(
   Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,

   Name VARCHAR(50) NOT NULL,
   Difficulty VARCHAR(50) NOT NULL,
   IsInfinite NUMERIC NOT NULL,
   ResponseSequence TEXT NOT NULL,
   Star1MinimumScore INTEGER NOT NULL,
   Star2MinimumScore INTEGER NOT NULL,
   Star3MinimumScore INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS UserProgressions(
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    UserId INTEGER NOT NULL,
    LevelId INTEGER NOT NULL,

    IsUnlocked NUMERIC NOT NULL,
    BestScore INTEGER NOT NULL,

    FOREIGN KEY(UserId) REFERENCES Users(Id),
    FOREIGN KEY(LevelId) REFERENCES Levels(Id)
);

CREATE TABLE IF NOT EXISTS LevelResponseTimeSteps(
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    LevelId INTEGER NOT NULL,

    TriggerTimeInSeconds NUMERIC(15,5) NOT NULL,
    MaxResponseTimeInSeconds NUMERIC(15,5) NOT NULL,

    FOREIGN KEY(LevelId) REFERENCES Levels(Id)
);


INSERT INTO Users (
    Username,
    PasswordHash
)
VALUES (
        'Alexandre94.fr',
        'MotDePasseD''Alexandre'
    ),
    (
        'Zaidras',
        'MotDePasseDeClément'
);

INSERT INTO Levels (
    Name,
    Difficulty,
    IsInfinite,
    ResponseSequence,
    Star1MinimumScore,
    Star2MinimumScore,
    Star3MinimumScore
)
VALUES (
        'Niveau 1',
        'Facile',
        FALSE,
        'LRLRRLLL',
        1000,
        2500,
        5000
    ),
    (
        'Infini',
        'Progressif',
        TRUE,
        'L',
        5000,
        12000,
        15000
);

INSERT INTO UserProgressions (
    UserId,
    LevelId,

    IsUnlocked,
    BestScore
)
VALUES (
        1,
        1,

        TRUE,
        6548
    ),
    (
        1,
        2,
    
        TRUE,
        18574
    ),
    (
        2,
        1,
    
        TRUE,
        5746
    ),
    (
        2,
        2,
    
        FALSE,
        0
);

INSERT INTO LevelResponseTimeSteps (
    LevelId,

    TriggerTimeInSeconds,
    MaxResponseTimeInSeconds
)
VALUES (
        1,

        0,
        1
    ),
    (
        2,

        0,
        1
    ),
    (
        2,

        5,
        0.85
    ),
    (
        2,

        9.25,
        0.75
    ),
    (
        2,

        16.75,
        0.5
);