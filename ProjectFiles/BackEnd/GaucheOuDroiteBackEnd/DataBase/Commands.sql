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
   Difficulty INTEGER NOT NULL CHECK (Difficulty >= 0),
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

    MinimumCorrectResponses INTEGER NOT NULL,
    MaximumResponseTimeInSeconds NUMERIC(15,5) NOT NULL,

    FOREIGN KEY(LevelId) REFERENCES Levels(Id)
);


-- No more manual User adding 
/*
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
*/

-- All the Levels data will be create here
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
        0,
        FALSE,
        'LRLRRLLL',
        3000,
        5000,
        8000
    ),
    (
        'Niveau 2',
        0,
        FALSE,
        'LLRLRRLRLL',
        5000,
        7500,
        10000
    ),
    (
        'Niveau 3',
        1,
        FALSE,
        'LRLLRRLLRLLR',
        7500,
        10000,
        12000
    ),
    (
        'Niveau 4',
        1,
        FALSE,
        'RLLRRLRLRLLRLL',
        10000,
        12000,
        14000
    ),
    (
        'Niveau 5',
        2,
        FALSE,
        'RLRLLLRRLRLLRRLLRRL',
        12500,
        15000,
        19000
    ),
    (
        'Niveau 6',
        2,
        FALSE,
        'LRRLRLLRRRRLRLLRLLRRRLLR',
        15000,
        20000,
        24000
    ),
    (
        'Infini',
        3,
        TRUE,
        'L',
        35000,
        50000,
        100000
);

-- No more manual UserProgressions adding 
/*
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
*/

INSERT INTO LevelResponseTimeSteps (
    LevelId,

    MinimumCorrectResponses,
    MaximumResponseTimeInSeconds
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
        3,

        0,
        0.85
    ),
    
    (
        4,

        0,
        0.85
    ),
    
    (
        5,

        0,
        0.5
    ),
    
    (
        6,

        0,
        0.5
    ),

    (
        7,

        0,
        1
    ),
    (
        7,

        5,
        0.85
    ),
    (
        7,

        15,
        0.75
    ),
    (
        7,

        30,
        0.5
);