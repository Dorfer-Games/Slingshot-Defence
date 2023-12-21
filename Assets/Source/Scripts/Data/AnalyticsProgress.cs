using Source.Scripts.Data.Enum;

namespace Source.Scripts.Data
{
    public class AnalyticsProgress
    {
        public bool IsLoggedFirstLaunched;
     
        public bool IsLoggedTutorialStarted;
        public bool IsLoggedTutorialCompleted;
        public TutorStepType TutorStepType;

    }
}