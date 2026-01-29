namespace ITHSDatabasLabb3MongoDBDungeonCrawlerExtension.UI
{
    internal class StartUpScreen
    {
        public static async Task DrawAsync()
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

            Console.Clear();
            Console.WriteLine(StartText);
            Console.WriteLine("\nPress any key to skip...");

            if (await WaitForKeyOrDelayAsync(2000))
                return;

            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < 19; i++)
            {
                Console.WriteLine(new string(' ', 100));

                if (await WaitForKeyOrDelayAsync(200))
                    return;
            }
        }

        private static async Task<bool> WaitForKeyOrDelayAsync(int milliseconds)
        {
            int step = 25;
            int waited = 0;

            while (waited < milliseconds)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(intercept: true);
                    return true;
                }

                int remaining = milliseconds - waited;
                int chunk = remaining < step ? remaining : step;

                await Task.Delay(chunk);
                waited += chunk;
            }

            return false;
        }
    }
}
