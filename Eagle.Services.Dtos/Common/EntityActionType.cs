namespace Eagle.Services.Dtos.Common
{
    public enum EntityActionType
    {
        //=================================================
        // Common Actions
        Add = 1,
        Edit = 2,
        Delete = 3,
        View = 4,
        UpdateStatus=5,

        //=================================================
        // Roster Actions
        ViewRosterShift = 1000,

        //=================================================
        // Community Actions
        ViewFeeds = 2000,
        ViewActivity = 2001,
        EditActivity = 2002,
        DeleteActivity = 2003,
        AddActivity = 2004,

        // Likes
        Like = 2012,
        CannotLike = 2013,


        // Comments
        AddComment = 2023,
        EditComment = 2024,
        DeleteComment = 2025,
        CanComment = 2026,
        CannotComment = 2027,

        // Attachments
        AddAttachment = 2035,
        DeleteAttachment = 2036,

        // Blogs
        ViewBlogs = 2200,
        CreateBlog = 2201,
        UpdateBlog = 2202,
        DeleteBlog = 2203,

        //=================================================
        // Training Actions
        ViewCourses = 2400,

        //=================================================
        // Jobs 
        ManageJobs = 2500,

        //=================================================
        // System Management
        ImportEmailData = 3010,

        //=================================================
        // Third Party Provider
        CreateThirdPartyProvider = 3600,
        UpdateThirdPartyProvider = 3601,
        DeleteThirdPartyProvider = 3602,
        ViewThirdPartyProvider = 3603,

        //=================================================
        // Third Party Instance
        CreateThirdPartyInstance = 3604,
        UpdateThirdPartyInstance = 3605,
        DeleteThirdPartyInstance = 3606,
        ViewThirdPartyInstance = 3607,

        //=================================================
        // Third Party Data Persistance Instance
        CreateThirdPartyDataPersistenceInstance = 3608,
        UpdateThirdPartyDataPersistenceInstance = 3609,
        DeleteThirdPartyDataPersistenceInstance = 3610,
        ViewThirdPartyDataPersistenceInstance = 3611,

        //=================================================
        // Third Party Data Persistance
        CreateThirdPartyDataPersistance = 3612,
        UpdateThirdPartyDataPersistance = 3613,
        DeleteThirdPartyDataPersistance = 3614,
        ViewThirdPartyDataPersistance = 3615,

        //=================================================
        // Third Party Data Transfer
        CreateThirdPartyDataTransfer = 3616,
        UpdateThirdPartyDataTransfer = 3617,
        DeleteThirdPartyDataTransfer = 3618,
        ViewThirdPartyDataTransfer = 3619,

        //=================================================
        // Network Group Third Party Instance
        CreateNetworkGroupThirdPartyInstance = 3620,
        UpdateNetworkGroupThirdPartyInstance = 3621,
        DeleteNetworkGroupThirdPartyInstance = 3622,
        ViewNetworkGroupThirdPartyInstance = 3623


          
    }
}
