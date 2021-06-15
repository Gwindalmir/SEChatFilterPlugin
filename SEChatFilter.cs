using HarmonyLib;
using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;

namespace Gwindalmir.Plugins.SEChatFilter
{
    public class SEChatFilter : IPlugin
    {
        MySandboxGame m_game;
        private bool m_offensiveSettingPrevious;
        private bool disposedValue;
        bool m_inited = false;
        bool m_initReady = false;

        public void Init(object gameInstance)
        {
            m_game = gameInstance as MySandboxGame;
            MySession.BeforeLoading += MySession_BeforeLoading;
            MySession.OnUnloading += MySession_OnUnloading;

            //Harmony.DEBUG = true;
            var harmony = new Harmony("Gwindalmir.Plugins.SEChatFilter");
#if DEBUG
            harmony.PatchAll();
#else
            // ILRepack breaks PatchAll, so match manually
            harmony.Patch(AccessTools.Method(typeof(MyHudChat), "ShowMessage", new[] { typeof(string), typeof(string), typeof(Color), typeof(Color) }), new HarmonyMethod(typeof(MyHudChatPatch), "Prefix"));
#endif
        }

        private void MySession_OnUnloading()
        {
            // Restore previous value before final save
            MySession.Static.Settings.OffensiveWordsFiltering = m_offensiveSettingPrevious;
            m_inited = false;
        }

        private void MySession_BeforeLoading()
        {
            MySession.Static.OnReady += MySession_OnReady;
        }

        private void MySession_OnReady()
        {
            m_offensiveSettingPrevious = MySession.Static.Settings.OffensiveWordsFiltering;
            m_initReady = true;
        }

        public void Update()
        {
            if(!m_inited && !m_initReady && MySession.Static?.Settings.OffensiveWordsFiltering == false)
            {
                m_inited = true;
                MySession.Static.Settings.OffensiveWordsFiltering = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
