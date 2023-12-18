using Kuhpik;
using Source.Scripts.UI.Screen;

namespace Source.Scripts.System.Util
{
    public class SettingsSystem : GameSystemWithScreen<SettingsUIScreen>
    {
        public override void OnInit()
        {
            screen.OnSettingsButtonClick += ToggleSettings;
            screen.OnHapticButtonClick += ToggleHaptic;
            
            screen.ToggleHaptic(save.VibroOn);
        }
        public override void OnGameEnd()
        {
            base.OnGameEnd();
            screen.OnSettingsButtonClick -= ToggleSettings;
            screen.OnHapticButtonClick -= ToggleHaptic;
        }

        private void ToggleSettings()
        {
            screen.ToggleSettings();
        }
        
        private void ToggleHaptic()
        {
            save.VibroOn = !save.VibroOn;
            screen.ToggleHaptic(save.VibroOn);
            Bootstrap.Instance.SaveGame();
        }
    }
}