using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health;
        public float BaseDamage;
        public float DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; } 
        public Character(CharacterClass _characterClass){characterClass = _characterClass;}
        public CharacterClass characterClass;


        public void UpdateHealth(float amount)
        {
            Health = (Health + amount) > 0 ? Health += amount : 0;
        }

        public void StartTurn(Grid battlefield)
        {
            if (CheckCloseTargets(battlefield)) 
            {
                var Rand = new Random();
                int skill = Rand.Next(0,3);
                if(skill != 0){
                    Attack(Target);
                }
                else{
                    switch(characterClass){
                        case CharacterClass.Paladin : IncreaseAttack();
                        break;
                        case CharacterClass.Warrior : FocusedAttack(Target);
                        break;
                        case CharacterClass.Cleric : Heal();
                        break;
                        case CharacterClass.Archer : RandomArrows(Target);
                        break;
                    }
                }
                
            
                return;
            }
            else
            {   // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if(this.currentBox.xIndex > Target.currentBox.xIndex)
                {
                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                    {
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                        if(currentBox.ocupied){return;}
                        currentBox.ocupied = true;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength, battlefield);

                        return;
                    }
                } else if(currentBox.xIndex < Target.currentBox.xIndex)
                {
                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index + 1)))
                    {
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
                        if(currentBox.ocupied){return;}
                        currentBox.ocupied = true;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked right\n");
                        battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength, battlefield);
                    }

                    return;
                }

                if (currentBox.yIndex > Target.currentBox.yIndex)
                {
                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - battlefield.xLenght)))
                    {
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght));
                        if(currentBox.ocupied){return;}
                        currentBox.ocupied = true;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked up\n");
                        battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength, battlefield);
                    }

                    return;
                }
                else if(currentBox.yIndex < Target.currentBox.yIndex)
                {
                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index + battlefield.xLenght)))
                    {
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght));
                        if(currentBox.ocupied){return;}
                        currentBox.ocupied = true;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked down\n");
                        battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength, battlefield);
                    }

                    return;
                }

                battlefield.drawBattlefield(battlefield.xLenght, battlefield.yLength, battlefield);
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght).ocupied);

            //Console.WriteLine($"left {left} right {right} up {up} down {down}");

            if (left || right || up || down) 
            {
                return true;
            }
            return false; 
        }

        public void Attack (Character target)
        {
            var rand = new Random();
            int damage = rand.Next(0, (int)BaseDamage);
            target.UpdateHealth(-damage);
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {Target.PlayerIndex} and did {damage} damage\n");
            ShowHealth();
        }
        public void IncreaseAttack ()
        {
            BaseDamage += 5;
            Console.WriteLine($"Player {PlayerIndex} used Increase Attack and increased Base Damage to {BaseDamage}\n");
            ShowHealth();
        }
        public void FocusedAttack (Character target)
        {
            var rand = new Random();
            int damage = rand.Next(0, (int)BaseDamage/2);
            target.UpdateHealth(-damage);
            Console.WriteLine($"Player {PlayerIndex} used the Focused Attack in {Target.PlayerIndex} and did {damage} damage\n");
            ShowHealth();
        }
        public void Heal ()
        {
            var rand = new Random();
            int heal = rand.Next(0, (int)BaseDamage);
            UpdateHealth(heal);
            Console.WriteLine($"Player {PlayerIndex} used Magic Heal and did {heal} heal\n");
            ShowHealth();
        }
        public void RandomArrows (Character target)
        {
            var rand = new Random();
            int damage_player = rand.Next(0, (int)BaseDamage/2);
            int damage_target = rand.Next(0, (int)BaseDamage/2);
            UpdateHealth(-damage_player);
            Target.UpdateHealth(-damage_target);
            Console.WriteLine($"Player {PlayerIndex} used Random Arrows to the sky and did {damage_player} / {damage_target} damage\n");
            ShowHealth();
        }

        void ShowHealth(){
            Console.WriteLine("=====================================================");
            float playerLife = PlayerIndex == 0 ? Health : Target.Health;
            float enemyLife = PlayerIndex == 1 ? Health : Target.Health;
            Console.WriteLine($"[Player 0] HP: {playerLife} ---- [Player 1] HP: {enemyLife}");
            Console.WriteLine("=====================================================\n");
        }
    }
}
