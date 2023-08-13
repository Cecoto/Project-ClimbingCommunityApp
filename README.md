# Project-ClimbingCommunityApp
 This is my SoftUni Project defence application.
 "Climbing communiuty" is user-friendly web application designed for people related to climbing.


 ## Table of Contents

- [Overview](#overview)
- [Build with](#build-with)
- [Getting Started](#getting-started)
- [Features](#features)
- [License](#license)
- [Contact](#contact)
- [Gallery](#gallery)
  
<a id="overview"></a>
## Overview
The Climbing Community application is a platform designed to bring climbing enthusiasts together. It offers a range of features including training programs, events, and user profiles for climbers and coaches. Users can easily find and join training sessions, view coach profiles, upload photos, and interact with fellow climbers in a dedicated community space. With its user-friendly interface and comprehensive functionalities, Climbing Community aims to foster a thriving community of climbers and coaches.

"Climbing community" have to four user types: three registered and one unregistered.

1.Administrator

- Admins can Create, Read, Edit, and Delete all pages ,can comment, and users to roles and create new roles. Also have access to user current status online or offline.

2.Climber

- Climber can Create, Read, Edit, and Delete Climbing trips. They can join and leave other climbing trips and also can join trainings created by coaches, and can comment posts.

3.Coach user

 - Coaches can Create, Read, Edit, and Delete Trainings.They have access to watch climbers posts and comment posts.

4.Guest User

 - Unregistered users can view only the Home page, FAQ and About us pages. Also can reach regisster and login pages.

<a id="getting-started"></a>
## Getting started
To access the project, download the project's zip file and open it with Visual Studio or another IDE. Ensure you have SQL Server Management Studio (SMSS) installed.
Add a connection string to the "Manage Secrets JSON" and also add admin credential you want to use as admin after that.You can use that for easier setup:

```
"ConnectionStrings": {
    "DefaultConnection": "Server=YourServer;Database="SomeDbName";Trusted_Connection=True;MultipleActiveResultSets=true" 
  },
  "AdminSettings": {
     "AdminEmail": "admin@climbingcommunity.com",
     "AdminPassword": "admin123456"    
  }
```
  
This step will change once the application is deployed.Create initial migration via the "package manager Control" to see the test data. You can use the following commands:

Update-Database

<a id="build-wtih"></a>
## Build with
* ASP.NET Core -  .NET 6 
* Entity Framework Core
* Microsoft SQL Server
* JQuery
* AJAX
* HTML&CSS
* TempData messages
* NUnit

<a id="features"></a>
## Features
Welcome to ClimbingCommunity, a web application designed to enhance your climbing experience. Explore the exciting features that our platform offers:

### User Management
Create a personalized account as a climber, coach.
Update your profile with essential details and showcase your experience.

### Training Management
Browse through a diverse selection of training sessions, each with detailed information and organizers' details.
Seamlessly join or leave training sessions based on your availability.

### Search and Filtering
Use our search functionality to find specific training sessions by title, location, or organizer.

### Profile Customization
Add a personal touch by uploading a profile picture that represents your climbing passion.
Display age, experience, and coaching credentials for others to see.

### Photo Upload
Share your climbing adventures with the community by uploading and displaying photos on your profile.
Organize and manage your climbing-related images effortlessly.

### User Roles and Permissions
Administrators have the power to manage user roles and permissions.
Coaches can focus solely on their training sessions, ensuring data privacy.

### User Experience
Enjoy a user-friendly interface designed for intuitive navigation.
Access the application seamlessly from different devices, thanks to our responsive design.

### Admin Panel
Administrators can oversee user accounts, training sessions, and content with ease.
Maintain the security of administrative actions through proper authorization.

### Data Integrity and Security
Robust data validation ensures accurate input from users.
We prioritize the safety of your information through encryption and security best practices.

### Error Handling
Receive clear error messages and notifications for a smooth user experience.
Our application handles unexpected errors gracefully to minimize disruptions.

### Documentation
Find comprehensive instructions on setting up and using the application in our documentation.
Get started quickly with installation steps, usage guidelines, and prerequisites.

### Testing
Our application is thoroughly tested through unit tests and integration tests.
Trust in our commitment to delivering a reliable and bug-free experience.
<a id="gallery"></a>
## Gallery
### Home page - guest users
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/869e81fa-57f2-41c3-8fba-c3eeb93e2d44)
### FAQ page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/a2eab675-a9d9-4272-934b-202ac4a612ca)
### About us page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/764b4757-fb52-4ab5-9c80-2e70105007f3)
### Pop up page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/6968fe81-49c9-4110-8a98-c792634a7ec6)
###Register Climber page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/30e4d83f-853f-4b3c-bc52-f3d8e5a4a470)
### Registet Coach page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/caf9c9cd-072d-4584-a2c2-9b3982ce8168)
### LoginPage
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/0cdbc86c-5e2f-404a-94c3-153d5d9c0f08)
### Home page for logged in users
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/fd096fe7-0709-43ae-ac01-3ce742a164d6)
### Users Profile page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/d740007b-1176-42fe-a00e-59b91a3bcfdb)
### All page
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/92d00c92-c89a-49fc-9829-042a42677e78)
### Admin pages
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/85e3775d-3fdb-40da-adea-63dd558da85f)
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/43aba76f-1da3-46d0-a927-8b75c5e1172d)
![image](https://github.com/Cecoto/Project-ClimbingCommunityApp/assets/106250052/559c0ce9-b3af-4536-8115-6d25bf785a53)
<a id="contact"></a>
## Contact
If you have any questions, suggestions, or feedback, please feel free to contact us:

- Email: cecohristov97@gmail.com

We appreciate your interest and engagement with the Climbing Community project! Your input helps us improve and grow the platform.

<a id="license"></a>
## License
This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code according to the terms of the license.

