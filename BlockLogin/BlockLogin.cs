using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using TShockAPI;

namespace BlockLogin
{
    [APIVersion(1, 12)]
    public class BlockLogin : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version("1.0.1"); }
        }
        public override string Name
        {
            get { return "Login Blocker"; }
        }
        public override string Author
        {
            get { return "Colin"; }
        }
        public override string Description
        {
            get { return "Blocks typing login without /."; }
        }

        public BlockLogin(Main game)
            : base(game)
        {
            Order = -10;
        }
        public override void Initialize()
        {
            Hooks.ServerHooks.Chat += OnChat;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Hooks.ServerHooks.Chat -= OnChat;
            }
            base.Dispose(disposing);
        }
        private void OnChat(messageBuffer msg, int ply, string text, HandledEventArgs args)
        {
            string[] array = text.Split(' ');
            if (args.Handled) { return; }
            if (array[0][0] == '/') { return; }
            TSPlayer player = TShock.Players[ply];
            if (array[0].Contains("login") && (array.Length == 2))
            {
                string encrPass = TShock.Utils.HashPassword(array[1]);
                var user = TShock.Users.GetUserByName(player.Name);
                if (user == null)
                {
                    return;
                }
                if (user.Password.ToUpper() == encrPass.ToUpper())
                {
                    player.SendMessage("Woah there! You almost said your password in chat. Use /login with the slash.", Color.Red);
                    args.Handled = true;
                    return;
               }
            }
            return;
        }
    }
}