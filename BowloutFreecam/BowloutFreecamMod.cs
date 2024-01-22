using BowloutModManager;
using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutMod.Interfaces;
using UnityEngine;
using System;
using FPSepController;
using UnityEngine.InputSystem;

namespace BowloutFreecam
{
    public class BowloutFreecamMod : IBowloutMod
    {
        public string Name => "Bowlout Freecam";

        public Version Version => new Version(0, 0, 1, 0);

        public string Description => "Allows the user to freecam wherevery they may please :)";

        public bool Enabled { get; set; }

        public IBowloutConfiguration Configuration { get; private set; }
        public FreecamSettings FreecamSettings => (FreecamSettings)Configuration;

        public void OnSetup()
        {
            Configuration = this.GetConfiguration<FreecamSettings>() ?? new FreecamSettings();
            this.SaveConfiguration(Configuration);
            BLogger.WriteLineToLog("Freecam Mod: " + FreecamSettings.Version + " : " + FreecamSettings.ScrollingHandlesSpeed);
        }

        public void Dispose()
        {
            
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

        Vector3 lastPos = Vector3.zero;

        float speed = 4.0f;

        public void OnUpdate()
        {
            if (Keyboard.current[Key.LeftAlt].wasPressedThisFrame)
            {
                freecamOn = !freecamOn;
                if (freecamOn) HandleOn();
                else HandleOff();
            }
            float scroll = 0;
            if (FreecamSettings.ScrollingHandlesSpeed) scroll = Mouse.current.scroll.ReadValue().y * Time.unscaledDeltaTime * FreecamSettings.ScrollingSpeed;

            speed += scroll;

            speed = Mathf.Clamp(speed, 0.5f, 20f);

            if (freecamOn)
            {
                if (Keyboard.current[Key.Escape].wasPressedThisFrame) HandleOff();
                dir = Vector3.zero;

                Vector2 inputDirs = new Vector2(Keyboard.current.dKey.isPressed ? 1.0f : Keyboard.current.aKey.isPressed ? -1.0f : 0.0f, Keyboard.current.wKey.isPressed ? 1.0f : Keyboard.current.sKey.isPressed ? -1.0f : 0.0f);

                dir += curCam.transform.right * inputDirs.x;
                dir += curCam.transform.forward * inputDirs.y;

                dir += Keyboard.current.spaceKey.isPressed ? Vector3.up : Keyboard.current.leftShiftKey.isPressed ? -Vector3.up : Vector3.zero;

                dir *= Time.unscaledDeltaTime;
                dir *= speed;

                PlayerCamLook pCam = Component.FindObjectOfType<PlayerCamLook>();
                if (pCam == null)  return;

                pCam.cameraFollow.position = lastPos;
                pCam.cameraFollow.position += dir;
                lastPos = pCam.cameraFollow.position;   
            }
        }

        void HandleOn()
        {
            freecamOn = true;   
            PauseHandler pHandler = Component.FindObjectOfType<PauseHandler>();
            if (pHandler == null) return;
            pHandler.GAME_PAUSE();
            pHandler.Disable_PauseMenu();
            curCam = Camera.main;
            PlayerCamLook pCam = Component.FindObjectOfType<PlayerCamLook>();
            if (pCam == null) return;
            lastPos = pCam.cameraFollow.position;
            startPos = pCam.cameraFollow.position;
            pCam.OnPlayerUnpause();
        }

        void HandleOff()
        {
            freecamOn = false;
            PauseHandler pHandler = Component.FindObjectOfType<PauseHandler>();
            if (pHandler == null) return;
            pHandler.GAME_UNPAUSE();
            PlayerCamLook pCam = Component.FindObjectOfType<PlayerCamLook>();
            if (pCam == null) return;
            pCam.OnPlayerUnpause();
            curCam.transform.localPosition = new Vector3(0, 0, 0);
            pCam.cameraFollow.position = startPos;
        }

        public void OnLateUpdate()
        {
            
        }

        public void OnFixedUpdate()
        {
            
        }

        public void OnGUI()
        {
            
        }
    }
}
