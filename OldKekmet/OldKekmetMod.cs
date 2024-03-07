using MSCLoader;
using System.IO;
using System.Reflection;
using System.Web;
using UnityEngine;

namespace OldKekmet
{
    public class OldKekmetMod : Mod
    {
        public override string ID => "oldkekmet";

        public override string Name => "Old Kekmet";

        public override string Version => "1.1";

        public override string Author => "アカツキ";

        public override void ModSetup()
        {
            base.ModSetup();

            SetupFunction(Setup.OnLoad, Mod_Load);
        }

        private SettingsCheckBox engineSounds;
        private SettingsCheckBox startingSounds;
        private SettingsCheckBox disableDashboard;
        private SettingsCheckBox disableForkliftArm; 
        private SettingsCheckBox makeForkliftWhite; 
        private SettingsCheckBox oldStartingSystem; 
        private SettingsCheckBox disableRadio;

#if ADVANCED_FEATURES
        private SettingsCheckBox enableSoundImprovementScript;
        private SettingsCheckBox disableForkliftArm1;
        private SettingsCheckBox disableForkliftArm2;
        private SettingsCheckBox disableForkliftArm3;
        
        private SettingsSlider engineThrottleVolume;
        private SettingsSlider engineNoThrottleVolume;
        private SettingsSlider engineThrottlePitchFactor;
        private SettingsSlider engineNoThrottlePitchFactor;
        private SettingsSlider antiStallRPM;
        private SettingsSlider soundImprovementMuteDeaccelThreshold;

        private SettingsColorPicker makeForkliftBlackColor;
#endif



        public override void ModSettings()
        {
            base.ModSettings();

#if ADVANCED_FEATURES   
            Settings.AddHeader(this, "Main");
#endif

            engineSounds = Settings.AddCheckBox(this, "engineSounds", "Old Engine Sounds", true);
            startingSounds = Settings.AddCheckBox(this, "engineStarting", "Old Engine Starting Sounds", true);
            disableDashboard = Settings.AddCheckBox(this, "disableDashboard", "Disable Dashboard", false);
            disableForkliftArm = Settings.AddCheckBox(this, "disableForkliftArm", "Disable Forklift Arm", false);
            makeForkliftWhite = Settings.AddCheckBox(this, "makeForkliftWhite", "Make the forklift white", false);
            oldStartingSystem = Settings.AddCheckBox(this, "startingSystem", "Old Starting System", true);
            disableRadio = Settings.AddCheckBox(this, "radio", "Disable Radio", false);

            Settings.AddButton(this, "Suggest", () =>
            {
                ModUI.ShowYesNoMessage("About to open https://mldkyt.com/suggestions?type=oldkekmet, continue?", () =>
                {
                    Application.OpenURL("https://mldkyt.com/suggestions?type=oldkekmet");
                });
            });

#if ADVANCED_FEATURES
            Settings.AddHeader(this, "Advanced");

            Settings.AddText(this, "The values load after reload.");

            engineThrottleVolume = Settings.AddSlider(this, "engineThrottleVolume", "engineThrottleVolume", 0.1f, 6f, 3f);
            engineNoThrottleVolume = Settings.AddSlider(this, "engineNoThrottleVolume", "engineNoThrottleVolume", 0.1f, 6f, 2.5f);
            engineThrottlePitchFactor = Settings.AddSlider(this, "engineThrottlePitchFactor", "engineThrottlePitchFactor", 0.1f, 4f, 1f);
            engineNoThrottlePitchFactor = Settings.AddSlider(this, "engineNoThrottlePitchFactor", "engineNoThrottlePitchFactor", 0.1f, 4f, 1f);

            soundImprovementMuteDeaccelThreshold = Settings.AddSlider(this, "soundImprovementMuteDeaccelThreshold", "soundImprovementMuteDeaccelThreshold", 0f, 1f, 0.5f);

            enableSoundImprovementScript = Settings.AddCheckBox(this, "enableSoundImprovementScript", "enableSoundImprovementScript", true);

            disableForkliftArm1 = Settings.AddCheckBox(this, "disableForkliftArm1", "disableForkliftArm:FrontLoader/ArmPivot/Arm", true);
            disableForkliftArm2 = Settings.AddCheckBox(this, "disableForkliftArm2", "disableForkliftArm:FrontLoader", true);
            disableForkliftArm3 = Settings.AddCheckBox(this, "disableForkliftArm3", "disableForkliftArm:FrontLoader/ArmPivot/Arm/LoaderPivot/Loader", true);

            makeForkliftBlackColor = Settings.AddColorPickerRGB(this, "makeForkliftWhiteColor", "makeForkliftWhiteColor", Color.black);

            antiStallRPM = Settings.AddSlider(this, "antiStallRPM", "antiStallRPM", 300f, 2500f, 500f);
#endif
        }

