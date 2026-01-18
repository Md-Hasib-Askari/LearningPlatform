using LearningPlatform.Data.Domain.Enums;

public static class GetRole
{
    public static string GetRoleName(RoleEnum role)
    {
        return role switch
        {
            RoleEnum.Admin => "Admin",
            RoleEnum.Staff => "Staff",
            RoleEnum.Instructor => "Instructor",
            RoleEnum.Student => "Student",
            RoleEnum.Guest => "Guest",
            _ => "Unknown"
        };
    }

    public static RoleEnum GetRoleEnum(string roleName)
    {
        return roleName switch
        {
            "Admin" => RoleEnum.Admin,
            "Staff" => RoleEnum.Staff,
            "Instructor" => RoleEnum.Instructor,
            "Student" => RoleEnum.Student,
            "Guest" => RoleEnum.Guest,
            _ => RoleEnum.Guest
        };
    }
}