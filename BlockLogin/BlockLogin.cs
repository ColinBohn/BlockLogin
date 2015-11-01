using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace BlockLogin
{
    [ApiVersion(1, 22)]
    public class BlockLogin : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version("1.1"); }
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
            Order = 500;
        }
        public override void Initialize()
        {
            ServerApi.Hooks.ServerChat.Register(this, OnChat);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerChat.Deregister(this, OnChat);
            }
            base.Dispose(disposing);
        }
        private void OnChat(ServerChatEventArgs args)
        {
            if (args.Handled) { return; }
            string[] array = args.Text.Split(' ');
            if (String.IsNullOrWhiteSpace(array[0])) { return; }
            TShock.Log.ConsoleInfo("array: " + array[0][0]);
            if (array[0][0].ToString().Equals(TShock.Config.CommandSpecifier) || array[0][0].ToString().Equals(TShock.Config.CommandSilentSpecifier)) { return; }
            TSPlayer player = TShock.Players[args.Who];
            Match match = Regex.Match(array[0], ".*l.*o.*g.*i.*n.*", RegexOptions.IgnoreCase);
            if (match.Success && (array.Length == 2))
            {
                string pass = array[1];
                var user = TShock.Users.GetUserByName(player.Name);
                if (user == null)
                {
                    return;
                }
                if (user.VerifyPassword(pass))
                {
                    player.SendErrorMessage("Woah there! You almost said your password in chat. Use " + TShock.Config.CommandSpecifier + "login with the " + TShock.Config.CommandSpecifier);
                    args.Handled = true;
                    return;
               }
            }
            return;
        }
    }
}