using System.ComponentModel;

namespace Eagle.Entities.SystemManagement
{
    public enum Alignment
    {
        [Description("Left")]
        Left = 1,
        [Description("Right")]
        Right = 2,
        [Description("Center")]
        Center =3,
        [Description("Start")]
        Start = 4,
        [Description("End")]
        End = 5
    }
}