namespace ClimbingCommunity.Common
{
    public class NotificationMessageConstants
    {
        public const string ErrorMessage = "ErrorMessage";
        public const string WarningMessage = "WarningMassage";
        public const string InformationMessage = "InfoMassage";
        public const string SuccessMessage = "SuccessMessage";

        public const string GeneralErrorMessage = "Unexpected error occured! Please try again later or contact administrator.";
        public const string InvalidUserIdMessage = "User with provided ID does not exist!";
        public const string NoAccessMessage = "You don't have access to this page!";
        public const string MustBeLoggedInMessage = "You must be logged in to reach that page!";
        public const string InvalidActivityTypeMessage = "This type of activity does not exist in our climbing community!";
        public const string InvalidActivityIdMessage = "Activity with the provided id does not exist! Please try again.";
        public const string SuccessfullyAddedCommentMessage = "Successfully added a comment!";

        public const string MustBeMemberMessage = "You need to be a member of the community to reach that page!";

        public const string MustBeAdminMessage = "You must be an administrator to have access to that pofile!";

        public const string InvalidLevelMessage = "Invalid level id, please select it again!";

        public const string InvalidSpecialityMessage = "Invalid speciality id, please select it again!";

        public const string SucceessfulllyEditedProfileMessage = "Your profile was successfully edited!";

        public const string NoPhotoSelecedMessage = "No photos selected.";

        public const string SuccessfullyUploadedMessage = "Successfully uploaded.";

        public const string InvalidClimbingTripIdMessage = "Climbing trip with the provided id does not exist!";


        public class AdminControllerMessages
        {
            public const string AlreadyCoachMessage = "You are already a coach!";

            public const string BecameCoachMessage = "You successfully become a coach!";

            public const string AlreadyClimberMessage = "You are already a climber!";

            public const string BecameClimberMessage = "You successfully become a climber!";

            public const string SuccessfullyActivatedActivityMessage = "Succesfully reactivted that activity in the application!";

            public const string EmptyRoleMessage = "Role name cannot be empty!";

            public const string RoleAlreadyExistMessage = "Role already exists.";

            public const string InvalidUserEmailMessage = "User with selected email does not exists!";

            public const string AlreadyInRoleMessage = "User is already in that role!";

        }

        public class ClimbingTripControllerMessages
        {

        public const string UnexpectedErrorMessage = "Unexpected error occured while trying to add your new Climbing trip! Please try again leter or contact the administartor!";

        public const string MustBeClimberMessage = "You must be a climber to have access to that page!";

       

        public const string MustBeClimberToAddMessage = "You must be a climber to add new climbing trips!";

        public const string SuccessfullyAddedMessage = "Climbing trip was added successfully!";

        public const string SuccessfullyEditedMessage = "Climbing trip was successfully edited!";

        public const string SuccessfullyDeletedMessage = "Succesfully deleted that activity from the application!";

        public const string SuccessfullyJoinedMessage = "Successfuly joined in that trip!";

        public const string SuccessfullyLeftMessage = "Successfuly left that trip!";

        public const string InvalidTripTypeMessage = "Selected trip type does not exist!";

        public const string MustBeOrganizatorMessage = "You must be a climber and organizator of the trip in order to have access to delete or edit this climbing trip!";

        public const string MustBeClimberToJoinTripMessage = "You must be climber in order to join this climbing trips!";

        public const string YouAreOrganizatorMessage = "You are the organizator of the trip!";

        public const string NotParticipantMessage = "You are not participant of the that trip!";

        }

        

    }
}
