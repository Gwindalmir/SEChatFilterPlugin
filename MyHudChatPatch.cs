using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Gwindalmir.Plugins.SEChatFilter
{
    [HarmonyPatch(typeof(MyHudChat), "ShowMessage", typeof(string), typeof(string), typeof(Color), typeof(Color))]
    public class MyHudChatPatch
    {
        static void Prefix(string sender, ref string message, Color senderColor, Color messageColor)
        {
            var messagebuilder = new StringBuilder(message);

            string offensive;
            while ((offensive = MyScreenManager.ValidateText(messagebuilder)) != null)
            {
                messagebuilder.Replace(offensive, "*****");
            }

            message = messagebuilder.ToString();
        }
    }
}
