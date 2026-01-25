using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI
{
    internal class StartUpScreen
    {
        public static void Draw()
        {
            string StartText =
@"
  _________              __                  
 /   _____/ ____   ____ |  | __ ____   ______
 \_____  \ /    \_/ __ \|  |/ // __ \ /  ___/
 /        \   |  \  ___/|    <\  ___/ \___ \ 
/_______  /___|  /\___  >__|_ \\___  >____  >
        \/     \/     \/     \/    \/     \/ 
                              .___           
           _____    ____    __| _/           
           \__  \  /    \  / __ |            
            / __ \|   |  \/ /_/ |            
           (____  /___|  /\____ |            
                \/     \/      \/            
      __________         __                  
      \______   \_____ _/  |_  ______        
       |       _/\__  \\   __\/  ___/        
       |    |   \ / __ \|  |  \___ \         
       |____|_  /(____  /__| /____  >        
              \/      \/          \/         
";
            Console.WriteLine(StartText);
            Thread.Sleep(2000);
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < 19; i++)
            {
                Console.WriteLine(new string(' ',100));
                Thread.Sleep(200);
            }
        }
    }
}