        void Mod_Load()
        {
            var resources = GetResource();

            var traktor = GameObject.Find("KEKMET(350-400psi)");
            var traktorSounds = traktor.GetComponent<SoundController>();
            if (engineSounds.GetValue())
            {
                var valmetIdle = resources.LoadAsset<AudioClip>("valmet_idle");

                traktorSounds.engineThrottle = valmetIdle;
                traktorSounds.engineNoThrottle = valmetIdle;
#if ADVANCED_FEATURES
                traktorSounds.engineThrottleVolume = engineThrottleVolume.GetValue();
                traktorSounds.engineNoThrottleVolume = engineNoThrottleVolume.GetValue();
                traktorSounds.engineThrottlePitchFactor = engineThrottlePitchFactor.GetValue();
                traktorSounds.engineNoThrottlePitchFactor = engineNoThrottlePitchFactor.GetValue();
#else
                traktorSounds.engineThrottleVolume = 3;
                traktorSounds.engineNoThrottleVolume = 2.5f;
                traktorSounds.engineThrottlePitchFactor = 1;
                traktorSounds.engineNoThrottlePitchFactor = 1;
#endif

                traktor.transform.GetChild(21).GetComponent<AudioSource>().clip = valmetIdle;
                traktor.transform.GetChild(22).GetComponent<AudioSource>().clip = valmetIdle;
                traktor.transform.GetChild(21).GetComponent<AudioSource>().Play();
                traktor.transform.GetChild(22).GetComponent<AudioSource>().Play();

#if ADVANCED_FEATURES
                if (enableSoundImprovementScript.GetValue())
                {
                    var si = traktor.AddComponent<SoundImprovement>();
                    si.threshold = soundImprovementMuteDeaccelThreshold.GetValue();
                }
#else
                var si = traktor.AddComponent<SoundImprovement>();
#endif
            }

            if (startingSounds.GetValue())
            {
                var valmetStart = resources.LoadAsset<AudioClip>("valmet_start");
                var valmetStarting = resources.LoadAsset<AudioClip>("valmet_starting");

                var start1 = GameObject.Find("MasterAudio/Valmet/start1");
                start1.GetComponent<AudioSource>().clip = valmetStart;

                var start2 = GameObject.Find("MasterAudio/Valmet/start2");
                start2.GetComponent<AudioSource>().clip = valmetStarting;

                var start3 = GameObject.Find("MasterAudio/Valmet/start3");
                start3.GetComponent<AudioSource>().clip = valmetStarting;
            }

            if (disableDashboard.GetValue())
            {
                var dash1 = traktor.transform.GetChild(3);

                dash1.GetChild(2).gameObject.SetActive(false);

                var dash2 = traktor.transform.GetChild(5).GetChild(4);

                dash2.GetChild(7).gameObject.SetActive(false);
                dash2.GetChild(8).gameObject.SetActive(false);
            }

            if (disableForkliftArm.GetValue())
            {
#if ADVANCED_FEATURES
                if (disableForkliftArm1.GetValue())
                {
#endif
                    var forklift1 = GameObject.Find("KEKMET(350-400psi)/Frontloader/ArmPivot/Arm").transform;

                    forklift1.GetChild(0).gameObject.SetActive(false);
                    forklift1.GetChild(1).gameObject.SetActive(false);
                    forklift1.GetChild(3).gameObject.SetActive(false);
                    forklift1.GetChild(4).gameObject.SetActive(false);
                    forklift1.GetChild(5).gameObject.SetActive(false);

                    var forklift2 = traktor.transform.GetChild(5);

                    for (var i = 17; i <= 20; i++)
                        forklift2.GetChild(i).gameObject.SetActive(false);
#if ADVANCED_FEATURES
                }

                if (disableForkliftArm2.GetValue())
                {
#endif
                var forklift3 = GameObject.Find("KEKMET(350-400psi)/Frontloader").transform;

                    forklift3.GetChild(0).gameObject.SetActive(false);
                    forklift3.GetChild(1).gameObject.SetActive(false);
                    forklift3.GetChild(2).gameObject.SetActive(false);
#if ADVANCED_FEATURES
                }

                if (disableForkliftArm3.GetValue())
                {
#endif
                var forklift4 = GameObject.Find("KEKMET(350-400psi)/Frontloader/ArmPivot/Arm/LoaderPivot/Loader").transform;

                    forklift4.GetChild(5).gameObject.SetActive(false);
                    forklift4.GetChild(6).gameObject.SetActive(false);
#if ADVANCED_FEATURES
                }
#endif
            }

            if (makeForkliftWhite.GetValue())
            {
                GameObject.Find("KEKMET(350-400psi)/Frontloader/ArmPivot/Arm/LoaderPivot/Loader/mesh").GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"))
                {
#if ADVANCED_FEATURES
                    color = makeForkliftBlackColor.GetValue()
#else
                    color = new Color(43, 43, 43)
#endif
                };
            }

            if (disableRadio.GetValue())
            {
                traktor.transform.GetChild(14).gameObject.SetActive(false);
            }

            if (oldStartingSystem.GetValue())
            {
                var antiStall = traktor.AddComponent<AntiStall>();
#if ADVANCED_FEATURES
                antiStall.minRPM = antiStallRPM.GetValue();
#endif
            }

            resources.Unload(false);
        }

        AssetBundle GetResource()
        {
            var resourceBytes = new byte[0];

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OldKekmet.Resources.oldkekmet.unity3d"))
            {
                resourceBytes = new byte[stream.Length];
                stream.Read(resourceBytes, 0, resourceBytes.Length);
            }

            return AssetBundle.CreateFromMemoryImmediate(resourceBytes);
        }
    }
}
