<img width="1051" height="164" alt="image" src="https://github.com/user-attachments/assets/1ccf4c3d-6544-4496-95c5-4dadeae6075e" />

<br/>
<br/>
<br/>

# 📋 ⮞ Table of contents

- [🔰 ⮞ Why does this project exist?](#--why-does-this-project-exist)
    
- [📦 ⮞ Download](#--download)
    
- [👾 ⮞ Game](#--game)
    - [🎮 ▸ Gameplay](#--gameplay)
    - [⭐ ▸ Scoring](#--scoring)
    - [⌨️ ▸ Multiple ways to play](#️--multiple-ways-to-play)
    - [👤 ▸ User accounts](#--user-accounts)
    
- [🏗️ ⮞ Architecture](#️--architecture)
    - [🌐 ▸ Client / Server](#--client--server)
    - [🔗 ▸ Shared](#--shared)
    - [💾 ▸ Player progression](#--player-progression)
    - [📊 ▸ Data-driven gameplay](#--data-driven-gameplay)
    - [🛡️ ▸ Security](#️--security)
    
- [🧪 ⮞ Testing](#--testing)
    - [👤 ▸ Manual testing](#--manual-testing)
    - [👥 ▸ External testing](#--external-testing)
    - [🤖 ▸ Automated testing](#--automated-testing)
    
- [⚙️ ⮞ CI/CD](#️--cicd)
    - [🔬 ▸ Continuous Integration](#--continuous-integration)
    - [📦 ▸ Continuous Delivery](#--continuous-delivery)
    
- [📚 ⮞ Project documentation](#--project-documentation)
    
- [🔬 ⮞ Major technologies used](#--major-technologies-used)
    - [📟 ▸ Programming](#--programming)
    - [🔀 ▸ Versionning](#--versionning)
    - [🎓 ▸ Gathering information](#--gathering-information)
    - [🌄 ▸ Art](#--art)
    - [📚 ▸ Planning](#--planning)
    - [🔷 ▸ Other](#--other)
    
- [🎁 ⮞ Bonus](#--bonus)




<br/>

# 🔰 ⮞ Why does this project exist?

This project has been **created for two personal reasons**:

<br/>

### 1. Quickly distinguish between the left and the right

I’m currently working on getting my driver’s license.
During my driving lessons, I have noticed that I still have a lot of trouble quickly figuring out which way is left and which way is right.

This has led to a few dangerous moments on the road, where I have driven in the wrong direction because I mixed up left and right.

Usually, I have a mnemonic to help me tell left from right.
I picture a map of Europe and tell myself that left is the West and right is the East.
However, this strategy takes too long by road standards.

So for a while now, I have been trying to train myself to identify left and right very quickly. But it’s still not enough.

<br/>

### 2. Have a project to present that meets the CDA's requirements

In order to fully complete my studies and earn a state-certified degree.  
I must take an exam that requires me to demonstrate to at least two expert examiners in the field, that I possess the skills necessary to earn the "Concepteur Développeur d'Application" (CDA) degree, or "Application Designer and Developer" when translated into English.

The problem I’m facing is that, among the projects I have already created, none of them fully meet the CDA requirements.

<br/>

### The solution?

To solve both of my problems, I decided to kill two birds with one stone.  
I decided to create an application that would validate the skills required to earn my CDA degree and, as an added bonus, help me solve my problem with left and right.  

The application’s goal will be to teach users how to identify left and right very quickly.

The application takes the form of a dynamic game consisting of different levels. The further the player progresses through the levels, the less time they have to identify left or right.  
Depending on their speed, the player receives a score as well as a rating in the form of a three-star rating when the level ends.

<br/>

The project is therefore not limited to creating a functional game.

It also includes an architecture that separates the FrontEnd (Client) from the BackEnd (Server), a REST API that enables communication between them, a relational DataBase for storing user's and game's data, an authentication system, various security measures to prevent potential malicious acts, and a lot of other constraints that, if not presented to the jury, could prevent me from graduating.




<br/>

# 📦 ⮞ Download

If you want to test the project by yourself. You can go in the [project's Releases](https://github.com/Alexandre94fr/GaucheOuDroite/releases).

Each Release (except the v1.0.0) contains an English and French version of the project, they also describe the major changes that have been made to the project since the last version.

<br/>

> [!NOTE]
> If you don't want to get spoiled about what contains the project, you can stop reading here and play the game now.
>
> After that, you can come back here to read more, if you are still interested.



<br/>

# 👾 ⮞ Game

_Gauche ou Droite_ is a reflex-based training game designed to help players improve how quickly they can distinguish between left and right.

The game presents the player with a succession of directions that must be answered within a limited amount of time.

The difficulty progressively increases as the player advances through the different levels.

The objective is simple:

▸ **See the direction**  
▸ **Identify it**  
▸ **Respond as quickly as possible**  


<br/>

### 🎮 ▸ Gameplay

The game currently includes:

- Multiple handcrafted levels  
- Progressive difficulty
- Configurable levels
- Level selection
- Locked and unlocked levels
- Three-star ratings
- Best-score tracking
- A countdown before each level starts
- Pause menu allowing the player to:
  - Resume the game
  
  - Restart the current level
  - Return to the level selection menu
- Feedback based on reaction speed
- Immediate defeat on:
  - Wrong answer
  
  - Timeout (took too long to respond)
- and other small features...


<br/>

### ⭐ ▸ Scoring

The player's score is based on their response speed.

Faster responses result in a higher score, while slower responses provide fewer points.

<br/>

<img width="1920" height="1080" alt="Snapshot_8" src="https://github.com/user-attachments/assets/2e3de19d-c871-4144-9901-544c4e8f031c" />

<br/>

<img width="1920" height="1080" alt="Snapshot_9" src="https://github.com/user-attachments/assets/e39b5a8f-4e51-4041-892a-89f1df5400b7" />

<br/>
<br/>
<br/>

> [!NOTE]
> The reason why the pressed button on the screenshot doesn't match the direction text on the screen is because it's the next direction to guess.
>
> The screenshots have been taken just after giving the good direction.

<br/>
<br/>

At the end of a level, the player receives a rating ranging from **zero to three stars**, depending on their score.

The best score achieved for each level is saved and can be viewed later.

<br/>

<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/2d4578ba-9248-4ac9-bd96-eb7c462344cf" />

<br/>
<br/>


<br/>

### ⌨️ ▸ Multiple ways to play

The player can answer directions using several input methods:

- `A` / `D` keys for QWERTY keyboards, and `Q` / `D` keys for AZERTY keyboards
- `Left` / `Right` arrow keys
- On-screen buttons

<br/>

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/be083444-3e15-43a8-af4b-9aadbea72853" />

<br/>
<br/>

The interface also provides visual feedback when a direction is given.


<br/>

### 👤 ▸ User accounts

The game includes a complete user account system.

Players can:

- Create an account
- Log in to an existing account
- Save their progression
- Load their progression by logging in
- Delete their account and associated data

<br/>

Passwords are never stored directly. They are hashed before being stored in the DataBase.

<img width="615" height="103" alt="image" src="https://github.com/user-attachments/assets/deeb2821-979f-4844-9cab-ffb2cb4713fe" />  


<br/>
<br/>

Authentication is handled using **JWT (JSON Web Tokens)**.

<br/>

> [!NOTE]
> The rest of this ReadMe will be mostly talking about technical stuff.
>
> So if you are not interested in that. You can directly download and test the project by going into the [project's Releases](https://github.com/Alexandre94fr/GaucheOuDroite/releases).




<br/>

# 🏗️ ⮞ Architecture

The project is divided into several applications:

- FrontEnd
- BackEnd
- Shared

<br/>

They are structured this way:

<img width="2720" height="2126" alt="image" src="https://github.com/user-attachments/assets/4de89131-2c31-4e89-ae23-7b5a21ee948b" />

<br/>
<br/>

<br/>

The BackEnd follows a layered organization separating HTTP request handling, business logic, and data access.

The FrontEnd is also organized according to the responsibilities of its different features, rather than simply placing every script into large generic folders.


<br/>

### 🌐 ▸ Client / Server

The game uses a **Client / Server architecture**.

<br/>

The **FrontEnd (Client)** is responsible for:

- User interfaces
- User inputs
- Gameplay
- Displaying information
- Sending requests to the server

<br/>

The **BackEnd (Server)** is responsible for:

- Receiving requests from FrontEnd
- Authentication of users and requests
- Validating received data
- Handling game and user services
- Managing the DataBase
- Returning responses to the FrontEnd

<br/>

The BackEnd is considered the **source of truth** for important data and operations.

The FrontEnd is always considered untrusted and therefore important operations are checked by the BackEnd.

<br/>

The communication between the FrontEnd and BackEnd is performed through HTTP requests using JSON data.

Authenticated requests include a JWT in the HTTP `Authorization` header.


<br/>

### 🔗 ▸ Shared

The Shared application is not a similar application as the FrontEnd and BackEnd are.

Shared is used to centralize common elements such as constants, DTOs, enums, and tools used by both the FrontEnd and BackEnd.

<br/>

This allows the Client and Server to use the same definitions when communicating with each other.

<br/>

If anything is modified in Shared, both FrontEnd and BackEnd will have their values automatically updated.


<br/>

### 💾 ▸ Player progression

Player progression is stored in a SQLite DataBase.

The player's progression includes:

- Unlocked levels
- Best scores for each levels

<br/>

The number of stars does not need to be stored because it can be calculated from the player's score and the score thresholds defined for each level.  
This strategy allows to keep the amount of data that needs to be stored relatively small.


<br/>

### 📊 ▸ Data-driven gameplay

A large part of the game's gameplay configuration is stored as data in the DataBase, rather than being hard-coded directly into the gameplay logic.

This includes information such as:

- If the level is infinite or not
- Level direction sequences
- Maximum response times
- Score needed to gain stars
- Difficulty rank

<br/>

This makes it possible to modify and balance the game without having to update the FrontEnd (Client).


<br/>

### 🛡️ ▸ Security

Several security mechanisms have been implemented, including:

- Client-side validation
- Request rate limiting
- JWT authentication
- Mandatory Server-side validation
- Password hashing + salt
- Limiting SQL injection risks through Server-side validations and Entity Framework Core

<br/>

The Server does not blindly trust data received from the Client.

The goal is to limit unexpected behaviour and reduce the risks associated with malicious or malformed requests.




<br/>

# 🧪 ⮞ Testing

Testing has been performed throughout the development of the project.

The project uses several levels of testing:


<br/>

### 👤 ▸ Manual testing

The game has been constantly tested manually during development, including normal use cases and invalid inputs.

Special attention has been given to:

- Gameplay
- Authentication
- Client / Server communication
- Data validation
- Security mechanisms
- Distributed Builds


<br/>

### 👥 ▸ External testing

Several Builds have also been tested by people outside the project.

These tests had two main goals:

- Find bugs that were not detected during development.
- Collect feedback about the game and its user experience.

<br/>

The feedback was then listed, analysed and, when relevant, integrated into the project.

<br/>

New Builds were subsequently tested to verify the changes.

<br/>
<br/>

This manual testing by testers and I, leads to have this procedure:

<img width="2752" height="2774" alt="image" src="https://github.com/user-attachments/assets/278a0a3a-cbbc-4a49-a44d-8f635228a1fe" />

<br/>

<img width="1888" height="2048" alt="image" src="https://github.com/user-attachments/assets/f56c6946-7a0e-4d0e-b540-396234622dbe" />

<br/>
<br/>


<br/>

### 🤖 ▸ Automated testing

The project now also contains a dedicated C# test project named `GaucheOuDroiteBackEndTests`.

It contains automated **unit and integration tests** for the BackEnd.

<br/>

<img width="892" height="897" alt="image" src="https://github.com/user-attachments/assets/2b6197b8-97a2-4d76-90c9-97bb143562dc" />

<br/>
<br/>

The automated tests are executed automatically by GitHub Actions when a Pull Request targets the `dev` or `main` branches.




<br/>

# ⚙️ ⮞ CI/CD

[Version 1.3.0](https://github.com/Alexandre94fr/GaucheOuDroite/releases/tag/v1.3.0) introduced a complete CI/CD pipeline to automate the testing and deployment process.

This pipeline is managed using **GitHub Actions**.

<br/>

### 🔬 ▸ Continuous Integration

The CI pipeline automatically:

- Restores the BackEnd dependencies
- Compiles the BackEnd and test project
- Runs the automated tests
- Uses the GitHub repository rules to prevent merging when the required tests fail

<br/>

### 📦 ▸ Continuous Delivery

The CD pipeline automatically creates the project Builds when a new version is pushed to the `main` branch.

It handles:

- BackEnd publication
- French FrontEnd Build
- English FrontEnd Build
- Creation of the final Client / Server directory structure
- Creation of the usage instructions
- Generation of the GitHub Release
- Uploading the French and English Builds

<br/>

<img width="900" height="553" alt="image" src="https://github.com/user-attachments/assets/ae16981c-81a1-4e48-a98a-f8c61cf34369" />

<br/>
<br/>

The Unity Builds are generated automatically using **GameCI**.

The BackEnd is published as a self-contained executable, meaning that the user does not need to install the development environment or additional .NET components to launch the distributed Server.

<br/>

The result is a Release containing both the French and English versions of the project.




<br/>

# 📚 ⮞ Project documentation

The project was also documented extensively during its development.

The documentation covers topics such as:

- Project requirements
- Project management
- Planning
- User journeys
- Interface design
- Software architecture
- Client / Server architecture
- DataBase design
- Development
- Security
- Testing
- Deployment
- DevOps
- Project review

<br/>

Two complete documents were also created as part of the CDA certification process.  

One of them, the `Dossier de projet - Passage diplôme CDA - Alexandre` is only about this project and is 72 pages long.  
He goes into much greater detail about how the project was organized and created.  
If you are more curious about how this project was made, I encourage you to go check it out :D

<br/>

> [!NOTE]
> Not all created documents are in the [Documents folder](Documents) because some documents are too valuable to be made publicly available or contain sensitive information.
>
> But you will be able to see screenshots of them in the [project's Releases](https://github.com/Alexandre94fr/GaucheOuDroite/releases), and in the `Dossier de projet - Passage diplôme CDA - Alexandre`, in the [documents folder](Documents).




<br/>

# 🔬 ⮞ Major technologies used

### 📟 ▸ Programming

- C#
- Visual Studio 2026

<br/>

### - FrontEnd

- Unity 6.3 LTS

<br/>

### - BackEnd

- ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- SQLite
- Visual Studio Code + plugins:
  - SQLite (from `alexcvzz`)
    
  - SQLite Viewer (from `Florian Klampfer`)

<br/>

<ins>Testing BackEnd</ins>
- MSTest
- Microsoft Testing Platform


<br/>

### - CI/CD
- GitHub Actions
- GameCI


<br/>

### 🔀 ▸ Versionning

- GitHub
- GitHub Desktop


<br/>

### 🎓 ▸ Gathering information

- Firefox + DuckDuckGo
- ChatGPT


<br/>

### 🌄 ▸ Art

- Gimp
- Canva


<br/>

### 📚 ▸ Planning

- Notepad
- LibreOffice
- Figma
- Canva
- Looping


<br/>

### 🔷 ▸ Other

- Windows




<br/>

# 🎁 ⮞ Bonus

As a bonus, and to thank you for reading this ReadMe.  
Here is a video showing the process of creating, modifying, and deleting files throughout all the development of _Gauche ou Droite_:

<br/>

[Gauche ou Droite creation progression video](https://github.com/user-attachments/assets/e59bb94a-210e-4ecb-bd2d-ef17124bd6a7)

<br/>

> [!NOTE]
> The period from August 17 to September 21 was entirely dedicated to writing the documents required for the CDA degree exam.
> 
> That's why that period is very calm and even mostly skipped.