using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShopNoUsers.Controllers
{
    public class ExternalApiController : Controller
    {
        private const string ApiKey = "3xq99zhmrdeeweb2zq3sybynbrszymkq";
        
        // GET: /<controller>/
        public async Task<IActionResult> Index(string id)
        {
            if( id == null ) return View();
            D3Api.ApiKey = ApiKey;
            var battleTag = new BattleTag( id );
            var career = GetCareer( battleTag );
            return View( career);
        }

        private Career GetCareer( BattleTag battleTag ) {
            return Career.CreateFromBattleTag( battleTag );

            //Console.WriteLine( "BattleTag: " + career.BattleTag.Id );
            //Console.WriteLine( "Last hero played: {0}", career.LastHeroPlayed );
            //Console.WriteLine( "Time played on Monk is {0}", career.TimePlayed.Monk );
            //Console.WriteLine( "Kills: monsters={0} / elites={1} / hardcore monsters={2}", career.Kills.Monsters, career.Kills.Elites,
            //    career.Kills.HardcoreMonsters );
            //Console.WriteLine();
            //Console.WriteLine( "Heroes count: " + career.Heroes.Length );
            //foreach( var heroDigest in career.Heroes ) {
            //    Console.WriteLine( "Hero {0}: {1} is {2} level {3} + {4} last updated {5}",
            //        heroDigest.Id,
            //        heroDigest.Name,
            //        heroDigest.HeroClass,
            //        heroDigest.Level,
            //        heroDigest.ParagonLevel, heroDigest.LastUpdated );

            //    var heroFull = heroDigest.GetHeroFromBattleTag( battleTag );

            //    if( heroFull.Items.MainHand != null ) {
            //        var mainHand = Item.CreateFromTooltipParams( heroFull.Items.MainHand.TooltipParams );
            //        Console.WriteLine( "Hero main hand: level {0} {1} (DPS {2}-{3}) is of type {4}",
            //            mainHand.ItemLevel,
            //            mainHand.Name,
            //            mainHand.Dps.Min, mainHand.Dps.Max,
            //            mainHand.TypeName );
            //    }

            //    if( heroFull.Items.Torso != null ) {
            //        var torso = Item.CreateFromTooltipParams( heroFull.Items.Torso.TooltipParams );
            //        Console.WriteLine( "Hero torso: level {0} {1} (armor {2}-{3}) is of type {4}",
            //            torso.ItemLevel,
            //            torso.Name,
            //            torso.Armor.Min, torso.Armor.Max,
            //            torso.TypeName );
            //    }

            //    Console.WriteLine( "Hero DPS {0}", heroFull.Stats.Damage );
            //}
            //Console.WriteLine();
            //Console.WriteLine( "Fallen Heroes count: " + career.FallenHeroes.Length );
            //foreach( var heroDigest in career.FallenHeroes ) {
            //    Console.WriteLine( "Hero {0}: {1} is {2} level {3} + {4} ", heroDigest.Id, heroDigest.Name, heroDigest.Hardcore, heroDigest.Level,
            //        heroDigest.ParagonLevel );
            //}
        }
    }
}
