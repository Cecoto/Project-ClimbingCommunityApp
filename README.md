# Project-ClimbingCommunityApp
 This is my SoftUni Project defence application.
 "Climbing communiuty" is user-friendly web application designed for people related to climbing.


 ## Table of Contents

- [Overview](#overview)
- [Build with](#build-with)
- [Getting Started](#getting-started)
- [Credentials](#credentials)
- [Features](#features)
- [License](#license)
- [Contact](#contact)

<a id="overview"></a>
## Overview
The Climbing Community application is a platform designed to bring climbing enthusiasts together. It offers a range of features including training programs, events, and user profiles for climbers and coaches. Users can easily find and join training sessions, view coach profiles, upload photos, and interact with fellow climbers in a dedicated community space. With its user-friendly interface and comprehensive functionalities, Climbing Community aims to foster a thriving community of climbers and coaches.
<a id="getting-started"></a>
## Getting started
o access the project, download the project's zip file and open it with Visual Studio or another IDE. Ensure you have SQL Server Management Studio (SMSS) installed.
Add a connection string to the "Manage Secrets JSON" and also add admin credential you want to use as admin after that.You can use that for easier setup:

"ConnectionStrings": {
    "DefaultConnection": "Server=;Database="";Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "AdminSettings": {
    "AdminEmail": "admin@climbingcommunity.com",
    "AdminPassword": "admin123456"
  }
  
This step will change once the application is deployed.Create initial migration via the "package manager Control" to see the test data. You can use the following commands:

Add-Migration initial

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

<a id="contact"></a>
## Contact
If you have any questions, suggestions, or feedback, please feel free to contact us:

- Email: cecohristov97@gmail.com

We appreciate your interest and engagement with the Climbing Community project! Your input helps us improve and grow the platform.

<a id="license"></a>
## License
This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code according to the terms of the license.

