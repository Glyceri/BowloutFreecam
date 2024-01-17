using BowloutModManager;
using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutMod.Interfaces;
using UnityEngine;
using System;
using FPSepController;

namespace BowloutFreecam
{
    public class BowloutFreecamMod : IBowloutMod
    {
        public string Name => "Bowlout Freecam";

        public Version Version => new Version(0, 0, 1, 0);

        public string Description => "Allows the user to freecam wherevery they may please :)";

        public IBowloutConfiguration Configuration { get; private set; }
        public FreecamSettings FreecamSettings => (FreecamSettings)Configuration;

        public void OnSetup()
        {
            BLogger.WriteLineToLog("Hello from Freecam :D");
            Configuration = this.GetConfiguration<FreecamSettings>() ?? new FreecamSettings();
            this.SaveConfiguration(Configuration);
            BLogger.WriteLineToLog(FreecamSettings.Version + " : " + FreecamSettings.ScrollingHandlesSpeed);
        }

        public void Dispose()
        {
            BLogger.WriteLineToLog("Lights out innit!");
        }

        public void OnEnable()
        {
            
        }

        public void OnDisable()
        {
            
        }

        bool freecamOn = false;
        Camera curCam;
        Vector3 dir = new Vector3();

        Vector3 startPos;

        float speed = 4.0f;

        public void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                freecamOn = !freecamOn;
                if (freecamOn) HandleOn();
                else HandleOff();
            }
            float scroll = 0;
            if (FreecamSettings.ScrollingHandlesSpeed) scroll = UnityEngine.Input.mouseScrollDelta.y * Time.unscaledDeltaTime * FreecamSettings.ScrollingSpeed;

            speed += scroll;

            speed = Mathf.Clamp(speed, 0.5f, 20f);

            if (freecamOn)
            {
                dir = Vector3.zero;

                dir += curCam.transform.right * Input.GetAxisRaw("Horizontal");
                dir += curCam.transform.forward * Input.GetAxisRaw("Vertical");
                dir += Input.GetKey(KeyCode.Space) ? Vector3.up : Input.GetKey(KeyCode.LeftShift) ? -Vector3.up : Vector3.zero;

                dir *= Time.unscaledDeltaTime;
                dir *= speed;

                PlayerCamLook pCam = Component.FindObjectOfType<PlayerCamLook>();
                if (pCam == null) return;
                pCam.vCam.transform.position += dir;
            }
        }

        void HandleOn()
        {
            PauseHandler pHandler = Component.FindObjectOfType<PauseHandler>();
            if (pHandler == null) return;
            pHandler.GAME_PAUSE();
            pHandler.Disable_PauseMenu();
            curCam = Camera.main;
            PlayerCamLook pCam = Component.FindObjectOfType<PlayerCamLook>();
            if (pCam == null) return;
            startPos = pCam.vCam.transform.position;
            pCam.OnPlayerUnpause();
        }

        void HandleOff()
        {
            PauseHandler pHandler = Component.FindObjectOfType<PauseHandler>();
            if (pHandler == null) return;
            pHandler.GAME_UNPAUSE();
            PlayerCamLook pCam = Component.FindObjectOfType<PlayerCamLook>();
            if (pCam == null) return;
            pCam.OnPlayerUnpause();
            curCam.transform.localPosition = new Vector3(0, 0, 0);
            pCam.vCam.transform.position = startPos;
        }

        public void OnLateUpdate()
        {
            
        }

        public void OnFixedUpdate()
        {
            
        }
    }
}
