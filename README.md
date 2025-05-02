# Overview
This app is simple group-based polling platform where users can create and vote on polls 
within their assigned groups
# Identity and Authorization Architecture
This application uses a federated identity and access control model built around two core services
## SimpleTen (as Identity Provider)
- Acts as an **OpenID Connect (OIDC)** provider
- Maintains static JSON configuration for 
    - Users
    - Clients
    - Applications and it's scopes
- Authenticate users and issues identity and access tokens
## SimpleTor (Authorization Service)
- Manages application-specific roles and user metadata
- SimpleTor is registered in SimpleTen with a set of scope, to access it's endpoint
- Provide endpoints to(some):
    - Get user's roles within applications
    - Get user profile
        - Like email, name
    - Get users within application

# User Roles
There are two roles for this application
### Admin
- Manages group creation
- Assign user to group
- Can't create, vote poll in the group. Also can't be part of any group
### Member
- Create poll within their group(s)
- Can vote in polls created in their group(s)

# Entity Relationship
## Identity (`User`)
- The application **does not store full user profiles** in its own database.
- Users are authenticated through an identity provider
## Groups
Represents a collection of Member(s).
## GroupMembers
This is a join (junction) table that models a **many-to-many relationship** between `User` and `Group`.
## Polls
- A poll consists of a question and several options(`Choices`)
- Created by a member inside a group(`Groups`) and visible only to members of that group(`Groups`).
    - A group(`Groups`) can have many `Polls`
## Choices
- Represent one of possible options in a poll(`Polls`)
- One poll(`Polls`) can have many `Choices`
## Voters
- Voter is a member of group(`Groups`) that give vote to poll(`Polls`)
- It is a join(junction) table that models **many-to-many relationship** between `User` and `Polls`
## Answers
- it records user's selected option for a given poll
- It is a join(junction) table that models **many-to-many relationship** between `Voters` and `Choices`


# Migration db command related
- Set connection string in `DefaultConnection` field at `ConnectionStrings` section at appsettings.json
- `dotnet ef database update -p src\03.Infrastructure --startup-project src\06.WebAPI`
