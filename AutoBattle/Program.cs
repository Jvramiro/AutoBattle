using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = GetGridChoice();
            CharacterClass playerCharacterClass;
            GridBox PlayerCurrentLocation;
            GridBox EnemyCurrentLocation;
            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles = grid.grids.Count;
            StartGameDisplay();
            Setup(); 

            Grid GetGridChoice(){
                int value;
                bool checkValue = false;
                Grid toReturn = new Grid(ReadInt("X"),ReadInt("Y"));

                return toReturn;
                
            }

            void Setup()
            {
                GetPlayerChoice();
                CreateEnemyCharacter();
                StartGame();
            }

            void StartGameDisplay(){
                Console.WriteLine("=====================================================\n");
                Console.WriteLine("=====================================================\n");
                Console.WriteLine("Starting Auto-Battle PVP");
                Console.WriteLine("[Player 0] HP: 100 ---- [Player 1] HP: 100\n");
                Console.WriteLine("=====================================================\n");
                Console.WriteLine("=====================================================\n");
            }

            void FinishGameDisplay(int winnerIndex){
                Console.WriteLine("=====================================================\n");
                Console.WriteLine("=====================================================\n");
                Console.WriteLine("Finishing Auto-Battle PVP");
                Console.WriteLine($"[Player {winnerIndex} Wins]\n");
                Console.WriteLine("=====================================================\n");
                Console.WriteLine("=====================================================\n");
            }

            void GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        Console.WriteLine("Invalid Value\n");
                        GetPlayerChoice();
                        break;
                }
            }

            int ReadInt(string axis)
            {
                int value;
                bool checkValue = false;
                int toReturn = 0;

                while(!checkValue){
                    Console.Write($"Choose the {axis} Grid Size: ");
                    var choice = Console.ReadLine();

                    if(int.TryParse(choice, out value)){
                        if(value > 1 && value < 11){
                            toReturn = value;
                            checkValue = true;
                        }
                        else{
                            Console.WriteLine("Insert a value between 2~10");
                        }
                    }
                    else{
                        Console.WriteLine("Insert a valid value");
                    }
                }

                return toReturn;
            }

            void CreatePlayerCharacter(int classIndex)
            {
                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                PlayerCharacter = new Character(characterClass);
                PlayerCharacter.Health = 100;
                PlayerCharacter.BaseDamage = 20;
                PlayerCharacter.PlayerIndex = 0;
            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                EnemyCharacter = new Character(enemyClass);
                EnemyCharacter.Health = 100;
                EnemyCharacter.BaseDamage = 20;
                EnemyCharacter.PlayerIndex = 1;

            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                AlocatePlayers();
                
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Click on any key to start the next turn...\n");
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.ReadLine();

                StartTurn();

            }

            void StartTurn(){

                if (currentTurn == 0)
                {
                    Random Rand = new Random();
                    AllPlayers = AllPlayers.OrderBy(q => Rand.Next()).ToList();
                }

                foreach(Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if(PlayerCharacter.Health <= 0 || EnemyCharacter.Health <= 0){
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    int winnerIndex = PlayerCharacter.Health <= 0 ? 1 : 0;
                    FinishGameDisplay(winnerIndex);
                    Console.WriteLine("Click on any key to finish the application...\n");
                    ConsoleKeyInfo key = Console.ReadKey();

                    Console.Write(Environment.NewLine + Environment.NewLine);
                }
                else{
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            void AlocatePlayers()
            {
                var rand = new Random();
                int random = rand.Next(0,grid.grids.Count);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    GridBox PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    Console.WriteLine($"Player {PlayerCharacter.PlayerIndex} started on {PlayerCharacter.currentBox.Index}");
                    AlocateEnemyCharacter();
                } else
                {
                    AlocatePlayers();
                }
            }

            void AlocateEnemyCharacter()
            {
                var rand = new Random();
                int random = rand.Next(0,grid.grids.Count);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    grid.drawBattlefield(grid.xLenght , grid.yLength, grid);
                }
                else
                {
                    AlocateEnemyCharacter();
                }

                
            }

        }
    }
}
